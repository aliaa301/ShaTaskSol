namespace ShaTask.Core.ViewModels
{
    public class InvoiceViewModel
    {
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int CashierId { get; set; }
        public string CashierName { get; set; }
        public ICollection<InvoiceItemViewModel> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CashierViewModel> AvailableCashiers { get; set; }
        public InvoiceViewModel()
        {
            AvailableCashiers = new List<CashierViewModel>();
            Items = new List<InvoiceItemViewModel>(); // Initialize Items list

        }


    }
}
