using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.ViewModels
{
    public class VoucherAndBoucherList
    {
        public IEnumerable<Voucher>? vouchers { get; set; }
        public Voucher? voucher { get; set; }
        
       
    }
}
