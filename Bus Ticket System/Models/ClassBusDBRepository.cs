using Bus_Ticket_System.Models;
namespace Bus_Ticket_System.Models
{
    public class ClassBusDBRepository : IBusDBRepository
    {
        private readonly AppDbContext context;
        public ClassBusDBRepository(AppDbContext appDb)
        {
            this.context = appDb;
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

        public Bus Update(Bus? busChange)
        {
            var bus = context.Buses.Attach(busChange);
            bus.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return busChange;
        }


    }
}
