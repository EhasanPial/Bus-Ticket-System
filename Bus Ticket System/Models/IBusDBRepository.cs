﻿using Bus_Ticket_System.Models;

namespace Bus_Ticket_System.Models
{
    public interface IBusDBRepository
    {
        IEnumerable<Bus> GetAllBus();

        Bus GetBusById(int? id);
        Bus Add(Bus? newBus);
        Bus Update(Bus? bus);
        void Delete(int? id);


        IEnumerable<BusSeatNew> GetAllBusSeats();
        BusSeatNew GetBusSeats(int? id);
        BusSeatNew UpdateBusSeat(BusSeatNew? busSeat);
        BusSeatNew AddBusSeat(BusSeatNew? busSeat);
        void DeleteBusSeat(int id);


        Ticket AddTicket(Ticket ticket);
        void DeleteTicket(int? id);
        IEnumerable<Ticket> GetAllTicket();
        Ticket GetTicket(int? id);
        Ticket UpdateTicket(Ticket ticket);

        Voucher AddVoucher(Voucher voucher);
        void DeleteVoucher(int? id);
        IEnumerable<Voucher> GetAllVoucher();
        Voucher GetVourcher(int? id);


    }
}
