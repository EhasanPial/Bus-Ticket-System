using Bus_Ticket_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Bus_Ticket_System.Models
{
    public class ClassBusDBRepository : IBusDBRepository
    {
        public readonly AppDbContext context;
        public ClassBusDBRepository(AppDbContext appDb)
        {
            this.context = appDb;
        }
        public AppDbContext GetDbContext()
        {
            return context;
        }
        public Bus Add(Bus? newBus)
        {
            context.Buses.Add(newBus);
            context.SaveChanges();
            return newBus;
        }

        public void Delete(int? id)
        {
            Bus bus = context.Buses.Find(id);
            if (bus != null)
            {
                context.Buses.Remove(bus);
                context.SaveChanges();
            }

        }

        public IEnumerable<Bus> GetAllBus()
        {

            return context.Buses;

        }

        public Bus GetBusById(int? id)
        {
            return context.Buses.Find(id);
        }




        // -------------------------------------- BUS SEAT ------------------------------------------- //

        public IEnumerable<BusSeatNew> GetBusSeats()
        {
            return context.BusSeatsNew;
        }

        public BusSeatNew GetBusSeats(int? id)
        {
            return context.BusSeatsNew.Find(id);
        }

        public BusSeatNew UpdateBusSeat(BusSeatNew? busSeat)
        {
            var bus = context.BusSeatsNew.Attach(busSeat);

            bus.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return busSeat;
        }
        public BusSeatNew AddBusSeat(BusSeatNew? busSeat)
        {
            context.BusSeatsNew.Add(busSeat);
            context.SaveChanges();
            return busSeat;
        }

        public Bus Update(Bus? busChange)
        {
            var bus = context.Buses.Attach(busChange);
            bus.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return busChange;
        }
        public void DeleteBusSeat(int id)
        {
            BusSeatNew busSeatNew = context.BusSeatsNew.Find(id);
            if (busSeatNew != null)
            {
                context.BusSeatsNew.Remove(busSeatNew);
                context.SaveChanges();
            }
        }



        // -------------------------- TICKET ---------------------------------- //
        public Ticket AddTicket(Ticket ticket)
        {
            context.Tickets.Add(ticket);
            context.SaveChanges();
            return ticket;
        }


        public void DeleteTicket(int? id)
        {
            Ticket ticket = context.Tickets.Find(id);
            if (ticket != null)
            {
                context.Tickets.Remove(ticket);
                context.SaveChanges();
            }
        }

        public IEnumerable<Ticket> GetAllTicket()
        {
            return context.Tickets;
        }

        public Ticket GetTicket(int? id)
        {
            return context.Tickets.Find(id);
        }

        public IEnumerable<BusSeatNew> GetAllBusSeats()
        {
            throw new NotImplementedException();
        }

        public Voucher AddVoucher(Voucher voucher)
        {
            context.Vouchers.Add(voucher);
            context.SaveChanges();
            return voucher;
        }

        public void DeleteVoucher(int? id)
        {
            Voucher voucher = context.Vouchers.Find(id);
            if (voucher != null)
            {
                context.Vouchers.Remove(voucher);
                context.SaveChanges();
            }
        }

        public IEnumerable<Voucher> GetAllVoucher()
        {
            return context.Vouchers;
        }

        public Voucher GetVourcher(int? id)
        {
            return context.Vouchers.Find(id);
        }


    }
}
