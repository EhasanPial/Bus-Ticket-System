using Bus_Ticket_System.Models;
using Bus_Ticket_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Ticket_System.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {

        private readonly IBusDBRepository _busDBRepository;
        private readonly AppDbContext _context;

        public HomeController(IBusDBRepository busDBRepository, AppDbContext context)
        {
            _busDBRepository = busDBRepository;
            _context = context;

        }


        // [Route("~/")] // "localhost:5000/"   // root of website
        // [Route("")] // "localhost:5000/home" [Route("/Home")]  
        [Route("[action]")]  // localhost:5000/home/index
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Route("~/")] // "localhost:5000/"   // root of website
        [Route("")] // "localhost:5000/home" [Route("/Home")]  
        [Route("[action]")]  // localhost:5000/home/index
        [AllowAnonymous]
        public IActionResult Index2()
        {
            IEnumerable<Bus> allBus = _busDBRepository.GetAllBus();


            /* Enum dhaka = Models.Route.Dhaka;
             for (int i = 0; i < allBus.Count(); i++)
             {
                 if(!allBus.ElementAt(i).From.Equals(dhaka))
                 {
                     allBus.ToList().RemoveAt(i);
                 }
             }

             Console.WriteLine(allBus.Count());*/

            return View(allBus);
        }





        [Route("SearchBus")]
        public async Task<IActionResult> SearchBus(Models.Route? SearchFrom, Models.Route? SearchTo, Models.BusType? SeachBusType, DateTime datePassing)
        {

            ViewData["FromFilter"] = SearchFrom;
            ViewData["DesFilter"] = SearchTo;
            ViewData["SearchBusType"] = SeachBusType;
            var allBus = from s in _context.Buses
                         select s
                         ;

            if (SearchFrom != null)
                allBus = allBus.Where(bus => bus.From.Equals(SearchFrom) && bus.To.Equals(SearchTo) && bus.Type.Equals(SeachBusType));


            IEnumerable<Bus> buses = await allBus.AsNoTracking().ToListAsync();

            BusBookingViewModel viewModel = new BusBookingViewModel
            {
                Buses = buses,
                datePassing = datePassing
            };
            ModelState.Remove("SearchFrom");
            ModelState.Remove("SearchTo");


            return View(viewModel);


        }

        [Route("/SearchBus/{id}")]
        public IActionResult Purchase(int id, BusBookingViewModel viewModel)
        {
            viewModel.busId = id;

            return View(viewModel);
        }
    }
}
