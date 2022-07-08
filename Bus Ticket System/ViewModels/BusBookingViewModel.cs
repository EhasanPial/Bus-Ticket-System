using Bus_Ticket_System.Models;
using Bus_Ticket_System.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_System.ViewModels
{
    public class BusBookingViewModel
    {
        public IEnumerable<Bus>? Buses { get; set; }

        [Required]
        [DateTimeValidation(ErrorMessage = "Select a valid date")]
        public int busId { get; set; }
        public Models.Route? SearchFrom { get; set; }
        public Models.Route? SearchTo { get; set; }
        public BusType? SeachBusType { get; set; }

        public DateTime datePassing { get; set; }


    }
}
