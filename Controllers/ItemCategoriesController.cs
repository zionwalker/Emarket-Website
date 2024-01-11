using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Emarket_Website.Models;
using NuGet.Configuration;

namespace Emarket_Website.Controllers
{
    public class ItemCategoriesController : Controller
    {
        private readonly EmarketContext _context;

        public ItemCategoriesController(EmarketContext context)
        {
            _context = context;
           
        }
        public IActionResult AddPossibleValues(int variationTypeId, string value)
        {

            VariationTypePossibleValue variationTypePossibleValue = new VariationTypePossibleValue
            {
                VarId = variationTypeId,
                Value = value
            };
            _context.VariationTypePossibleValues.Add(variationTypePossibleValue);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public IActionResult EditPossibleValue(int variationTypePossibleValueId, string value)
        {
            var variationTypePossibleValue = _context.VariationTypePossibleValues.FirstOrDefault(x => x.Id == variationTypePossibleValueId);
            //var variationTypePossibleValue = _context.VariationTypePossibleValues.Find(variationTypePossibleValueId);

            variationTypePossibleValue.Value = value;
            _context.VariationTypePossibleValues.Update(variationTypePossibleValue);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DeletePossibleValue(int VariationTypePossibleValueId)
        {
            var variationTypePossibleValue = _context.VariationTypePossibleValues.FirstOrDefault(x => x.Id == VariationTypePossibleValueId);

            if (variationTypePossibleValue != null)
            {
                _context.VariationTypePossibleValues.Remove(variationTypePossibleValue);
                _context.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult AddVariationType(int itemCategoryId, string name)
        {

            VariationType variationType = new VariationType
            {
                ItemCatId = itemCategoryId,
                Name = name
            };
            _context.VariationTypes.Add(variationType);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult EditVariationType(int VariationTypeId, string name)
        {
            var variationType = _context.VariationTypes.FirstOrDefault(x => x.Id == VariationTypeId);

            variationType.Name = name;
            _context.VariationTypes.Update(variationType);
            _context.SaveChanges();
            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult DeleteVariationType(int VariationTypeId)
        {
            var variationType = _context.VariationTypes.FirstOrDefault(x => x.Id == VariationTypeId);

            if (variationType != null)
            {
                _context.VariationTypes.Remove(variationType);
                _context.SaveChanges();
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }
        // GET: ItemCategories
        public async Task<IActionResult> Index()
        {
              return _context.ItemCategories != null ? 
                          View(await _context.ItemCategories.ToListAsync()) :
                          Problem("Entity set 'EmarketContext.ItemCategories'  is null.");
        }

        // GET: ItemCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.ItemCategories == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.ItemCategories
                .Include(i => i.VariationTypes)
                .ThenInclude(x => x.VariationTypePossibleValues)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return View(itemCategory);
        }


        // GET: ItemCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ItemCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ItemCategory itemCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(itemCategory);
        }

        // GET: ItemCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ItemCategories == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.ItemCategories.FindAsync(id);
            if (itemCategory == null)
            {
                return NotFound();
            }
            return View(itemCategory);
        }

        // POST: ItemCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] ItemCategory itemCategory)
        {
            if (id != itemCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemCategoryExists(itemCategory.Id))
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
            return View(itemCategory);
        }

        // GET: ItemCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ItemCategories == null)
            {
                return NotFound();
            }

            var itemCategory = await _context.ItemCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (itemCategory == null)
            {
                return NotFound();
            }

            return View(itemCategory);
        }

        // POST: ItemCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ItemCategories == null)
            {
                return Problem("Entity set 'EmarketContext.ItemCategories'  is null.");
            }
            var itemCategory = await _context.ItemCategories.FindAsync(id);
            if (itemCategory != null)
            {
                _context.ItemCategories.Remove(itemCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemCategoryExists(int id)
        {
          return (_context.ItemCategories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
