using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emarket_Website.Models;
using Microsoft.Extensions.Hosting.Internal;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using NuGet.Protocol.Plugins;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
namespace Emarket_Website.Controllers
{
    public class SellerApplicationsController : Controller
    {
        private readonly EmarketContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public SellerApplicationsController(EmarketContext context , IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: SellerApplications
        public async Task<IActionResult> Index()
        {
            var emarketContext = _context.SellerApplications
                .Include(s => s.ApplicationStatus)
                .Include(s => s.ApprovedByNavigation)
                .Include(s => s.User);
            return View(await emarketContext.ToListAsync());
        }

        // GET: SellerApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SellerApplications == null)
            {
                return NotFound();
            }
            var sellerApplication = await _context.SellerApplications
                .Include(s => s.ApplicationStatus)
                .Include(s => s.ApprovedByNavigation)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (sellerApplication == null)
            {
                return NotFound();
            }

            return View(sellerApplication);
        }

       
        public void Upload(IFormFile file, int id)
        {
            try
            {
                string wwwPath = this._hostingEnvironment.WebRootPath;


                string relPath = Path.Combine("uploads", id.ToString());
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

                var seller = _context.SellerApplications.Find(id);
                if (seller != null)
                {
                    seller.Url = "~/" + url;
                    _context.Update(seller);
                    _context.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult Create()
        {
            ViewData["ApplicationStatusId"] = new SelectList(_context.ApplicationStatuses, "Id", "Id");
            ViewData["ApprovedBy"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSeller(string Address, int NumberOfShops, string Description, IFormFile file)
        {


            int UserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            SellerApplication sellerApplication = new SellerApplication
            {
                UserId = UserId,
                Address = Address,
                NumberOfShops = NumberOfShops,
                Description = Description,
                ApplicationStatusId = 1,
                RequestedDate = DateTime.Now,
                Url = "No Image"
            };

            _context.Add(sellerApplication);
            await _context.SaveChangesAsync();
            Upload(file, sellerApplication.Id);
            return RedirectToAction("Index", "Home");
        }

        private void AddSellerRole(int UserId, int RoleId)
        {
            UserRole userRole = new UserRole();
            userRole.UserId = UserId;
            userRole.RoleId = RoleId;
            _context.UserRoles.Add(userRole);
            _context.SaveChanges(true);

        }
        public async Task<IActionResult> AcceptSeller(int id)
        {

            int approvedBy = HttpContext.Session.GetInt32("UserId") ?? 0;


            var sellerApplication = await _context.SellerApplications.FindAsync(id);


            if (sellerApplication == null)
            {

                return NotFound();
            }


            sellerApplication.ApprovedBy = approvedBy;
            sellerApplication.AcceptedDate = DateTime.Now;
            sellerApplication.ApplicationStatusId = 2;


            _context.Update(sellerApplication);
            await _context.SaveChangesAsync();

            AddSellerRole(sellerApplication.UserId, 2);


            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> RejectSeller(int id)
        {
            int approvedBy = HttpContext.Session.GetInt32("UserId") ?? 0;


            var sellerApplication = await _context.SellerApplications.FindAsync(id);


            if (sellerApplication == null)
            {

                return NotFound();
            }

            sellerApplication.ApprovedBy = approvedBy;
            sellerApplication.AcceptedDate = DateTime.Now;
            sellerApplication.ApplicationStatusId = 3;

            _context.Update(sellerApplication);
            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }

       
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SellerApplications == null)
            {
                return NotFound();
            }

            var sellerApplication = await _context.SellerApplications
                .Include(s => s.ApplicationStatus)
                .Include(s => s.ApprovedByNavigation)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sellerApplication == null)
            {
                return NotFound();
            }

            return View(sellerApplication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SellerApplications == null)
            {
                return Problem("Entity set 'EmarketContext.SellerApplications'  is null.");
            }
            var sellerApplication = await _context.SellerApplications.FindAsync(id);
            if (sellerApplication != null)
            {
                _context.SellerApplications.Remove(sellerApplication);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Requests","Home");
        }

        private bool SellerApplicationExists(int id)
        {
          return (_context.SellerApplications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
