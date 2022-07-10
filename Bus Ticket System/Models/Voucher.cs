using System.ComponentModel.DataAnnotations;

namespace Bus_Ticket_System.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }
        public string code { get; set; }
        public int discount_percent { get; set; }
    }
}
