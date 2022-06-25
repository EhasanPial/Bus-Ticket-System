using Microsoft.AspNetCore.Identity;

namespace Bus_Ticket_System.Models
{

    public class ApplicationUser : IdentityUser
    {
        public int PhoneNumber { get; set; }
        public string Name { get; set; }
    }

}
