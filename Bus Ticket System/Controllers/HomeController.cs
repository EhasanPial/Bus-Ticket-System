using Bus_Ticket_System.Models;
using Bus_Ticket_System.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Ticket_System.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {

        private readonly IBusDBRepository _busDBRepository;
        private readonly AppDbContext _context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;



        public HomeController(IBusDBRepository busDBRepository, AppDbContext context, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,
                                    SignInManager<ApplicationUser> signInManager)
        {
            _busDBRepository = busDBRepository;
            _context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;

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

        [Route("/Purchase/{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PurchaseAsync(int id, DateTime date, BusBookingViewModel viewModel)
        {
            var allSeats = from s in _context.BusSeatsNew
                           select s
                         ;
            IEnumerable<BusSeatNew> busSeats = await allSeats.AsNoTracking().ToListAsync(); // getting all bus ;
            int currentAva = 1;
            int busseatId = 1;
            foreach (BusSeatNew busSeat in busSeats)
            {
                if (id == busSeat.busId)
                {

                    currentAva = busSeat.seatNo;
                    busseatId = busSeat.Id;
                    break;
                }

            }

            Bus bus = _busDBRepository.GetBusById(id);

            PurchaseViewModel purchaseViewModel = new PurchaseViewModel
            {
                busId = id,
                currentAva = currentAva,
                name = User.Identity.GetUserName(),
                bus = bus,
                busseatid = busseatId,
                userId = User.Identity.GetUserId(),
                journeyTime = date




            };

            return View(purchaseViewModel);
        }


        [Route("/Purchase/{id}")]
        [HttpPost]
        [Authorize]
        public IActionResult PurchaseAsync(int id, PurchaseViewModel purchaseViewModel)
        {
            Bus bus = _busDBRepository.GetBusById(purchaseViewModel.busId);
            purchaseViewModel.bus = bus;
            Ticket ticket = new Ticket
            {

                busId = id,
             
                currentAva = purchaseViewModel.currentAva,
                name = purchaseViewModel.name,
                From = purchaseViewModel.bus.From,
                To = purchaseViewModel.bus.To,
                busTime = purchaseViewModel.bus.Time,
                cost = purchaseViewModel.bus.Cost,
                purchaseTime = DateTime.Now,
                userId = purchaseViewModel.userId,
                journeyTime = purchaseViewModel.journeyTime

            };
            _busDBRepository.AddTicket(ticket);
            BusSeatNew busSeat = new BusSeatNew
            {
                Id = purchaseViewModel.busseatid,
                busId = id,
                seatNo = purchaseViewModel.currentAva + 1
            };
            _busDBRepository.UpdateBusSeat(busSeat);

            return RedirectToAction("purchaseHistory", "home");
        }

        [Route("purchaseHistory")]
        [Authorize]
        public async Task<IActionResult> purchaseHistoryAsync()
        {

            var allSeats = from s in _context.Tickets
                           select s
                       ;
            IEnumerable<Ticket> tickets = await allSeats.AsNoTracking().ToListAsync(); // getting all bus ; 
            List<Ticket> userTicket = new List<Ticket>();

            foreach (Ticket t in tickets)
            {
                if (t.userId == User.Identity.GetUserId())
                {
                    userTicket.Add(t);
                }
            }
            return View(userTicket);
        }
        [Route("returnTicket")]
        [Authorize]
        public IActionResult returnTicket(int ticketId)
        {
            Ticket ticket = _busDBRepository.GetTicket(ticketId);
            DateTime now = DateTime.Now;
            DateTime journeyDate = ticket.journeyTime;

            if (now < journeyDate)
            {
                _busDBRepository.DeleteTicket(ticketId);
               

            }

            return RedirectToAction("purchaseHistory", "Home");
        }

    }
        

}
