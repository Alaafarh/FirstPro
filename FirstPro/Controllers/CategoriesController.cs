using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstPro.Models;

namespace FirstPro.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
              return _context.Categories != null ? 
                          View(await _context.Categories.ToListAsync()) :
                          Problem("Entity set 'ModelContext.Categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Categoryid == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Categoryid,Categoryname,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + category.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        category.ImageFile.CopyToAsync(fileStream);
                    }
                    category.Imagepath = imageName;
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Categoryid,Categoryname,ImageFile")] Category category)
        {
            if (id != category.Categoryid)
            {
                return NotFound();
            }
            //var CatIMG = await _context.Categories.Where(x => x.Categoryid == id).FirstOrDefaultAsync();
            //category.Imagepath = CatIMG.Imagepath;

            if (category.ImageFile != null)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + category.ImageFile.FileName;
                string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    category.ImageFile.CopyToAsync(fileStream);
                }
                category.Imagepath = imageName;
            }
            //_context.Entry(CatIMG).State = EntityState.Detached;
            //_context.Categories.AsNoTracking().SingleOrDefault(u => u.Categoryid == category.Categoryid);


            _context.Update(category);
                    await _context.SaveChangesAsync();
               
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }


            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Categoryid == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ModelContext.Categories'  is null.");
            }
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(decimal id)
        {
          return (_context.Categories?.Any(e => e.Categoryid == id)).GetValueOrDefault();
        }
        public IActionResult Categorypage()
        {
            //var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.User);
            //ViewBag.Categoryid = new SelectList(_context.Categories, "Categoryid", "Categoryname");
            //ViewBag.Userid = new SelectList(_context.Users, "UserId", "Fname");
            var modelContext = _context.Categories.Include(r => r.Recipes.Where(r =>r.Flag==1));

            return View(modelContext);
        }
    }
}
