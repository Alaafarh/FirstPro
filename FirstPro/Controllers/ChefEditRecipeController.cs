using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstPro.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace FirstPro.Controllers
{
    public class ChefEditRecipeController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _toastNotification;



        public ChefEditRecipeController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService toastNotification)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }

        // GET: ChefEditRecipe
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: ChefEditRecipe/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: ChefEditRecipe/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid");
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: ChefEditRecipe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipeid,Recipename,Price,Flag,Imagepath,Userid,Categoryid,Creatdate,Discription")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryid", recipe.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "UserId", recipe.Userid);
            return View(recipe);
        }

        // GET: ChefEditRecipe/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname", recipe.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "UserId", recipe.Userid);
            return View(recipe);
        }

        // POST: ChefEditRecipe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Recipeid,Recipename,Price,Flag,ImageFile,Userid,Categoryid,Creatdate,Discription")] Recipe recipe)
        {
            if (id != recipe.Recipeid)
            {
                return NotFound();
            }
            var recipeIMG = await _context.Recipes.Where(x => x.Recipeid == id).FirstOrDefaultAsync();
            recipe.Imagepath = recipeIMG.Imagepath;
            if (recipe.ImageFile != null)
            {
                string wwwrootPath = _webHostEnvironment.WebRootPath;
                string imageName = Guid.NewGuid().ToString() + "_" + recipe.ImageFile.FileName;
                string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                using (var fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    recipe.ImageFile.CopyToAsync(fileStream);
                }
                recipe.Imagepath = imageName;
            }
            _context.Entry(recipeIMG).State = EntityState.Detached;
            _context.Users.AsNoTracking().SingleOrDefault(u => u.UserId == recipe.Userid);
            var userid = HttpContext.Session.GetInt32("Chifid");
            recipe.Userid = userid;
            _context.Update(recipe);
            await _context.SaveChangesAsync();
            var change = await _context.SaveChangesAsync();
            if (change==0)
            {
                _toastNotification.Success("Modified successfully");


            }

            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname", recipe.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "UserId", recipe.Userid);
            return View(recipe);
        }

        // GET: ChefEditRecipe/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null || _context.Recipes == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Recipeid == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: ChefEditRecipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            if (_context.Recipes == null)
            {
                return Problem("Entity set 'ModelContext.Recipes'  is null.");
            }
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
            }
            
            await _context.SaveChangesAsync();
             var change = await _context.SaveChangesAsync();
            if (change==0)
            {
                _toastNotification.Success("Deleted successfully");


            }

            return RedirectToAction("RecipespageChef", "Recipes");
        }

        private bool RecipeExists(decimal id)
        {
          return (_context.Recipes?.Any(e => e.Recipeid == id)).GetValueOrDefault();
        }
    }
}
