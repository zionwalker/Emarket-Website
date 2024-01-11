using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emarket_Website.Models;
using Microsoft.Extensions.Hosting.Internal;

namespace Emarket_Website.Controllers
{
    public class ShopCategoriesController : Controller
    {
        private readonly EmarketContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ShopCategoriesController(EmarketContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: ShopCategories
        public async Task<IActionResult> Index()
        {
              return _context.ShopCategories != null ? 
                          View(await _context.ShopCategories.ToListAsync()) :
                          Problem("Entity set 'EmarketContext.ShopCategories'  is null.");
        }

        // GET: ShopCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopCategories == null)
            {
                return NotFound();
            }

            var shopCategory = await _context.ShopCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopCategory == null)
            {
                return NotFound();
            }

            return View(shopCategory);
        }
        private void UploadImage(IFormFile file, int itemId)
        {
            string wwwPath = _hostingEnvironment.WebRootPath;

            string relPath = Path.Combine("uploads", "item", itemId.ToString());
            string filePath = Path.Combine(wwwPath, relPath);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string fullPath = Path.Combine(filePath, newFileName);

            if (System.IO.File.Exists(fullPath))
            {
                throw new IOException("A file with the same name already exists.");
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
            {
                file.CopyTo(stream);
            }

            string url = Path.Combine(relPath, newFileName);

            ItemImage itemImage = new ItemImage
            {
                
                Url = "~/" + url,
            };
            _context.ItemImages.Add(itemImage);
            _context.SaveChanges();
        }

        // GET: ShopCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShopCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Url")] ShopCategory shopCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopCategory);
        }

        // GET: ShopCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopCategories == null)
            {
                return NotFound();
            }

            var shopCategory = await _context.ShopCategories.FindAsync(id);
            if (shopCategory == null)
            {
                return NotFound();
            }
            return View(shopCategory);
        }

        // POST: ShopCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Url")] ShopCategory shopCategory)
        {
            if (id != shopCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopCategoryExists(shopCategory.Id))
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
            return View(shopCategory);
        }

        // GET: ShopCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopCategories == null)
            {
                return NotFound();
            }

            var shopCategory = await _context.ShopCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopCategory == null)
            {
                return NotFound();
            }

            return View(shopCategory);
        }

        // POST: ShopCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopCategories == null)
            {
                return Problem("Entity set 'EmarketContext.ShopCategories'  is null.");
            }
            var shopCategory = await _context.ShopCategories.FindAsync(id);
            if (shopCategory != null)
            {
                _context.ShopCategories.Remove(shopCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopCategoryExists(int id)
        {
          return (_context.ShopCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
