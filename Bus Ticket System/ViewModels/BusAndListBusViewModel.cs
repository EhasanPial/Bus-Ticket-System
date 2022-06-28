using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.ViewModels
{
    public class BusAndListBusViewModel
    {
        public IEnumerable<Bus>? Buses { get; set; }
        public Bus? bus { get; set; }
        
       
    }
}
