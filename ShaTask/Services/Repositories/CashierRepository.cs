
using ShaTask.Core.Models;
using ShaTask.Data.Context;
using ShaTask.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
namespace ShaTask.Services.Repositories
{

    public class CashierRepository : ICashierRepository
    {
        private readonly ApplicationDbContext _context;

        public CashierRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Cashier GetCashierById(int id)
        {
            return _context.Cashiers.FirstOrDefault(c => c.CashierId == id);
        }

        public IEnumerable<Cashier> GetAllCashiers()
        {
            return _context.Cashiers.ToList();
        }

        public void AddCashier(Cashier cashier)
        {
            _context.Cashiers.Add(cashier);
            _context.SaveChanges();
        }

        public void UpdateCashier(Cashier cashier)
        {
            _context.Cashiers.Update(cashier);
            _context.SaveChanges();
        }

        public void DeleteCashier(int id)
        {
            var cashierToDelete = _context.Cashiers.FirstOrDefault(c => c.CashierId == id);
            if (cashierToDelete != null)
            {
                _context.Cashiers.Remove(cashierToDelete);
                _context.SaveChanges();
            }
        }
    }
}
