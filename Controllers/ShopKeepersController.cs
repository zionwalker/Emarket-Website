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
    public class ShopKeepersController : Controller
    {
        private readonly EmarketContext _context;

        public ShopKeepersController(EmarketContext context)
        {
            _context = context;
        }

        // GET: ShopKeepers
        public async Task<IActionResult> Index()
        {
            var emarketContext = _context.ShopKeepers.Include(s => s.Shop).Include(s => s.User);
            return View(await emarketContext.ToListAsync());
        }

        // GET: ShopKeepers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShopKeepers == null)
            {
                return NotFound();
            }

            var shopKeeper = await _context.ShopKeepers
                .Include(s => s.Shop)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopKeeper == null)
            {
                return NotFound();
            }

            return View(shopKeeper);
        }

        // GET: ShopKeepers/Create
        public IActionResult Create()
        {
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: ShopKeepers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShopId,UserId,AssignedBy,AssignDate")] ShopKeeper shopKeeper)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopKeeper);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Name", shopKeeper.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", shopKeeper.UserId);
            return View(shopKeeper);
        }

        // GET: ShopKeepers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShopKeepers == null)
            {
                return NotFound();
            }

            var shopKeeper = await _context.ShopKeepers.FindAsync(id);
            if (shopKeeper == null)
            {
                return NotFound();
            }
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Name`", shopKeeper.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", shopKeeper.UserId);
            return View(shopKeeper);
        }

        // POST: ShopKeepers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShopId,UserId,AssignedBy,AssignDate")] ShopKeeper shopKeeper)
        {
            if (id != shopKeeper.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopKeeper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopKeeperExists(shopKeeper.Id))
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
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", shopKeeper.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", shopKeeper.UserId);
            return View(shopKeeper);
        }

        // GET: ShopKeepers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShopKeepers == null)
            {
                return NotFound();
            }

            var shopKeeper = await _context.ShopKeepers
                .Include(s => s.Shop)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopKeeper == null)
            {
                return NotFound();
            }

            return View(shopKeeper);
        }

        // POST: ShopKeepers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShopKeepers == null)
            {
                return Problem("Entity set 'EmarketContext.ShopKeepers'  is null.");
            }
            var shopKeeper = await _context.ShopKeepers.FindAsync(id);
            if (shopKeeper != null)
            {
                _context.ShopKeepers.Remove(shopKeeper);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopKeeperExists(int id)
        {
          return (_context.ShopKeepers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
