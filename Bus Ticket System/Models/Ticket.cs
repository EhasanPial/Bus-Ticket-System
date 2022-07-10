using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_System.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int busId { get; set; }
       
        public int currentAva { get; set; }
        public string name { get; set; }
        public Models.Route From { get; set; }
        public Models.Route To { get; set; }
        public DateTime busTime { get; set; }
        public int cost { get; set; }

        public DateTime purchaseTime { get; set; }
        public DateTime journeyTime { get; set; }

        public string userId { get; set; }
        public bool checkFood { get; set; }
    }
}
