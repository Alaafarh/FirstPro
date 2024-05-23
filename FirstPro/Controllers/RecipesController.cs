using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstPro.Models;
using Microsoft.AspNetCore.Http;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace FirstPro.Controllers
{
    public class RecipesController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _toastNotification;


        public RecipesController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService toastNotification)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: Recipes/Details/5
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

        // GET: Recipes/Create
        public IActionResult Create()
        {
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname");
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Recipeid,Recipename,Price,Flag,ImageFile,Userid,Categoryid,Creatdate,Discription")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {

                if (recipe.ImageFile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + recipe.ImageFile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        recipe.ImageFile.CopyToAsync(fileStream);
                    }
                    recipe.Imagepath = imageName;
                }
                recipe.Creatdate = DateTime.Now;
                recipe.Flag = 0;
                recipe.Userid = HttpContext.Session.GetInt32("Chifid");
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction("RecipespageChef", "Recipes");
            }
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname", recipe.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "Fname", recipe.Userid);
            ViewBag.Username = _context.Userlogins.First().Username;
            return View(recipe);
        }

        // GET: Recipes/Edit/5
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
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "Fname", recipe.Userid);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
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
            _context.Update(recipe);
            await _context.SaveChangesAsync();
          
            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        if (recipe.ImageFile != null)
            //        {
            //            string wwwrootPath = _webHostEnvironment.WebRootPath;
            //            string imageName = Guid.NewGuid().ToString() + "_" + recipe.ImageFile.FileName;
            //            string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
            //            using (var fileStream = new FileStream(fullPath, FileMode.Open))
            //            {
            //                recipe.ImageFile.CopyToAsync(fileStream);
            //            }
            //            recipe.Imagepath = imageName;
            //        }
            //        _context.Update(recipe);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!RecipeExists(recipe.Recipeid))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            ViewData["Categoryid"] = new SelectList(_context.Categories, "Categoryid", "Categoryname", recipe.Categoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "UserId", "Fname", recipe.Userid);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
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

        // POST: Recipes/Delete/5
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
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(decimal id)
        {
            return (_context.Recipes?.Any(e => e.Recipeid == id)).GetValueOrDefault();
        }
        public IActionResult Recipespage()//all recipes
        {
            var recipe = _context.Recipes.Where(r => r.Flag == 1).ToList();
            //var modelContext = _context.Recipes.Include(r => r.Category).Include(r => r.User);
            //ViewBag.Categoryid = new SelectList(_context.Categories, "Categoryid", "Categoryname");
            //ViewBag.Userid = new SelectList(_context.Users, "UserId", "Fname");
            //var modelContext = _context.Recipes.Where(r => r.Flag == 1).Include(r => r.Category);
            var modelContext = _context.Recipes.Where(r => r.Flag == 1).Include(r => r.User);

            ViewBag.chefname = _context.Users.Where(x => x.Roleid == 2).ToList();

            return View(modelContext);
        }
        //action get recipe for each chef
        public IActionResult GetRecipeById(int Id)//when click chef name go page recipes for chef clicked
        {
            var ChefRecipe = _context.Recipes.Where(x => x.Userid == Id && x.Flag==1).ToList();
            var name = _context.Users.Where(x => x.UserId == Id).FirstOrDefault();
            ViewBag.Fname = name.Fname;
            ViewBag.Lname = name.Lname;


            return View(ChefRecipe);
        }

        public IActionResult RecipespageChef()//for chef 
        {
            var id = HttpContext.Session.GetInt32("Chifid");

            var recipe = _context.Recipes.Where(r => r.Userid == id && r.Flag == 1).ToList();
            var pindingrECIPE = _context.Recipes.Where(r => r.Flag == 0 && r.Userid == id).ToList();

            ViewBag.pinding = pindingrECIPE;
            return View(recipe);
        }

        public IActionResult Search()
        {
            var recipe = _context.Recipes.Where(r => r.Flag == 1).Include(c =>c.Category).ToList();
            //var modelContext = _context.Recipes.Where(r => r.Flag == 1).Include(r => r.Category);


            return View(recipe);
        }
        [HttpPost]
        public IActionResult Search(string searchBy, string search)
        {
            if (searchBy != null)
            {
                var category = _context.Categories.Where(obj => obj.Categoryname == searchBy).FirstOrDefault();
                if (category != null)
                {
                    var all = _context.Recipes.Include(obj => obj.Category).Where(obj => obj.Categoryid == category.Categoryid && obj.Flag == 1).ToList();
                    if (all.Count != 0)
                    {
                        return View(all);
                    }
                    else
                    {
                        _toastNotification.Warning("This Category is Empty");
                        return View();
                    }
                }
               

            }

            if (search != null)
            {
                var recipename = _context.Recipes.Include(obj => obj.Category).Where(obj => obj.Recipename.StartsWith(search) && obj.Flag == 1).ToList();
                if (recipename.Count != 0)
                {
                    return View(recipename);
                }
                else
                {
                    _toastNotification.Warning("This recipe not found");
                    return View();
                }

            }

            return View();








        }


    }
}

