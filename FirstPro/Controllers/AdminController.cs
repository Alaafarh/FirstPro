using AspNetCoreHero.ToastNotification.Abstractions;
using FirstPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace FirstPro.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _toastNotification;


        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService toastNotification)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            ViewBag.recipynum = _context.Recipes.Count();
            ViewBag.order = _context.Orderrecipes.Count();
            ViewBag.user = _context.Users.Where(u => u.Roleid == 3).Count();
            ViewBag.chef = _context.Users.Where(u =>u.Roleid==2).Count();
            ViewBag.date = DateTime.Now;
            ViewBag.users=_context.Users.Where(u=>u.Roleid==2).ToList();
            ViewBag.UserOrder = _context.Orderrecipes.Include(r =>r.Recipe).Include(r =>r.User).ToList();
            //chart
            var recipeSales = _context.Orderrecipes
                .GroupBy(or => or.Recipe.Recipename)
                .Select(group => new
                {
                    RecipeName = group.Key,
                    SalesCount = group.Count()
                })
                .OrderByDescending(rs => rs.SalesCount) // ترتيب تنازلي حسب عدد المبيعات
                .ToList();

            ViewBag.RecipeSales = recipeSales;
            


            return View();
        }

        public IActionResult AcceptRecipe()
        {
           var  recipe = _context.Recipes.Where(r => r.Flag == 0).ToList();
            return View(recipe);
        }

        [HttpPost]
        public IActionResult AcceptRecipe(decimal id)
        {
            var recipe = _context.Recipes.Find(id);
            recipe.Flag = 1;
            _context.Update(recipe);
            _context.SaveChangesAsync();
            //ViewBag.AcceptRecipe = _context.Recipes.Where(r => r.Flag == 0).ToList();

            return RedirectToAction("Index", "Recipes");
        }
        public IActionResult AcceptTestemonial()
        {
            var modelContext = _context.Testimonials.Where(x => x.Flag == 0).Include(r => r.User);

            return View(modelContext);
        }
        [HttpPost]
        public IActionResult AcceptTestemonial(decimal id)
        {
            var testimonials = _context.Testimonials.Find(id);
            testimonials.Flag = 1;
            _context.Update(testimonials);
            _context.SaveChangesAsync();
            //ViewBag.AcceptRecipe = _context.Recipes.Where(r => r.Flag == 0).ToList();

            return RedirectToAction();
        }

        public IActionResult ChefProfile()
        {

            var id = HttpContext.Session.GetInt32("AdminId");
            
            
                var Login = _context.Userlogins.Include(obj => obj.User).Where(obj => obj.Userid == id).FirstOrDefault();
                var user = _context.Users.Where(obj => obj.UserId == Login.Userid).FirstOrDefault();
                var tuple = new Tuple<Userlogin, User>(Login, user);
            ViewBag.numuser = _context.Users.Count();
            ViewBag.order = _context.Orderrecipes.Count();
            return View(tuple);
           



        }
        public IActionResult EditChefProfile(decimal id)
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
        public IActionResult EditChefProfile(decimal id, string Fname, string Lname, IFormFile imagefile, string Email, string password, string username)
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
                if (_context.SaveChanges() == 1)
                {
                    _toastNotification.Success("Your Profile Updated");
                }
            }

            return RedirectToAction("CustmerProfile", "Custmer");
        }

        public IActionResult Search()
        {

            var result = _context.Recipes.Include(x => x.Category).Include(x => x.User).ToList();

            return View(result);
        }

        [HttpPost]
        public IActionResult Search(DateTime? startDate, DateTime? endDate)
        {
            var result = _context.Recipes.Include(x => x.Category).Include(x => x.User).ToList();

            if (startDate == null && endDate == null)
            {
                return View(result);
            }
            else if (startDate != null && endDate == null)
            {

                result = result.Where(x => x.Creatdate.Value.Date >= startDate).ToList();

                return View(result);
            }
            else if (startDate == null && endDate != null)
            {

                result = result.Where(x => x.Creatdate.Value.Date <= endDate).ToList();

                return View(result);
            }
            else
            {

                result = result.Where(x => x.Creatdate.Value.Date >= startDate && x.Creatdate.Value.Date <= endDate).ToList();
                return View(result);
            }
        }
        public IActionResult Report()
        {
            var orders = _context.Orderrecipes.Include(o => o.User).Include(o => o.Recipe).ToList();
            return View(orders);
        }
        [HttpPost]

        public IActionResult Report(string MonthDate, string YearDate)
        {
            if (!string.IsNullOrEmpty(MonthDate))
            {
                // Parse the MonthDate to get the month and year
                DateTime monthDateTime;
                if (!DateTime.TryParse(MonthDate, out monthDateTime))
                {
                    _toastNotification.Warning("Invalid Month Date format.");
                    return RedirectToAction("Report", "Admin");
                }

                int month = monthDateTime.Month;
                int year = monthDateTime.Year;

                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1);

                var obj = _context.Orderrecipes
                    .Include(x => x.Recipe)
                    .Include(x => x.User)
                    .Where(x => x.Shopdate >= startDate && x.Shopdate < endDate)
                    .ToList();

                if (obj.Count > 0)
                {
                    int TOTlprice = (int)_context.Orderrecipes
                        .Where(x => x.Shopdate >= startDate && x.Shopdate < endDate)
                        .Sum(x => x.Totalprice);

                    decimal? TotalPrice = obj.Sum(x => (decimal?)x.Totalprice);
                    decimal? myprofit = obj.Sum(x => x.Totalprice) * 0.03m;
                    decimal? expenses = 10;

                    decimal? amountOfProfitOrLoss = myprofit - expenses;
                    if (amountOfProfitOrLoss > 0)
                    {
                        _toastNotification.Information("This Month is a Profit Month");
                        ViewBag.Message = "This Month is a Profit Month";
                    }
                    else
                    {
                        _toastNotification.Information("This Month is a Loss Month, sorry to tell you that");
                        ViewBag.Message = "This Month is a Loss Month, sorry to tell you that";
                    }
                    ViewBag.totalprice = TotalPrice;

                    ViewBag.myprofit = myprofit;
                    ViewBag.expenses = expenses;

                    ViewBag.Ammount = amountOfProfitOrLoss;
                }
                else
                {
                    _toastNotification.Warning("This Month Does Not Contain Any Orders");
                }

                return View(obj);
            }

            if (!string.IsNullOrEmpty(YearDate))
            {
                int year;
                if (!int.TryParse(YearDate, out year))
                {
                    _toastNotification.Warning("Invalid Year format.");
                    return RedirectToAction("Report", "Admin");
                }

                var startDate = new DateTime(year, 1, 1);
                var endDate = new DateTime(year + 1, 1, 1);

                var obj = _context.Orderrecipes
                    .Include(x => x.Recipe)
                    .Include(x => x.User)
                    .Where(x => x.Shopdate >= startDate && x.Shopdate < endDate)
                    .ToList();

                if (obj.Count > 0)
                {
                    int TOTlprice = (int)_context.Orderrecipes
                        .Where(x => x.Shopdate >= startDate && x.Shopdate < endDate)
                        .Sum(x => x.Totalprice);

                    decimal? TotalPrice = obj.Sum(x => (decimal?)x.Totalprice);
                    decimal? myprofit = obj.Sum(x => x.Totalprice) * 0.03m;
                    decimal? expenses = 10;

                    decimal? amountOfProfitOrLoss = myprofit - expenses;
                    if (amountOfProfitOrLoss > 0)
                    {
                        _toastNotification.Information("This Year is a Profit Year");
                        ViewBag.Message = "This Year is a Profit Year";
                    }
                    else
                    {
                        _toastNotification.Information("This Year is a Loss Year, sorry to tell you that");
                        ViewBag.Message = "This Year is a Loss Year, sorry to tell you that";
                    }
                    ViewBag.totalprice = TotalPrice;

                    ViewBag.myprofit = myprofit;
                    ViewBag.expenses = expenses;

                    ViewBag.Ammount = amountOfProfitOrLoss;
                }
                else
                {
                    _toastNotification.Warning("This Year Does Not Contain Any Orders");
                }

                return View(obj);
            }

            return RedirectToAction("Report", "Admin");
        }

        public IActionResult contact()
        {
            var contact = _context.Contacts.ToList();
            return View(contact);
        }
       

        public IActionResult chart()
        {
            ViewBag.order = _context.Orderrecipes.ToList();

            var order = _context.Orderrecipes.ToList();
            int TOTlprice = (int)_context.Orderrecipes.Sum(x => x.Totalprice);
            decimal? myprofit = order.Sum(x => x.Totalprice) * 0.03m;
            decimal? expenses = 10;
            decimal? amountOfProfitOrLoss = myprofit - expenses;

            ViewBag.totalprice = TOTlprice;
            ViewBag.myprofit = myprofit;
            ViewBag.expenses = expenses;
            ViewBag.Ammount = amountOfProfitOrLoss;

            // تحضير البيانات للرسم البياني، مع ترتيب الوصفات حسب الأكثر مبيعاً
            var recipeSales = _context.Orderrecipes
                .GroupBy(or => or.Recipe.Recipename)
                .Select(group => new
                {
                    RecipeName = group.Key,
                    SalesCount = group.Count()
                })
                .OrderByDescending(rs => rs.SalesCount) // ترتيب تنازلي حسب عدد المبيعات
                .ToList();

            ViewBag.RecipeSales = recipeSales;

            return View();
        }

        public IActionResult orders()
        {
           var UserOrder = _context.Orderrecipes.Include(r => r.Recipe).Include(r => r.User).ToList();
            return View(UserOrder);
        }

        public IActionResult AboutManage()
        {
            return View(_context.Aboutus.ToList());
        }
        [HttpPost]
        public IActionResult AboutManage(string Textinsideimage, IFormFile Image ,string About)
        {
            Aboutu aboutu = new Aboutu();
            if (Textinsideimage != null)
            {
                string fileName = "";
                if (Image != null)
                {
                    //pring path
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    //get file name 
                    //generate uniq value Guid.NewGuid().ToString()
                    fileName = Guid.NewGuid().ToString() + "_" + Image.FileName;

                    //create path 
                    string extension = Path.GetExtension(Image.FileName);
                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);
                    //to but inside file images
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        Image.CopyTo(fileStream);
                    }

                    aboutu.Imageform = fileName;
                }
              
               
                aboutu.Textinsideimage = Textinsideimage;
                aboutu.About = About;
                _context.Add(aboutu);
                if (_context.SaveChanges() == 1)
                {
                    _toastNotification.Success("add succsessfully");
                }
            }
            else
                _toastNotification.Warning("You Must fill all required data");
            return RedirectToAction("AboutManage", "Admin");
        }

        public IActionResult homeManage()
        {
            return View(_context.Homes.ToList());
        }
        [HttpPost]
        public IActionResult homeManage(string Textaboutus, IFormFile Imageslider, string Textcontact ,string Textonimage,string Textonvedio)
        {
            Home h = new Home();
            if (Textaboutus != null)
            {
                string fileName = "";
                if (Imageslider != null)
                {
                    //pring path
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    //get file name 
                    //generate uniq value Guid.NewGuid().ToString()
                    fileName = Guid.NewGuid().ToString() + "_" + Imageslider.FileName;

                    //create path 
                    string extension = Path.GetExtension(Imageslider.FileName);
                    string path = Path.Combine(wwwRootPath + "/Images/" + fileName);
                    //to but inside file images
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        Imageslider.CopyTo(fileStream);
                    }

                    h.Imageslider = fileName;
                }


                h.Textaboutus = Textaboutus;
                h.Textcontact = Textcontact;
                h.Textonimage = Textonimage;
                h.Textonvedio = Textonvedio;
                _context.Add(h);
                if (_context.SaveChanges() == 1)
                {
                    _toastNotification.Success("add succsessfully");
                }
            }
            else
                _toastNotification.Warning("You Must fill all required data");
            return RedirectToAction("homeManage", "Admin");
        }

         
    }


}

