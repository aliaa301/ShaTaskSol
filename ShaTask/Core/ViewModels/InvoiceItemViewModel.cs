namespace ShaTask.Core.ViewModels
{
    public class InvoiceItemViewModel
    {
        public int InvoiceItemId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
