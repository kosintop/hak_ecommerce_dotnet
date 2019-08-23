using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eCommerceDotNet.Data;
using eCommerceDotNet.Models;

namespace eCommerceDotNet.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Details(int id)
        {
            var product = _context.Product
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                .Include(p => p.ProductImages)
                .SingleOrDefault(m=>m.ProductId==id);

            if (product == null) return NotFound();

            //ViewBag.VariantList = new SelectList(product.ProductVariants, "ProductVariantId", "Name");

            ViewBag.VariantList = product.ProductVariants;

            return View(product);
        }
    }
}
