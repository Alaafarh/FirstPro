using AspNetCoreHero.ToastNotification.Abstractions;
using FirstPro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Stripe.Checkout;
using MailKit.Net.Smtp;
using MailKit;
using IronPdf;

namespace FirstPro.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        //Notification
        private readonly INotyfService _toastNotification;

        public CheckOutController(ModelContext context, IWebHostEnvironment webHostEnvironment, INotyfService toastNotification)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _toastNotification = toastNotification;
        }
        public IActionResult OrderRecipe(decimal id)
        {
            var userid = HttpContext.Session.GetInt32("IDcustmer");
            if (userid == null)
            {
                _toastNotification.Warning("Plese login first as custmer");
                return RedirectToAction("Login", "Account");

            }
            else
            {
                var reccipy = _context.Recipes.Where(r => r.Recipeid == id).FirstOrDefault();
                var domain = "https://localhost:7157/";
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                    CancelUrl = domain + $"Account/Login",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment"
                };

                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = ((long)reccipy.Price),
                        //UnitAmount = 200,
                        Currency = "AED",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {

                            Name = reccipy.Recipename.ToString(),
                            Description = reccipy.Discription

                        }
                    },
                    Quantity = 1
                };

                options.LineItems.Add(sessionListItem);
                var service = new SessionService();
                Session session = service.Create(options);
                Response.Headers.Add("Location", session.Url);
                Orderrecipe order = new Orderrecipe();
                order.Shopdate = DateTime.Now;
                order.Userid = userid;
                order.Recipeid = reccipy.Recipeid;
                order.Totalprice = reccipy.Price;
                _context.Add(order);
                _context.SaveChanges();
                TempData["OrderId"] = order.Orderrecipe1.ToString();
                TempData["SessionId"] = session.Id;


                return new StatusCodeResult(303);


            }

        }
        public IActionResult OrderConfirmation()
        {
            var orderId = Convert.ToDecimal(TempData["OrderId"].ToString());
            var invoice = _context.Orderrecipes.Where(x => x.Orderrecipe1 == orderId).FirstOrDefault();
            var user = _context.Users.Where(x => x.UserId == invoice.Userid).FirstOrDefault();
            var recipe = _context.Recipes.Where(x => x.Recipeid == invoice.Recipeid).FirstOrDefault();
            var tuple = new Tuple<Orderrecipe, User, Recipe>(invoice, user, recipe);
            ViewBag.numorder = _context.Orderrecipes.Count();
            //create new message 
            MimeMessage newMessage = new MimeMessage();
            //sender 
            newMessage.From.Add(new MailboxAddress("CookingRecipe", "magazinestoremaya@gmail.com"));

            //var InformationUser = _context.Userlogins.Include(obj => obj.Usersfs).Where(obj => obj.Id == HttpContext.Session.GetInt32("UserId")).FirstOrDefault();

            //receiver
            newMessage.To.Add(MailboxAddress.Parse(user.Email));

            //Message Subject
            newMessage.Subject = "detilse for recipe";


            var PDF = new ChromePdfRenderer();
            var PDF2 = new ChromePdfRenderer();


            var Receipt = PDF.RenderHtmlAsPdf($"<h1 style=color:Red;>Cooking Recipe Store </h1><br>" +
                $" <h3> Date Of Receipt :{DateTime.Today.Day} / {DateTime.Today.Month} / {DateTime.Today.Year}</h3> <br>" +
                $" <h3> Recipe Name : {recipe.Recipename}  </h3>" +
                $" <h3> Total Price : {recipe.Price} </h3>" +
                $" <h3> description : {recipe.Discription}  </h3>");

            Receipt.SaveAs("recipedsend.pdf");
            string htmlInvoiceContent = @"
<h1 style='color:Red;'>Invoice</h1>

<br>
<br>
<table border='1' style='width:100%; border-collapse:collapse;'>
    <tr>
        <th>User Name</th>
        <th>User Email</th>

        <th>Recipe Name</th>
        <th> Price</th>
        <th>Total Price</th>
        <th>date</th>
    </tr>
    <tr>
        <td>" + user.Fname + user.Lname + @"</td>
        <td>" + user.Email + @"</td>

        <td>" + recipe.Recipename + @"</td>
        <td>" + recipe.Price + @"</td>
        <td>" + recipe.Price + @"</td>
        <td>" + invoice.Shopdate + @"</td>



    </tr>
</table>";
            var invoicePdf = PDF2.RenderHtmlAsPdf(htmlInvoiceContent);
            invoicePdf.SaveAs("invoic.pdf");


            var builder = new BodyBuilder();

            builder.HtmlBody = "<p>Thank you for trusting us. This is the required recipe</p>";
            builder.Attachments.Add("C:\\Users\\User\\Desktop\\asp.net\\FirstPro\\FirstPro\\recipedsend.pdf");
            builder.Attachments.Add("C:\\Users\\User\\Desktop\\asp.net\\FirstPro\\FirstPro\\invoic.pdf");

            newMessage.Body = builder.ToMessageBody();


            //string emailaddress = "recipec085@gmail.com";
            //string password = "Aa@112233";
            string emailaddress = "magazinestoremaya@gmail.com";
            string password = "jtyjzcdtwcmgjmmm";


            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailaddress, password);
                client.Send(newMessage);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }





            return View(tuple);

        }
    }
}