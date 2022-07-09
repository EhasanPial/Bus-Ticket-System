using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bus_Ticket_System.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {





        }

        public DbSet<Bus> Buses { get; set; }
       
        public DbSet<BusSeatNew> BusSeatsNew { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        

    }
}
