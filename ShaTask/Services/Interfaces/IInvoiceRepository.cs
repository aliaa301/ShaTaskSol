using ShaTask.Core.Models;
using ShaTask.Core.ViewModels;

namespace ShaTask.Services.Interfaces
{
    public interface IInvoiceRepository
    {

        Invoice GetInvoiceById(int id);
        IEnumerable<Invoice> GetAllInvoices();
        void AddInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(int id);

        IEnumerable<InvoiceViewModel> GetAllInvoicesWithItems();
        InvoiceViewModel GetInvoiceWithItemsById(int id);
        Invoice GetInvoiceByItemId(int itemId);

    }
}
