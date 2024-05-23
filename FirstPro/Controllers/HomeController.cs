using AspNetCoreHero.ToastNotification.Abstractions;
using FirstPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FirstPro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly INotyfService _toastNotification;


        public HomeController(ILogger<HomeController> logger, ModelContext context, INotyfService toastNotification)
        {
            _logger = logger;
            _context = context;
            _toastNotification = toastNotification;

        }

        public IActionResult Index( int id )
        {
            // layout for custmer and chif
            var user = _context.Userlogins.Where(obj => obj.Userid == HttpContext.Session.GetInt32("AdminId")).FirstOrDefault();
            if (user != null)
                ViewBag.IDAdmin = user.Roleid;
            var user2 = _context.Userlogins.Where(obj => obj.Userid == HttpContext.Session.GetInt32("IDcustmer")).FirstOrDefault();
            if (user2 != null)
                ViewBag.IDcustmer = user2.Roleid;
            var user3 = _context.Userlogins.Where(obj => obj.Userid == HttpContext.Session.GetInt32("Chifid")).FirstOrDefault();
            if (user3 != null)
                ViewBag.Chifid = user3.Roleid;
            // disblay chif in home page
            var chif =_context.Users.Where(R => R.Roleid == 2).ToList();
           

            ViewBag.chif = chif;
            var cat= _context.Categories.ToList();
            ViewBag.categorysee = cat;
            var modelContext = _context.Categories.Include(r => r.Recipes.Where(r=>r.Flag==1));
            ViewBag.home=_context.Homes.FirstOrDefault();
            return View(modelContext);
        }
       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AboutUs(decimal ?id)
        {
            if (id != null)
            {
                var design = _context.Aboutus.Where(obj => obj.Aboutus == id).FirstOrDefault();
                Aboutu c = new Aboutu();
                c.Aboutus = design.Aboutus;
                c.About = design.About;
                c.Textinsideimage = design.Textinsideimage;
                c.Imageform = design.Imageform;

                _context.Aboutus.Remove(design);
                _context.SaveChanges();

                _context.Add(c);
                _context.SaveChanges();

            }
            var a = _context.Aboutus.FirstOrDefault();

            return View(a);

        }
        public IActionResult Home(decimal? id)
        {
            if (id != null)
            {
                var design = _context.Homes.Where(obj => obj.Home1 == id).FirstOrDefault();
                Home h = new Home();
                h.Home1 = design.Home1;
                h.Textaboutus = design.Textaboutus;
               h.Textcontact = design.Textcontact;//contact
                h.Imageslider = design.Imageslider;
                h.Textonimage = design.Textonimage;//contact
                h.Textonvedio = design.Textonvedio;

                _context.Homes.Remove(design);
                _context.SaveChanges();

                _context.Add(h);
                _context.SaveChanges();

            }
            var a = _context.Homes.FirstOrDefault();

            return View(a);

        }
        public IActionResult Contact()
        {
            ViewBag.home = _context.Homes.FirstOrDefault();

            return View();
        }
        [HttpPost]

        public IActionResult Contact( Contact contact)
        {
            _context.Add(contact);
            _context.SaveChanges();

            return RedirectToAction("Index","Home") ;
        }

        public IActionResult Testimonial()
        {
            

            return View();
        }
        [HttpPost]
        public IActionResult Testimonial( string comment)
        {
            var id = HttpContext.Session.GetInt32("IDcustmer");

            if (comment != null)
            {

                if (id == null)
                {
                    _toastNotification.Warning("Please login first to Write Your Opinion");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var Login = _context.Userlogins.Where(obj => obj.Userid == id).FirstOrDefault();
                    var user = _context.Users.Where(obj => obj.UserId == Login.Userid).FirstOrDefault();
                    //var user1 = _context.Users.Find(id);

                    Testimonial obj = new Testimonial();
                    obj.CommentUser = comment;
                    obj.Flag = 0;
                    obj.Userid = user.UserId;

                    _context.Add(obj);
                    if (_context.SaveChanges() == 1)
                    {
                        _toastNotification.Success("Thanks for your opinion");
                    }
                }
            }
            else
            {
                _toastNotification.Warning("Please Write Your Opinion");
            }
            //var acceabt = _context.Testimonials.Where(x => x.Flag == 0).ToList();
            //var user1 =_context.Users.Where(r=>r.UserId==id).FirstOrDefault();

            //var tuple = new Tuple<User, IEnumerable<Testimonial>>(user1, acceabt);

            return RedirectToAction("SeeTestimonial", "Home");
        }

        public IActionResult SeeTestimonial()
        {
            var modelContext = _context.Testimonials.Where(x => x.Flag == 1 && x.User.Roleid == 3).Include(r => r.User);

            return View(modelContext);
        }

        public IActionResult chefTestimonial()
        {


            return View();
        }
        [HttpPost]
        public IActionResult chefTestimonial(string comment)
        {
            var id = HttpContext.Session.GetInt32("Chifid");

            if (comment != null)
            {

                if (id == null)
                {
                    _toastNotification.Warning("Please login first to Write Your Opinion");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var Login = _context.Userlogins.Where(obj => obj.Userid == id).FirstOrDefault();
                    var user = _context.Users.Where(obj => obj.UserId == Login.Userid).FirstOrDefault();
                    //var user1 = _context.Users.Find(id);

                    Testimonial obj = new Testimonial();
                    obj.CommentUser = comment;
                    obj.Flag = 0;
                    obj.Userid = user.UserId;

                    _context.Add(obj);
                    if (_context.SaveChanges() == 1)
                    {
                        _toastNotification.Success("Thanks for your opinion");
                    }
                }
            }
            else
            {
                _toastNotification.Warning("Please Write Your Opinion");
            }
            //var acceabt = _context.Testimonials.Where(x => x.Flag == 0).ToList();
            //var user1 =_context.Users.Where(r=>r.UserId==id).FirstOrDefault();

            //var tuple = new Tuple<User, IEnumerable<Testimonial>>(user1, acceabt);

            return RedirectToAction("chefSeeTestimonial", "Home");
        }
        public IActionResult chefSeeTestimonial()
        {
            var modelContext = _context.Testimonials.Where(x => x.Flag == 1 && x.User.Roleid==2).Include(r => r.User);

            return View(modelContext);
        }

        
    }
}