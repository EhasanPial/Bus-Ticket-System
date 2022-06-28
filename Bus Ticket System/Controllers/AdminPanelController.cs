using Bus_Ticket_System.Models;
using Bus_Ticket_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bus_Ticket_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[Controller]")]
    public class AdminPanelController : Controller
    {
        private readonly IBusDBRepository _busDBRepository;

        public AdminPanelController(IBusDBRepository busDBRepository)
        {
            _busDBRepository = busDBRepository;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Bus")]
        [HttpGet]
        public IActionResult Bus()
        {
            IEnumerable<Bus> buses = _busDBRepository.GetAllBus();

            BusAndListBusViewModel busAndListBusViewModels = new BusAndListBusViewModel
            {
                Buses = buses
            };
            return View(busAndListBusViewModels);

        }

        /// ----------- BUS CREATE HTTP POST ----------- ///
        [Route("Bus")]
        [HttpPost]

        public IActionResult Bus(BusAndListBusViewModel busAndListBus)
        {
            if (ModelState.IsValid)
            {
                if((busAndListBus.bus.From != busAndListBus.bus.To))
                {
                    _busDBRepository.Add(busAndListBus.bus);
                    IEnumerable<Bus> buses = _busDBRepository.GetAllBus();

                    BusAndListBusViewModel busAndListBusViewModels = new BusAndListBusViewModel
                    {
                        Buses = buses,
                        bus = new Bus()

                    };
                    ModelState.Clear();
                    return View(busAndListBusViewModels);
                }
                return View("Start and Destination are same");
            }
            else
            {
                return View();
            }
            
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
