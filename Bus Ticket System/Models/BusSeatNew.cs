using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bus_Ticket_System.Models
{
    public class BusSeatNew
    {

        [Key]
        public int Id { get; set; }

        public int seatNo { get; set; }

        public int busId { get; set; }



    }
}
