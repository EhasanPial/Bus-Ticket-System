using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_System.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [Route("~/")] // "localhost:5000/"   // root of website
        [Route("")] // "localhost:5000/home" [Route("/Home")]  
        [Route("[action]")]  // localhost:5000/home/index
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
