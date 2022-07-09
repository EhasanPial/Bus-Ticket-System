using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.ViewModels
{
    public class PurchaseViewModel
    {
       
        public int busId { get; set; }
        public int busseatid { get; set; }
        public int currentAva { get; set; }
        public string name { get; set; }    
        public Bus bus  { get; set; }

        public DateTime purchaseTime { get; set; }
        public DateTime journeyTime { get; set; }

        public string userId { get; set; }


    }
}
