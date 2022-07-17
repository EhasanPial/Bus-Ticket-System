using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.ViewModels
{
    public class PaymentViewModel
    {
        public Ticket ticket { get; set; }
        public string transactionNumber { get; set; }   
        public string phone { get; set; }   

    }
}
