using Bus_Ticket_System.Models;
using Bus_Ticket_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bus_Ticket_System.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[Controller]")]
    public class AdminPanelController : Controller
    {
        private readonly IBusDBRepository _busDBRepository;
        private readonly AppDbContext _context;

        public AdminPanelController(IBusDBRepository busDBRepository, AppDbContext context)
        {
            _busDBRepository = busDBRepository;
            _context = context;

        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Bus")]

        public async Task<IActionResult> Bus(string sortOrder, Models.Route SearchFrom, Models.Route SearchTo)
        {

            ViewData["CostSortParam"] = String.IsNullOrEmpty(sortOrder) ? "cost_desc" : "";
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["CostSortParam"] = sortOrder == "cost" ? "cost_desc" : "cost";
            ViewData["FromFilter"] = SearchFrom;
            ViewData["DesFilter"] = SearchTo;
            var allBus = from s in _context.Buses
                         select s;

            // Enum x = Models.Route.Select;
            /*if (!SearchFrom.Equals(x) && !SearchTo.Equals(x))

                allBus = allBus.Where(bus => bus.From.Equals(SearchFrom) && bus.To.Equals(SearchTo)


                );
*/

            switch (sortOrder)
            {

                case "id_desc":
                    allBus = allBus.OrderByDescending(s => s.Id);
                    break;
                case "Id":
                    allBus = allBus.OrderBy(s => s.Id);
                    break;
                case "cost_desc":
                    allBus = allBus.OrderByDescending(s => s.Cost);
                    break;
                case "cost_asc":
                    allBus = allBus.OrderBy(s => s.Cost);
                    break;
                default:
                    allBus = allBus.OrderBy(s => s.Id);
                    break;
            }

            IEnumerable<Bus> buses = await allBus.AsNoTracking().ToListAsync();
            List<Bus>? busesModified = new List<Bus>();
            foreach (Bus bu in buses)
            {
                bu.ava_seat = 41 - _busDBRepository.GetBusSeats(bu.busSeatId).seatNo;
                _busDBRepository.Update(bu);
                if (bu != null)
                    busesModified.Add(bu);

            }

            BusAndListBusViewModel busAndListBusViewModels = new BusAndListBusViewModel
            {
                Buses = busesModified
            };


            ModelState.Remove("SearchFrom");
            ModelState.Remove("SearchTo");


            return View(busAndListBusViewModels);


        }


        /// ----------- BUS CREATE HTTP POST ----------- ///
        [Route("Bus")]
        [HttpPost]

        public IActionResult Bus(BusAndListBusViewModel busAndListBus)
        {
            if (ModelState.IsValid)
            {
                if ((busAndListBus.bus.From != busAndListBus.bus.To))
                {
                    Console.WriteLine(busAndListBus.bus.Id);


                    busAndListBus.bus.ava_seat = 40;
                    _busDBRepository.Add(busAndListBus.bus); /// _-------- BUS ADD TO DATABASE ----------- ///


                    BusSeatNew busSeatNew = new BusSeatNew();
                    busSeatNew.busId = _busDBRepository.GetBusById(busAndListBus.bus.Id).Id;
                    busSeatNew.seatNo = 1;
                    _busDBRepository.AddBusSeat(busSeatNew);

                    Bus newBus = _busDBRepository.GetBusById(busAndListBus.bus.Id);
                    newBus.busSeatId = busSeatNew.Id;
                    _busDBRepository.Update(newBus);



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
                ModelState.Clear();
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

        [Route("/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _busDBRepository.Delete(id);
            return RedirectToAction("Bus", "AdminPanel");
        }

        [Route("Edit")]
        [HttpGet]
        public IActionResult Edit(int id)
        {

            Bus bus = _busDBRepository.GetBusById(id);


            return View(bus);
        }
        [Route("Edit")]
        [HttpPost]
        public IActionResult Edit(Bus busChangee)
        {
            ;

            if (ModelState.IsValid)
            {

                _busDBRepository.Update(busChangee);


                return RedirectToAction("Bus", "AdminPanel");

            }


            return View();
        }
        [Route("Voucher")]
        [HttpGet]
        public async Task<IActionResult> VoucherAsync()
        {
            var vouchers = from s in _context.Vouchers
                           select s;
            IEnumerable<Voucher> vouchersEnum = await vouchers.AsNoTracking().ToListAsync();
            VoucherAndBoucherList viewModel = new VoucherAndBoucherList()
            {
                vouchers = vouchersEnum
            };

            return View(viewModel);


        }

        [Route("Voucher")]
        [HttpPost]

        public IActionResult Voucher(VoucherAndBoucherList viewModel)
        {
            if (ModelState.IsValid)
            {

                _busDBRepository.AddVoucher(viewModel.voucher);
                IEnumerable<Voucher> vouchers = _busDBRepository.GetAllVoucher();

                VoucherAndBoucherList voucherAndBoucher = new VoucherAndBoucherList
                {
                    vouchers = vouchers,
                    voucher = new Voucher()


                };
                ModelState.Clear();
                return View(voucherAndBoucher);


            }
            else
            {
                return View(viewModel);
            }

        }

        [Route("Voucher/Delete/{id}")]
        public IActionResult VoucherDelete(int id)
        {
            _busDBRepository.DeleteVoucher(id);
            return RedirectToAction("voucher", "AdminPanel");
        }

        [Route("ResetSeat")]
        public IActionResult ResetSeat(int id, int busId)
        {
            BusSeatNew busSeatNew = new BusSeatNew
            {
                Id = id,
                busId = busId,
                seatNo = 1
            };
            _busDBRepository.UpdateBusSeat(busSeatNew);
            return RedirectToAction("Bus", "AdminPanel");
        }




    }
}
