using _42Cares.Models;
using System.Net;//added for NetworkCredential
using System.Net.Mail;//added for email
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;//for attributes (validation/formatting)

namespace _42Cares.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View("About");
        }

        [HttpGet]//GETS the form to fill out
        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Clinical()
        {
            return View("Clinical");
        }
        public ActionResult Resources()
        {
            return View("Resources");
        }
        public ActionResult EAP()
        {
            return View("EAP");
        }
        [HttpPost]//HANDLES the submitted form results (or "posts them")
        public ActionResult Contact(MessageViewModel msg)
        {
            //TODO: add logic to send email message on user's behalf AND show them confirmation page
            if (!ModelState.IsValid)
            {
                //bad data submitted - kick back to try again
                return View(msg);
            }
            try { 
            //possibly good data submitted - process results by building & sending email
            string messageBody = $"You have received an email from {msg.Name}. Please respond to {msg.EmailAddress}. Their message was: <blockquote>{msg.Message}</blockquote>";

            MailMessage m = new MailMessage("noreplies@DavidBellDevelops.com", "ajones@iaff42.org", "Message From 42Cares Website", messageBody);

                //m.IsBodyHtml = true;
                //object emailAddress = msg.EmailAddress;
                //m.ReplyToList.Add(emailAddress);//if receiver hits reply, goes back to user, not noreply address

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "ajones@iaff42.org",  // replace with valid value
                        Password = "Iaff42-2019!"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(m);
                    //return RedirectToAction("Index", "Home");
                    //if they land here - email sent OK so show them confirmation (new page)
                    return View("EmailConfirmation", msg);



                }

            }
            catch (System.Exception ex)
            {
                //if they land here, sending email failed
                ViewBag.CustomerMessage =
                    $"We're sorry the request failed. Try again later.<h2>Error</h2>{ex.StackTrace}";
                return View(msg);
            }

        }

    }
    public class MessageViewModel//represents a message user sends thru a form - we will email it 
    {
        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "A functional email address must be provided")]
        [Display(Name = "Email Address")]//used for dynamically genned labels
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "A functional subject must be provided")]
        [Display(Name = "Subject")]//used for dynamically genned labels
        [DataType(DataType.Text)]
        public string Subject { get; set; }

        [Required]
        [UIHint("MultilineText")]//changes the input control to a multi-line text box AKA <textarea>
        public string Message { get; set; }



    }
}

