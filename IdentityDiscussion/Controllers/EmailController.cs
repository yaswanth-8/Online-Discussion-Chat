using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace IdentityDiscussion.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(string address,string subject,string body)
        {
            Console.WriteLine(address);
            Console.WriteLine(subject);
            Console.WriteLine(body);
            return View();
        }
    }
}
