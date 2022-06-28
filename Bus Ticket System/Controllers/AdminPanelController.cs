using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[Controller]")]
    public class AdminPanelController : Controller
    {
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Bus")]
        public IActionResult Bus()
        {
            return View();
        }
        [Route("Users")]
        public IActionResult Users()
        {
            return View();
        }
        [Route("Tickets")]
        public IActionResult Tickets()
        {
            return View();
        }
    }
}
