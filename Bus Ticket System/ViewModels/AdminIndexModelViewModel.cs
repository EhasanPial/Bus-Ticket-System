using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.ViewModels
{
    public class AdminIndexModelViewModel
    {
        public IEnumerable<Bus> tourBuses { get; set; }
        public IEnumerable<Bus> normalBuses { get; set; }
        public IEnumerable<Ticket> tickets { get; set; }
        public IEnumerable<Voucher> vouchers { get; set; }

        public int total_sell { get; set; }
        public int total_tickets { get; set; }


    }
}
