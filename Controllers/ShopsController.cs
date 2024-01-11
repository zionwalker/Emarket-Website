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
    public class ShopsController : Controller
    {
        private readonly EmarketContext _context;
		private readonly IWebHostEnvironment _hostingEnvironment;
		public ShopsController(EmarketContext context, IWebHostEnvironment hostingEnvironment)
		{
			_context = context;
			_hostingEnvironment = hostingEnvironment;
		}


		[HttpPost]
        public IActionResult AddShopEntry(int ShopId, int itemId, int EntryQuantity)
        {
            ShopEntry shopEntry = new ShopEntry
            {
                ItemId = itemId,
                Quantity = EntryQuantity,
                EntryDate = DateTime.Now,
                ShopId = ShopId
            };
            _context.ShopEntries.Add(shopEntry);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult AddShopKeeper(int ShopId, int UserId)
        {
            ShopKeeper shopKeeper = new ShopKeeper
            {
                UserId = UserId,

                AssignDate = DateTime.Now,
                ShopId = ShopId

            };
            _context.ShopKeepers.Add(shopKeeper);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }


       
        public async Task<IActionResult> Index()
        {
            int UserId = HttpContext.Session.GetInt32("UserId") ??0;
            var eMarketContext = _context.Shops
                .Include(s => s.City)
                .Include(s => s.ShopCat)
                .Include(s => s.User)
                .Include(x => x.ShopKeepers)
                .Where(s => s.UserId == UserId || s.ShopKeepers.Any(x => x.UserId == UserId));
            return View(await eMarketContext.ToListAsync());
        }

      
        public async Task<IActionResult> Details(int? id)
        {

            var userRole = HttpContext.Session.GetString("roles");
            ViewBag.UserRole = userRole.Contains("Shopkeeper") ? "Shopkeeper" : "XXX";

            ViewData["ItemId"] = new SelectList(_context.Items, "Id", "Name");
            ViewData["VariationTypePossibleValueId"] = new SelectList(_context.VariationTypePossibleValues, "Id", "Value");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                
                .Include(s => s.City)
                .Include(s => s.ShopCat)
                .Include(s => s.User)
                .Include(s => s.ShopEntries).ThenInclude(x => x.ShopEntryVariations).ThenInclude(x => x.Var).ThenInclude(x => x.VariationTypePossibleValues)
                .Include(s => s.ShopKeepers).ThenInclude(x => x.User)
                .Include(s=>s.ShopEntries).ThenInclude(i=>i.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

      public async Task<IActionResult> Myshop()
        {
			ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
			ViewData["ShopCatId"] = new SelectList(_context.ShopCategories, "Id", "Name");
			int UserId = HttpContext.Session.GetInt32("UserId") ?? 0;
			var eMarketContext = _context.Shops
				.Include(s => s.City)
				.Include(s => s.ShopCat)
				.Include(s => s.User)
				.Include(x => x.ShopKeepers)
				.Where(s => s.UserId == UserId || s.ShopKeepers.Any(x => x.UserId == UserId));
			return View(await eMarketContext.ToListAsync());
		}
		[HttpPost]
		public IActionResult UploadImage(IFormFile file, int id)
		{

			return RedirectToAction(nameof(Details), new { id });
		}


		public void Upload(IFormFile file, int id)
		{
			try
			{
				string wwwPath = this._hostingEnvironment.WebRootPath;


				string relPath = Path.Combine("uploads", "shop", id.ToString());
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
					file.CopyToAsync(stream);
				}

				string url = Path.Combine(relPath, newFileName);

				var shop = _context.Shops.Find(id);
				if (shop != null)
				{
					shop.Url = "~/" + url;
					_context.Update(shop);
					_context.SaveChanges();
				}
			}
			catch (Exception)
			{
				throw;
			}

			{
				
			}
		}
       

		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShop(String Name,int ShopCatId, int CityId, string Address, IFormFile file)
        {
            int userId = HttpContext.Session.GetInt32("UserId")??0;
            int shopCount = await _context.Shops.Where(s => s.UserId == userId).CountAsync();

            if (shopCount >= 3)
            {
                TempData["Error"] = "You have already created the maximum of 3 shops. Please contact us if you need more.";
                return Redirect(Request.Headers["Referer"].ToString());

            }
            else
            {
                Shop shop = new Shop
                {
                    UserId = userId,
                    Name = Name,
                    ShopCatId = ShopCatId,
                    Address = Address,
                    CityId = CityId,
					Url = "No Image"
			};
				_context.Shops.Add(shop);
				 await _context.SaveChangesAsync();
				Upload(file, shop.Id);
				return Redirect(Request.Headers["Referer"].ToString());

			}
		}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }

            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", shop.CityId);
            ViewData["ShopCatId"] = new SelectList(_context.ShopCategories, "Id", "Name", shop.ShopCatId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", shop.UserId);
            return View(shop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int shopId, string Name, string Address, int CityId, IFormFile file)
        {
            if (id != shopId)
            {
                return NotFound();
            }

            var shop = await _context.Shops.FindAsync(shopId);

            if (shop != null)
            {
                try
                {
                    shop.Name = Name;
                    shop.Address = Address;
                    shop.CityId = CityId;
                    shop.Url = "no image";

                    _context.Update(shop);
                    await _context.SaveChangesAsync();

                    
                    if (file != null)
                    {
                        Upload(file, shopId);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    return View("Error"); // Display an error view or redirect to an error page
                }
            }

            // If shop is not found, handle it appropriately
            return NotFound();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEntry(int shopEntryId, int itemId, double entryQuantity)
        {
            try
            {
                var roles = HttpContext.Session.GetString("roles");
                if (roles.Contains("Seller") || roles.Contains("ShopKeeper"))
                {
                    var shopEntry = await _context.ShopEntries.FindAsync(shopEntryId);

                    if (shopEntry != null)
                    {
                        shopEntry.ItemId = itemId;
                        shopEntry.Quantity = entryQuantity;

                        _context.ShopEntries.Update(shopEntry);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Myshop));
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShopkeeper(int shopKeeperId, int userId)
        {
            
            try
            {
                var roles = HttpContext.Session.GetString("roles");
                if (roles.Contains("Seller"))
                {
                    var shopKeeper = await _context.ShopKeepers.FindAsync(shopKeeperId);

                    if (shopKeeper != null)
                    {
                        shopKeeper.UserId = userId;
                        _context.ShopKeepers.Update(shopKeeper);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Myshop));
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DeleteShopkeeper(int ShopKeeperId)
        {
            var shopKeeper = _context.ShopKeepers.FirstOrDefault(x => x.Id == ShopKeeperId);

            if (shopKeeper != null)
            {
                _context.ShopKeepers.Remove(shopKeeper);
                _context.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DeleteEntry(int ShopEntryId)
        {
            var shopEntry = _context.ShopEntries.FirstOrDefault(x => x.Id == ShopEntryId);

            if (shopEntry != null)
            {
                _context.ShopEntries.Remove(shopEntry);
                _context.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shops == null)
            {
                return NotFound();
            }

            var shop = await _context.Shops
                .Include(s => s.City)
                .Include(s => s.ShopCat)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            return View(shop);
        }

        // POST: Shops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Shops == null)
            {
                return Problem("Entity set 'EmarketContext.Shops'  is null.");
            }
            var shop = await _context.Shops.FindAsync(id);
            if (shop != null)
            {
                _context.Shops.Remove(shop);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Myshop));
        }

        private bool ShopExists(int id)
        {
            return (_context.Shops?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
