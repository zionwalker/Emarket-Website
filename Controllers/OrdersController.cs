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
    public class OrdersController : Controller
    {
        private readonly EmarketContext _context;

        public OrdersController(EmarketContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            int UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var emarketContext = _context.Orders
                .Where(o => o.Shop.UserId == UserId)
                .Include(o => o.OrderStat)
                .Include(o => o.Shop)
                .Include(o => o.User);

            return View(await emarketContext.ToListAsync());
        }
        public async Task<IActionResult> UserOrder()
        {
            int UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var emarketContext = _context.Orders
                .Where(o => o.UserId == UserId)
                .Include(o => o.OrderStat)
                .Include(o => o.Shop);

            return View(await emarketContext.ToListAsync());
        }

        public async Task<IActionResult> UserOrderDetails(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderStat)
                .Include(o => o.Shop)
                .Include(o => o.Ordereditems)
                .ThenInclude(o => o.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderStat)
                .Include(o => o.Shop)
                .Include(o => o.User)
                .Include(o => o.Ordereditems)
                .ThenInclude(o => o.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public ActionResult OrderInformation(int shopEntryId)
        {
           
            var shopEntries = _context.ShopEntries
                .Include(x => x.Shop)
                .Include(se => se.Item)
                    .ThenInclude(item => item.ItemImages)
                    .Include(se => se.Item.ItemPrices)
                    .Include(se => se.Item.Ordereditems)
                .FirstOrDefault(se => se.Id == shopEntryId);
          

            return View(shopEntries);
        }


		public ActionResult AcceptOrder(int orderId)
		{
			var order = _context.Orders
				.Include(o => o.Ordereditems)
				.ThenInclude(oi => oi.Item)
				.ThenInclude(item => item.ShopEntries)
				.FirstOrDefault(o => o.Id == orderId);

			if (order == null)
			{
				return NotFound();
			}

			order.OrderStatId = 1002;

			foreach (var orderedItem in order.Ordereditems)
			{
				var shopEntry = orderedItem.Item.ShopEntries.FirstOrDefault();
				if (shopEntry != null)
				{
					_context.SoldItems.Add(new SoldItem
					{
						Quantity = orderedItem.Quantity,
						Price = orderedItem.SubTotalPrice,
						ShopEntryId = shopEntry.Id
					});
				}
			}

			_context.SaveChanges();

			return RedirectToAction("Details", new { id = orderId });
		}




		// GET: Orders/Create
		public IActionResult Create()
        {
            ViewData["OrderStatId"] = new SelectList(_context.OrderStatusses, "Id", "Id");
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,OrderDate,OrderStatId,TotalPrice,ShopId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderStatId"] = new SelectList(_context.OrderStatusses, "Id", "Id", order.OrderStatId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", order.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["OrderStatId"] = new SelectList(_context.OrderStatusses, "Id", "Id", order.OrderStatId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", order.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,OrderDate,OrderStatId,TotalPrice,ShopId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["OrderStatId"] = new SelectList(_context.OrderStatusses, "Id", "Id", order.OrderStatId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id", order.ShopId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderStat)
                .Include(o => o.Shop)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'EmarketContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
