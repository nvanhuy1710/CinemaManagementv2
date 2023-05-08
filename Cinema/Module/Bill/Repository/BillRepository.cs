﻿using Cinema.Data;
using Cinema.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cinema.Module.Bill.Repository
{
    public class BillRepository : IBillRepository
    {

        private readonly DataContext _context;

        public BillRepository(DataContext context)
        {
            _context = context;
        }

        public BillModel AddBill(BillModel model)
        {
            EntityEntry<BillModel> entityEntry = _context.Bills.Add(model);
            _context.SaveChanges();
            return entityEntry.Entity;
        }

        public List<BillModel> GetBillByUserId(int userId)
        {
            return _context.Bills.Include(p => p.ReservationModels).
                ThenInclude(y => y.SeatModel).Include(p => p.FoodOrderModels).
                ThenInclude(y => y.FoodModel).Where(p => p.UserId == userId).ToList();
        }
    }
}