using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.ViewModels
{
    public class BusBookingViewModel
    {
        public IEnumerable<Bus>? Buses { get; set; }
        public DateTime DateTime { get; set; }  
        public int busId { get; set; }  
       

    }
}
