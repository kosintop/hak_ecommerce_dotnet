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
using eCommerceDotNet.Helpers;

namespace eCommerceDotNet.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Role.Admin)]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.MainImgUrl = FileHelper.UploadFile(product.IFormMainImageFile);
                _context.Add(product);

                if(product.IFormMoreImageFile != null)
                {
                    foreach (var file in product.IFormMoreImageFile)
                    {
                        ProductImage newImage = new ProductImage();
                        newImage.Product = product;
                        newImage.ImgUrl = FileHelper.UploadFile(file);
                        _context.Add(newImage);
                    }
                }

                ProductVariant defaultVariant = new ProductVariant();
                defaultVariant.Name = "Default";
                defaultVariant.Product = product;
                _context.Add(defaultVariant);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(m=>m.ProductImages)
                .Include(m=>m.ProductVariants)
                .SingleOrDefaultAsync(m=>m.ProductId==id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["Error"] = TempData["Error"];
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product)
        {

            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);

                    if (product.IFormMainImageFile != null)
                    {
                        product.MainImgUrl = FileHelper.UploadFile(product.IFormMainImageFile);
                    }
                    else
                    {
                        //dont update imgurl
                        _context.Entry(product).Property(m => m.MainImgUrl).IsModified = false;
                    }

                    if (product.IFormMoreImageFile != null)
                    {
                        foreach (var file in product.IFormMoreImageFile)
                        {
                            ProductImage newImage = new ProductImage();
                            newImage.Product = product;
                            newImage.ImgUrl = FileHelper.UploadFile(file);
                            _context.Add(newImage);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> AddVariant(int id, ProductVariant variant)
        {
            var newVariant = new ProductVariant();
            newVariant.AdditionalPrice = variant.AdditionalPrice;
            newVariant.Name = variant.Name;
            newVariant.ProductId = variant.ProductId;
            _context.Add(newVariant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit",new { id });
        }

        public async Task<IActionResult> UpdateVariant(int id, ProductVariant variant)
        {
            _context.Update(variant);
            await _context.SaveChangesAsync();
            return RedirectToAction("Edit", new { id });
        }

        public async Task<IActionResult> DeleteVariant(int id, int variantId)
        {
            var variants = _context.ProductVariant.Where(m => m.ProductId == id);
            if (variants.Count() == 1)
            {
                TempData["Error"] = "Product must has at least 1 variant";
            }
            else
            {
                var variant = _context.ProductVariant.SingleOrDefault(m => m.ProductVariantId == variantId);
                _context.Remove(variant);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction("Edit", new { id });
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id, int imageId)
        {
            var productImage = await _context.ProductImage.FindAsync(imageId);
            var path = productImage.ImgUrl;
            _context.ProductImage.Remove(productImage);
            await _context.SaveChangesAsync();
            FileHelper.DeleteFile(path);
            return RedirectToAction(nameof(Edit),new { id });
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
