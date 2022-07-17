using Bus_Ticket_System.Models;
using Bus_Ticket_System.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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




        [Route("~/")] // "localhost:5000/"   // root of website
        [Route("")] // "localhost:5000/home" [Route("/Home")]  
        [Route("[action]")]  // localhost:5000/home/index
        [AllowAnonymous]
        public IActionResult Index2()
        {
            IEnumerable<Bus> allBus = _busDBRepository.GetAllBus();
            List<Bus> busesList = new List<Bus>();

            Index2ViewModel viewModel = new Index2ViewModel();

            if (Request.Cookies["user_name"] != null)
            {
                viewModel.username = Request.Cookies["user_name"];
            }


            // Session

            if (signInManager.IsSignedIn(User))
            {
                HttpContext.Session.SetString("UserInfo", Newtonsoft.Json.JsonConvert.SerializeObject(User.Identity.GetUserName()));
            }


            Enum dhaka = Models.Route.Dhaka;

            for (int i = 0; i < 4; i++)
            {

                busesList.Add(allBus.ElementAt(i));

            }

            viewModel.allBus = busesList;

            return View(viewModel);
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
        public async Task<IActionResult> PurchaseAsync(int id, DateTime date, string voucherCode, int discount, bool? checkVoucher)
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

            string user = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("UserInfo"));

            PurchaseViewModel purchaseViewModel = new PurchaseViewModel
            {
                busId = id,
                currentAva = currentAva,
                name = user,
                bus = bus,
                busseatid = busseatId,
                userId = User.Identity.GetUserId(),
                journeyTime = date,
                voucherCode = voucherCode,


            };



            if (checkVoucher != null && checkVoucher == true)
            {
                purchaseViewModel.bus.Cost = purchaseViewModel.bus.Cost - (int)(purchaseViewModel.bus.Cost * (discount / 100.00));
                purchaseViewModel.checkVoucher = checkVoucher;
                purchaseViewModel.discount = discount;

            }

            return View(purchaseViewModel);
        }

        // --------------------------------------------- Purchase POST METHOD ------------------------------------- //
        [Route("/Purchase/{id}")]
        [HttpPost]
        [Authorize]
        public IActionResult PurchaseAsync(int id, PurchaseViewModel purchaseViewModel)
        {
            Bus bus = _busDBRepository.GetBusById(purchaseViewModel.busId);
            purchaseViewModel.bus = bus;
            // -------- TICKET SETTING ------------- //
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
                journeyTime = purchaseViewModel.journeyTime,
                checkFood = purchaseViewModel.checkFood


            };



            if (purchaseViewModel.checkVoucher == true)
            {
                ticket.cost = ticket.cost - (int)(ticket.cost * (purchaseViewModel.discount / 100.00));
            }
            if (purchaseViewModel.checkFood == true)
            {
                if (purchaseViewModel.bus.TourBus == true)
                {
                    ticket.cost += (100 * 40);
                    ticket.currentAva = 40;
                }
                else
                {
                    ticket.cost += 100;

                }
            }

            //   = _busDBRepository.AddTicket(ticket);

            BusSeatNew busSeat = new BusSeatNew
            {
                Id = purchaseViewModel.busseatid,
                busId = id,
                seatNo = purchaseViewModel.currentAva + 1
            };
            if (purchaseViewModel.bus.TourBus == true)
            {
                busSeat.seatNo = 41;
            }

            _busDBRepository.UpdateBusSeat(busSeat);

            return RedirectToAction("payment", "home", ticket);
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

        [Route("Payment")]
        [HttpGet]
        [Authorize]
        public IActionResult Payment(Ticket ticket)
        {
            string user = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("UserInfo"));

            Ticket ticket1 = ticket;
            
            ticket1.name = user;

            PaymentViewModel paymentViewModel = new PaymentViewModel
            {
                ticket = ticket1
            };


            return View(ticket);
        }

        [Route("Payment2")]
        [HttpPost]
        [Authorize]
        public IActionResult Payment2(Ticket mticket)
        {
            string user = JsonConvert.DeserializeObject<string>(HttpContext.Session.GetString("UserInfo"));



            Ticket ticket = new Ticket
            {
                busId = mticket.busId,
                currentAva = mticket.currentAva,
                name = user,
                From = mticket.From,
                To = mticket.To,
                busTime = mticket.busTime,
                cost = mticket.cost,
                purchaseTime = mticket.purchaseTime,
                userId = mticket.userId,
                journeyTime = mticket.journeyTime,
                checkFood = mticket.checkFood,
                phone = mticket.phone,
                transactionNumber = mticket.transactionNumber,
                isConfirmed = 1



            };

/*
            ticket.name = user;
            ticket.isConfirmed = 1; // 1 = not confirmed, 2 = confirmed , 3 = cencel
            ticket.phone = paymentViewModel.phone;
            ticket.transactionNumber = paymentViewModel.transactionNumber;*/
            _busDBRepository.AddTicket(ticket);


            return RedirectToAction("purchaseHistory", "home");
        }
        [Route("returnTicket")]
        [Authorize]
        public IActionResult returnTicket(int ticketId)
        {
            Ticket ticket = _busDBRepository.GetTicket(ticketId);
            DateTime now = DateTime.Now;
            DateTime journeyDate = ticket.journeyTime;


            ;


            if (now < journeyDate)
            {
                _busDBRepository.DeleteTicket(ticketId);


            }

            return RedirectToAction("purchaseHistory", "Home");
        }

        [Route("voucherCheck")]
        [HttpPost]
        public IActionResult voucherCheck(PurchaseViewModel purchaseViewModel)
        {
            Bus bus = _busDBRepository.GetBusById(purchaseViewModel.busId);
            purchaseViewModel.bus = bus;

            IEnumerable<Voucher> vouchers = _busDBRepository.GetAllVoucher();
            bool checkVoucher = false;
            int discount = 0;
            foreach (Voucher voucher in vouchers)
            {
                if (voucher != null)
                {
                    if (voucher.code.Equals(purchaseViewModel.voucherCode))
                    {
                        //purchaseViewModel.bus.Cost -= (int)(purchaseViewModel.bus.Cost * 0.20);
                        checkVoucher = true;
                        discount = voucher.discount_percent;
                    }
                }
            }

            return RedirectToAction("Purchase", "Home", new { id = bus.Id, date = purchaseViewModel.journeyTime, voucherCode = purchaseViewModel.voucherCode, discount = discount, checkVoucher = checkVoucher });
        }

    }


}
