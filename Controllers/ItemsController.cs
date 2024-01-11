using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emarket_Website.Models;

namespace Emarket_Website.Controllers
{
    public class ItemsController : Controller
    {
        private readonly EmarketContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ItemsController(EmarketContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            var emarketContext = _context.Items.Include(i => i.ItemCat);
            return View(await emarketContext.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
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
                ItemId = itemId,
                Url = "~/" + url,
            };
            _context.ItemImages.Add(itemImage);
            _context.SaveChanges();
        }

        public IActionResult AddItem()
        {
            ViewData["ItemCatId"] = new SelectList(_context.ItemCategories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(int ItemCatId, string Name, string Descripition, double Quantity, double Price, IFormFile ImageFile)
        {
            var itemCategory = _context.ItemCategories
                .Include(ic => ic.Items)
                    .ThenInclude(i => i.ItemPrices)
                .Include(ic => ic.Items)
                    .ThenInclude(i => i.ItemImages)
                .FirstOrDefault(ic => ic.Id == ItemCatId);

            int itemId;

            if (itemCategory != null)
            {
                var item = new Item
                {
                    ItemCatId = ItemCatId,
                    Name = Name,
                    Descripition = Descripition,
                    Quantity = Quantity
                };
                _context.Items.Add(item);
                _context.SaveChanges();

                itemId = item.Id;
                var itemPrice = new ItemPrice
                {
                    ItemId = itemId,
                    Price = Price
                };
               
                item.ItemPrices.Add(itemPrice);
                await _context.SaveChangesAsync();

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    UploadImage(ImageFile, item.Id);
                }

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["ItemCatId"] = new SelectList(_context.ItemCategories, "Id", "Id", item.ItemCatId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemCatId,Name,Descripition,Quantity")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["ItemCatId"] = new SelectList(_context.ItemCategories, "Id", "Id", item.ItemCatId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.ItemCat)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Items == null)
            {
                return Problem("Entity set 'EmarketContext.Items'  is null.");
            }
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
