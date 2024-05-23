using AspNetCoreHero.ToastNotification.Abstractions;
using FirstPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FirstPro.Controllers
{
    public class CustmerController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //Notification
        private readonly INotyfService _toastNotification;
        public CustmerController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService toastNotification)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }
        public IActionResult CustmerProfile()
        {
            var user2 = _context.Userlogins.Where(obj => obj.Userid == HttpContext.Session.GetInt32("IDcustmer")).FirstOrDefault();
            if (user2 != null)
                ViewBag.IDcustmer = user2.Roleid;
            var user3 = _context.Userlogins.Where(obj => obj.Userid == HttpContext.Session.GetInt32("Chifid")).FirstOrDefault();
            if (user3 != null)
                ViewBag.Chifid = user3.Roleid;

            var id = HttpContext.Session.GetInt32("IDcustmer");
            var id2 = HttpContext.Session.GetInt32("Chifid");
            ViewBag.ornum = _context.Orderrecipes.Where(x => x.Userid == id).Count();
            ViewBag.renum = _context.Recipes.Where(x => x.Userid == id2).Count();


            if (id != null)
            {
                var Login = _context.Userlogins.Include(obj => obj.User).Where(obj => obj.Userid == id).FirstOrDefault();
                var user = _context.Users.Where(obj => obj.UserId == Login.Userid).FirstOrDefault();
                var order = _context.Orderrecipes.Where(x => x.Userid == id).Include(a => a.Recipe).ToList();
                var recipe = _context.Recipes.Where(r => r.Userid == id).ToList();
                var tuple = new Tuple<Userlogin, User, IEnumerable<Orderrecipe>, IEnumerable<Recipe>>(Login, user, order, recipe);
                return View(tuple);
            }
            else
            {

                var Login = _context.Userlogins.Include(obj => obj.User).Where(obj => obj.Userid == id2).FirstOrDefault();
                var user = _context.Users.Where(obj => obj.UserId == Login.Userid).FirstOrDefault();
                var order = _context.Orderrecipes.Where(x => x.Userid == id).Include(a => a.Recipe).ToList();
                var recipe = _context.Recipes.Where(r => r.Userid == id2).ToList();
                var tuple = new Tuple<Userlogin, User, IEnumerable<Orderrecipe>, IEnumerable<Recipe>>(Login, user, order, recipe);
                return View(tuple);

            }


        }
        public IActionResult EditProfile(decimal id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Roleid"] = new SelectList(_context.Roles, "Roleid", "Rolename", user.Roleid);
            var userlogin = _context.Userlogins.Where(l => l.Userid == id).FirstOrDefault();
            var tuple = new Tuple<User, Userlogin>(user, userlogin);
            return View(tuple);

        }
        [HttpPost]
        public IActionResult EditProfile(decimal id, string Fname, string Lname, IFormFile imagefile, string Email, string password, string username)
        {

            var user = _context.Users.Find(id);
            var userlogin = _context.Userlogins.Where(l => l.Userid == id).FirstOrDefault();
            if (user != null && userlogin != null)
            {
                if (imagefile != null)
                {
                    string wwwrootPath = _webHostEnvironment.WebRootPath;
                    string imageName = Guid.NewGuid().ToString() + "_" + imagefile.FileName;
                    string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        imagefile.CopyTo(fileStream);
                    }
                    user.Imagepath = imageName;
                }
                if (user.Fname != Fname)
                {
                    user.Fname = Fname;
                }
                if (user.Lname != Lname)
                {
                    user.Lname = Lname;
                }
                if (user.Email != Email)
                {
                    user.Email = Email;
                }
                if (userlogin.Username != username)
                {
                    userlogin.Username = username;
                }
                if (userlogin.Password != password)
                {
                    userlogin.Password = password;
                }
                _context.Update(user);
                _context.SaveChanges();
                _context.Update(userlogin);
                _context.SaveChanges();
                if (_context.SaveChanges() == 0)
                {
                    _toastNotification.Success("Your Profile Updated");
                }
            }

            return RedirectToAction("CustmerProfile", "Custmer");
        }
        public IActionResult wishlist(decimal id)
        {
            var userid = HttpContext.Session.GetInt32("IDcustmer");
            var chefid = HttpContext.Session.GetInt32("Chifid");
            if (userid == null)
            {
                _toastNotification.Warning("plese login First");

                return RedirectToAction("Login", "Account");
            }
            else
            {
                var whishlist = _context.Wishlists.Where(w => w.Userid == userid).Include(w => w.User).Include(w => w.Recipe).ToList();
                ViewBag.user = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
                return View(whishlist);

            }
        }

        public IActionResult Addwishlist(decimal id)
        {
            var userid = HttpContext.Session.GetInt32("IDcustmer");

            var recipe = _context.Wishlists.Where(r => r.Recipeid == id).FirstOrDefault();
            if (recipe != null)
            {
                _toastNotification.Warning("This recipe was added before");

            }
            else
            {
                Wishlist obj = new Wishlist();
                obj.Recipeid = id;
                obj.Userid = userid;
                obj.Recipe = _context.Recipes.Where(obj => obj.Recipeid == id).FirstOrDefault();
                obj.User = _context.Users.Where(obj => obj.UserId == userid).FirstOrDefault();

                if (obj != null)
                {
                    _context.Wishlists.Add(obj);
                    _context.SaveChanges();
                    _toastNotification.Success("Added successfully");
                }
            }

            return RedirectToAction("wishlist", "Custmer");
        }
        public IActionResult Deletewishlist(decimal id)
        {
            var obj = _context.Wishlists.Find(id);
            _context.Wishlists.Remove(obj);
            _context.SaveChanges();
            return RedirectToAction("wishlist", "Custmer");
        }
    }
}
