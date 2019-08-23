using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eCommerceDotNet.Models;
using eCommerceDotNet.Data;

namespace eCommerceDotNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {
            List<Product> product = null;
            if (id != null)
            {
                product = _context.Product.Where(m=>m.CategoryId==id).ToList();
                ViewData["CategoryName"] = _context.Category.SingleOrDefault(m=>m.CategoryId==id)?.CategoryName;
            }
            else
            {
                product = _context.Product.ToList();
            }
            
            return View(product);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
