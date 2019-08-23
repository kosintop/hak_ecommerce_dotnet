using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eCommerceDotNet.Data;
using eCommerceDotNet.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace eCommerceDotNet.Controllers
{

    [Authorize(Roles=Role.Customer)]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItems = _context.CartItem.Where(m => m.ApplicationUserId == userId)
                .Include(m=>m.ProductVariant)
                .ThenInclude(m=>m.Product);

            return View(cartItems);
        }

        public async Task<IActionResult> AddToCart(CartItem item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            item.ApplicationUserId = userId;
            _context.Add(item);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = _context.CartItem.SingleOrDefault(m => m.CartItemId == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            if(cartItem.ApplicationUserId != userId)
            {
                return Unauthorized();
            }

            _context.Remove(cartItem);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartItems = _context.CartItem
            .Include(m=> m.ProductVariant)
            .ThenInclude(m => m.Product);

            var newOrder = new Order();
            newOrder.ApplicationUserId = userId;
            newOrder.DateCreated = DateTime.Now;

            double totalAmount = 0;

            foreach(CartItem item in cartItems)
            {
                var orderItems = new OrderItem();
                orderItems.Order = newOrder;
                orderItems.ProductVariantId = item.ProductVariantId;
                orderItems.ProductCount = item.ProductCount;
                _context.Add(orderItems);

                var amount = (item.ProductVariant.Product.Price + item.ProductVariant.AdditionalPrice) * item.ProductCount;
                totalAmount += amount;
            }

            newOrder.TotalAmount = totalAmount;

            _context.Add(newOrder);
            _context.RemoveRange(cartItems);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index),"Home");
        }
    }
}
