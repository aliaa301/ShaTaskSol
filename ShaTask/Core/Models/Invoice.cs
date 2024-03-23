namespace ShaTask.Core.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string CustomerName { get; set; }
        public DateTime InvoiceDate { get; set; }
        public ICollection<InvoiceItem> Items { get; set; } // One-to-Many relationship with InvoiceItem
        public int CashierId { get; set; }
        public Cashier Cashier { get; set; } // Many-to-One relationship with Cashier
        public decimal TotalPrice { get; set; }
        

    }
}
