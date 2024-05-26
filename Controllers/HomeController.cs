using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Emarket_Website.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using EMarket.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using SQLitePCL;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;
using NuGet.Protocol.Plugins;
using Microsoft.Extensions.Hosting.Internal;
using System.Net;
using System.Security.Policy;

namespace Emarket_Website.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EmarketContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public double TotalPrice { get; private set; }

        public HomeController(ILogger<HomeController> logger,EmarketContext context, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        
        public IActionResult Register()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,City,Email,Password,IsActive")] User user)
        {

            if (ModelState.IsValid)
            {
                user.Password = Encrypt(user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                AddBuyerRole(user.Id, 3);
                ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", user.CityId);
                return RedirectToAction(nameof(Login));
            }
            return View(user);
        }

        private void AddBuyerRole(int UserId, int RoleId)
        {
            UserRole userRole = new UserRole();
            userRole.UserId = UserId;
            userRole.RoleId = RoleId;
            _context.UserRoles.Add(userRole);
            _context.SaveChanges(true);

        }
        public IActionResult Admin()
        {
            return View();
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
       
        public IActionResult Login(string email, string password)
        {
            var hashedPassword = Encrypt(password);

            var user = _context.Users
                .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                .FirstOrDefault(x => x.Email == email && x.Password == hashedPassword);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);

                if (user.UserRoles.Any())
                {
                    var roleString = string.Join(",", user.UserRoles.Select(ur => ur.Role.Rname));
                    HttpContext.Session.SetString("roles", roleString);

                    if (user.UserRoles.Any(ur => ur.Role.Rname == "Seller"))
                    {
                        TempData["Message"] = "Ok - Seller";
                        return RedirectToAction(nameof(Seller));
                    }
					
				}

                TempData["Message"] = "Ok";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Message"] = "Error";
                return RedirectToAction(nameof(Login));
            }
        }


        private string Encrypt(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
		public IActionResult SuperAdmin()
		{
			return View();
		}



		public IActionResult Seller()
		{
			int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

			var shopId = _context.Shops
				.Where(s => s.UserId == userId)
				.Select(s => s.Id)
				.FirstOrDefault();

			var recentOrders = _context.Orders
				.Include(o => o.User)
				.Include(o => o.OrderStat)
				.Where(o => o.ShopId == shopId)
				.OrderByDescending(o => o.OrderDate)
                .Take(5)
				.ToList();

			 double Quantity = _context.ShopEntries
	               .Where(se => se.ShopId == shopId)
	                .Sum(se => se.Quantity);

			
			int numberOfOrders = recentOrders.Count;
			var numberOfSoldItems = _context.SoldItems.Count();
			ViewBag.NumberOfSoldItems = numberOfSoldItems;
            var NumberOfOrders = _context.Orders.Count();
            ViewBag.NumberOfOrders = NumberOfOrders;
			double remainingQuantity = Quantity - numberOfSoldItems;
			ViewBag.NumberInstock = remainingQuantity;
            double TotalQuantity = numberOfSoldItems + remainingQuantity;
			int Percentage = (int)(remainingQuantity / TotalQuantity * 100);
            ViewBag.percentage = Percentage;


			return View(recentOrders);
		}

		
		public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Guest", "Home");
        }

        public IActionResult Index(int? pageNumber, int? pageSize)
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            var cats = _context.ShopCategories.ToList();
            var shops = _context.Shops.Include(x => x.ShopEntries).ToList();

            
            pageNumber = pageNumber ?? 1;
            pageSize = pageSize ?? 10;
            int totalRecords = shops.Count();
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize.Value);
            ViewBag.PageNumber = pageNumber;
            ViewBag.TotalPages = totalPages;
          

            shops = shops.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList(); // Fix the syntax error here

            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.shopCatagorIES = cats;
            data.shopS = shops.ToList();
            return View(data);
        }

        public IActionResult Guest()
        {

            var cats = _context.ShopCategories.ToList();
            var shops = _context.Shops.ToList();
            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.shopCatagorIES = cats;
            data.shopS = shops;
            return View(data);


        }
     
        public IActionResult Shop()
        {
            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            var shops = _context.Shops.ToList();
            data.shopS = shops;

            
            return View(data);

        }

        public IActionResult ShopEntry(int id)
        {
            var shopEntries = _context.ShopEntries
                .Include(x => x.Shop)
                .Include(se => se.Item)
                    .ThenInclude(item => item.ItemImages)
                    .Include(se => se.Item.ItemPrices) 
                .Where(se => se.ShopId == id)
                .ToList();

            var viewModel = new ViewModels.HomeViewModel
            {
                ShopEntries = shopEntries
              
            };

            return View(viewModel);
        }
       
        public IActionResult ShopCategory(int Id, int? cityId)
        {
            
            var viewModel = new ViewModels.HomeViewModel
            {
                ShopCatId = Id
            };

            
            viewModel.shopS = _context.Shops
                .Where(s => s.ShopCatId == Id && s.CityId == cityId)
                .ToList();

            var shops= _context.Shops
                .Where(s => s.ShopCatId == Id )
                .ToList();
            if (cityId != null)
            {
                shops = shops
                    .Where(s => s.CityId == cityId)
                .ToList();
            }
            viewModel.shopS = shops;


            viewModel.shopCatagorIES = _context.ShopCategories.ToList();
            viewModel.cities = _context.Cities.ToList();
            

            return View(viewModel);
        }


        public IActionResult AddToCart(int shopEntryId)
        {
            var shopEntry = _context.ShopEntries
                .Include(se => se.Item)
                .Include(se => se.Shop)
                .FirstOrDefault(se => se.Id == shopEntryId);



            if (HttpContext.Session.GetInt32("UserId") == null)
            {

                TempData["Message"] = "You need to be logged in to add items to the cart.";
                return RedirectToAction("Login");
            }

            if (shopEntry == null)
            {
                TempData["Message"] = "Error adding item to the cart.";
            }

           

            var existingCartItem = _context.CartItems
                .FirstOrDefault(ci => ci.UserId == (HttpContext.Session.GetInt32("UserId")) && ci.ItemId == shopEntry.ItemId && ci.ShopId == shopEntry.ShopId);

            if (existingCartItem != null)
            {

                existingCartItem.Quantitiy = +1;
                _context.Update(existingCartItem);
                _context.SaveChanges();
            }
            else
            {

                var newCartItem = new CartItem
                {
                    ItemId = shopEntry.ItemId,
                    ShopId = shopEntry.ShopId,
                    Quantitiy = 1,
                    AddedDate = DateTime.Now,
                    CartItemStatusId = 1,
                    UserId = (int)HttpContext.Session.GetInt32("UserId")
                };
                _context.CartItems.Add(newCartItem);
                ViewData["Message"] = "Item added to the cart successfully.";

                _context.SaveChanges();
                
            }

            return RedirectToAction(nameof(Index)); 
        }

        public ActionResult Order(int shopEntryId, int quantity)
        {
            var shopEntry = _context.ShopEntries
                .Include(se => se.Item)
                .FirstOrDefault(se => se.Id == shopEntryId);

            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                TempData["Message"] = "You need to be logged in to order items.";
                return RedirectToAction("Login");
            }

            if (shopEntry == null)
            {
                TempData["Message"] = "Error ordering item to the Shop.";
                return RedirectToAction(nameof(Index));
            }

            int userId = (int)HttpContext.Session.GetInt32("UserId");
            var existingOrder = _context.Orders
                .Include(se => se.Ordereditems)
                .FirstOrDefault(ci => ci.UserId == userId && ci.ShopId == shopEntry.ShopId && ci.OrderStatId == 2);

            double unitPrice = _context.ItemPrices
                .Where(ip => ip.ItemId == shopEntry.ItemId)
                .Select(ip => ip.Price)
                .FirstOrDefault();

            if (unitPrice == 0)
            {
                TempData["Message"] = "Error fetching unit price for the item.";
                return RedirectToAction(nameof(Index));
            }

            double subTotalPrice = unitPrice * quantity;
            int orderId;

            if (existingOrder != null && existingOrder.OrderStatId == 2) 
            {
                orderId = existingOrder.Id;

                if (existingOrder.TotalPrice != null)
                {
                    existingOrder.TotalPrice += subTotalPrice;
                }
                else
                {
                    existingOrder.TotalPrice = subTotalPrice;
                }
            }
            else
            {
                var newOrder = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderStatId = 2,
                    ShopId = shopEntry.ShopId,
                    TotalPrice = subTotalPrice  
                };

                _context.Orders.Add(newOrder);
                _context.SaveChanges();
                orderId = newOrder.Id;
            }

            var orderedItem = new Ordereditem
            {
                ItemId = shopEntry.ItemId,
                Quantity = quantity,
                OrderId = orderId,
                UnitPrice = unitPrice,
                SubTotalPrice = subTotalPrice
            };

            _context.Ordereditems.Add(orderedItem);
            ViewData["Message"] = "Ordered added successfully.";
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Cart()
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var cartItems = _context.CartItems
                .Where(ci => ci.UserId == userId)
                .Include(ci => ci.Item)
                .ThenInclude(ci => ci.ShopEntries)
                .Include(ci => ci.Shop)
                .ToList();
            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.cartItemS = cartItems;
            return View(data);

            }

        public IActionResult RemoveItem(int id)
        {
           
            var cartItemToRemove = _context.CartItems
                .FirstOrDefault(ci => ci.Id == id);

            if (cartItemToRemove != null)
            {
                
                _context.CartItems.Remove(cartItemToRemove);
                _context.SaveChanges();

                TempData["Message"] = "Item removed from the cart successfully.";
            }
            else
            {
                TempData["Message"] = "No Item to remove from the cart ";
            }

            
            return RedirectToAction("Index");
        }

        public IActionResult RemoveOrder(int id)
        {
            var orderToRemove = _context.Orders.FirstOrDefault(ci => ci.Id == id);

            if (orderToRemove != null)
            {
                if (orderToRemove.OrderStatId == 2)
                {
                    var orphanedItems = _context.Ordereditems
                        .Where(oi => oi.OrderId == orderToRemove.Id)
                        .ToList();

                    if (orphanedItems.Any())
                    {
                        _context.Ordereditems.RemoveRange(orphanedItems);
                    }
                    _context.Orders.Remove(orderToRemove);
                    _context.SaveChanges();

                    TempData["Message"] = "Order removed from the MyOrder successfully.";
                }
                else
                {
                    TempData["Message"] = "Sorry can't remove from the MyOrder!";
                }
            }
            else
            {
                TempData["Message"] = "No Order to remove from the MyOrder ";
            }

            return RedirectToAction("Index");
        }


        public IActionResult ViewOrder(int shopEntryId)
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var ordereditems = _context.Ordereditems
                .Where(ci => ci.Order.UserId == userId)
                .Include(ci => ci.Item)
                .Include(ci => ci.Item.ShopEntries)  
                .Include(ci => ci.Order.Shop)
                .Include(ci => ci.Order.OrderStat)
                .ToList();

            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.Ordereditems = ordereditems;
            return View(data);

        }
        public IActionResult Requests()
        {
           
            var requests =_context.SellerApplications
                .Include(ci => ci.User)
                .Include(ci => ci.ApprovedByNavigation)
                .Include(ci =>ci.ApplicationStatus)
                .ToList();
            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.sellerApplications = requests;
            return View(data);


        }
        
            public IActionResult Item()
        {
          
            var items = _context.Items
               .Include(i => i.ItemImages)
               .Include(i => i.ItemPrices)
                  .ToList();

            
            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.itemS = items;
            return View(data);


        }

        public IActionResult Privacy()
        {
            var cats = _context.ShopCategories.ToList();
            ViewModels.HomeViewModel data = new ViewModels.HomeViewModel();
            data.shopCatagorIES = cats;
            return View(data);
        }
     
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}