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

            Enum x = Models.Route.Select;
            if (!SearchFrom.Equals(x) && !SearchTo.Equals(x))

                allBus = allBus.Where(bus => bus.From.Equals(SearchFrom) && bus.To.Equals(SearchTo)


                );


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

            BusAndListBusViewModel busAndListBusViewModels = new BusAndListBusViewModel
            {
                Buses = buses
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





    }
}
