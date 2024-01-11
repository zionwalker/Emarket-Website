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
    public class SoldItemsController : Controller
    {
        private readonly EmarketContext _context;

        public SoldItemsController(EmarketContext context)
        {
            _context = context;
        }

        // GET: SoldItems
        public async Task<IActionResult> Index()
        {
            var emarketContext = _context.SoldItems
                .Include(s => s.ShopEntry)
                .ThenInclude(s => s.Item);
            return View(await emarketContext.ToListAsync());
        }

        // GET: SoldItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SoldItems == null)
            {
                return NotFound();
            }

            var soldItem = await _context.SoldItems
                .Include(s => s.ShopEntry)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soldItem == null)
            {
                return NotFound();
            }

            return View(soldItem);
        }

        // GET: SoldItems/Create
        public IActionResult Create()
        {
            ViewData["ShopEntryId"] = new SelectList(_context.ShopEntries, "Id", "Id");
            return View();
        }

        // POST: SoldItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,Price,ShopEntryId")] SoldItem soldItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(soldItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShopEntryId"] = new SelectList(_context.ShopEntries, "Id", "Id", soldItem.ShopEntryId);
            return View(soldItem);
        }

        // GET: SoldItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SoldItems == null)
            {
                return NotFound();
            }

            var soldItem = await _context.SoldItems.FindAsync(id);
            if (soldItem == null)
            {
                return NotFound();
            }
            ViewData["ShopEntryId"] = new SelectList(_context.ShopEntries, "Id", "Id", soldItem.ShopEntryId);
            return View(soldItem);
        }

        // POST: SoldItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,Price,ShopEntryId")] SoldItem soldItem)
        {
            if (id != soldItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(soldItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoldItemExists(soldItem.Id))
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
            ViewData["ShopEntryId"] = new SelectList(_context.ShopEntries, "Id", "Id", soldItem.ShopEntryId);
            return View(soldItem);
        }

        // GET: SoldItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SoldItems == null)
            {
                return NotFound();
            }

            var soldItem = await _context.SoldItems
                .Include(s => s.ShopEntry)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (soldItem == null)
            {
                return NotFound();
            }

            return View(soldItem);
        }

        // POST: SoldItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SoldItems == null)
            {
                return Problem("Entity set 'EmarketContext.SoldItems'  is null.");
            }
            var soldItem = await _context.SoldItems.FindAsync(id);
            if (soldItem != null)
            {
                _context.SoldItems.Remove(soldItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoldItemExists(int id)
        {
          return (_context.SoldItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
