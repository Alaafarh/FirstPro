using AspNetCoreHero.ToastNotification.Abstractions;
using FirstPro.Models;
using Microsoft.AspNetCore.Mvc;
//using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FirstPro.Controllers
{
    public class AccountController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //Notification
        private readonly INotyfService _toastNotification;

        public AccountController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService toastNotification)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }
        public IActionResult Register()
        {
            var modelContext = _context.Users.Include(R => R.Role);

            //ViewData["RoleId"] = new SelectList(_context.Roles,"Roleid", "Rolename");
            ViewData["RoleId"] = new SelectList(
                _context.Roles.Except(_context.Roles.Where(obj => obj.Roleid == 1)),
                "Roleid", "Rolename");
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user, string userName, string Password, string ReEnterPass)
        {
            var userChick = _context.Userlogins.Where(x => x.Username == userName).SingleOrDefault();
            if (userChick == null)
            {
                if (user.Fname == null || user.Lname == null || user.Email == null || userName == null || Password == null || ReEnterPass == null)
                {
                    _toastNotification.Warning("You Must Fill All data only the picture is optional for custmer and Required for chif");
                    return View();
                }
                if (user.Fname != null || user.Lname != null || user.Email != null || userName != null || Password != null || ReEnterPass != null)
                {
                    if (Password.Equals(ReEnterPass))
                    {
                        if (user.imagefile != null)
                        {
                            string wwwrootPath = _webHostEnvironment.WebRootPath;
                            string imageName = Guid.NewGuid().ToString() + "_" + user.imagefile.FileName;
                            string fullPath = Path.Combine(wwwrootPath + "/Images/", imageName);
                            using (var fileStream = new FileStream(fullPath, FileMode.Create))
                            {
                                user.imagefile.CopyToAsync(fileStream);
                            }
                            user.Imagepath = imageName;
                        }
                        _context.Add(user);
                        _context.SaveChanges();
                        Userlogin userLogin = new Userlogin();
                        userLogin.Username = userName;
                        userLogin.Password = Password;
                        userLogin.UserloginId =user.UserId ;
                        userLogin.Userid = user.UserId;

                        userLogin.Roleid = user.Roleid;
                        _context.Add(userLogin);
                        _context.SaveChanges();
                       return RedirectToAction("Login", "Account");

                    }
                    else
                    {
                        _toastNotification.Warning("The password does not match");
                    }


                }
                

            }
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Roleid", "Rolename");
            ViewData["RoleId"] = new SelectList(
               _context.Roles.Except(_context.Roles.Where(obj => obj.Roleid == 1)),
               "Roleid", "Rolename");
            return View();


        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Userlogin userLogin)
        {

            var user = await _context.Userlogins.
            Where(x => x.Username == userLogin.Username && x.Password == userLogin.Password).SingleOrDefaultAsync();

            if (userLogin.Username == null && userLogin.Password == null)
            {
                _toastNotification.Warning("Please Fill All Required Input");
            }
            else
            {
                if (user != null)
                {
                    switch (user.Roleid)
                    {
                        case 1:
                            HttpContext.Session.SetInt32("AdminId", (int)user.Userid);
                            return RedirectToAction("Index", "Admin");
                        case 2:
                            HttpContext.Session.SetInt32("Chifid", (int)user.Userid);
                            return RedirectToAction("Index", "Home");
                        case 3:
                            HttpContext.Session.SetInt32("IDcustmer", (int)user.Userid);
                            return RedirectToAction("Index", "Home");



                    }



                }

                else
                {
                    _toastNotification.Warning("Password OR UserName Uncorrect");
                }

            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}
