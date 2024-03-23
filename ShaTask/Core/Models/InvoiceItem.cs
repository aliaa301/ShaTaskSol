namespace ShaTask.Core.Models
{
    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } // Many-to-One relationship with Invoice
        
    }
}
