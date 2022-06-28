using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_System.Models
{
    public class Bus
    {
        [Key]
        public int Id { get; set; }
        public int Type { get; set; }

        public int Cost { get; set; }

        public Route From { get; set; }
        
        public Route To { get; set; }
    }
}
