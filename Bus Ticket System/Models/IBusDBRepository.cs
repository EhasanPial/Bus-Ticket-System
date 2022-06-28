using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.Models
{
    public interface IBusDBRepository
    {
        IEnumerable<Bus> GetAllBus();
         
        Bus GetBusById(int? id);
        Bus Add(Bus? newBus);
        Bus Update(Bus? bus);
        void Delete(int? id);



    }
}
