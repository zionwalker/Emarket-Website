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
    public class ReviewShopsController : Controller
    {
        private readonly EmarketContext _context;

        public ReviewShopsController(EmarketContext context)
        {
            _context = context;
        }

        // GET: ReviewShops
        public async Task<IActionResult> Index()
        {
            var emarketContext = _context.ReviewShops.Include(r => r.Shop).Include(r => r.User);
            return View(await emarketContext.ToListAsync());
        }

        // GET: ReviewShops/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReviewShops == null)
            {
                return NotFound();
            }

            var reviewShop = await _context.ReviewShops
                .Include(r => r.Shop)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviewShop == null)
            {
                return NotFound();
            }

            return View(reviewShop);
        }
        // GET: ReviewShops/Create
        public IActionResult ReviewShop(int id)
        {
            


            var reviewShop = _context.ReviewShops
                .Include(r => r.Shop)
                .Include(r => r.User)
                .Where(r => r.ShopId == id)
                .ToList();

            ViewBag.ShopId = id;
            ViewBag.ReviewShops = reviewShop;

            return View();
        }


        [HttpPost]
        public IActionResult SubmitReviewShop(int ShopId, int RatingValue, string Comment)
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            ReviewShop reviewShop = new ReviewShop
            {

                UserId = userId,
                ShopId = ShopId,
                RatingValue = RatingValue,
                RatedDate = DateTime.Now,
                Comment = Comment,


            };


            _context.ReviewShops.Add(reviewShop);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());

        }

        // GET: ReviewShops/Create
        public IActionResult Create()
        {
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ReviewShops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ShopId,RatingValue,RatedDate,Comment")] ReviewShop reviewShop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reviewShop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", reviewShop.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reviewShop.UserId);
            return View(reviewShop);
        }

        // GET: ReviewShops/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReviewShops == null)
            {
                return NotFound();
            }

            var reviewShop = await _context.ReviewShops.FindAsync(id);
            if (reviewShop == null)
            {
                return NotFound();
            }
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", reviewShop.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reviewShop.UserId);
            return View(reviewShop);
        }

        // POST: ReviewShops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ShopId,RatingValue,RatedDate,Comment")] ReviewShop reviewShop)
        {
            if (id != reviewShop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reviewShop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewShopExists(reviewShop.Id))
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
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", reviewShop.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", reviewShop.UserId);
            return View(reviewShop);
        }

        // GET: ReviewShops/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReviewShops == null)
            {
                return NotFound();
            }

            var reviewShop = await _context.ReviewShops
                .Include(r => r.Shop)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reviewShop == null)
            {
                return NotFound();
            }

            return View(reviewShop);
        }

        // POST: ReviewShops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReviewShops == null)
            {
                return Problem("Entity set 'EmarketContext.ReviewShops'  is null.");
            }
            var reviewShop = await _context.ReviewShops.FindAsync(id);
            if (reviewShop != null)
            {
                _context.ReviewShops.Remove(reviewShop);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewShopExists(int id)
        {
          return (_context.ReviewShops?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
