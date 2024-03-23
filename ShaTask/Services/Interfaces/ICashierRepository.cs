using ShaTask.Core.Models;

namespace ShaTask.Services.Interfaces
{
    public interface ICashierRepository
    {
        Cashier GetCashierById(int id);
        IEnumerable<Cashier> GetAllCashiers();
        void AddCashier(Cashier cashier);
        void UpdateCashier(Cashier cashier);
        void DeleteCashier(int id);
    }
}
