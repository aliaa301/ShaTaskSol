
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShaTask.Core.Models;
using ShaTask.Core.ViewModels;
using ShaTask.Data.Context;
using ShaTask.Services.Interfaces;
namespace ShaTask.Services.Repositories
{


    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Invoice GetInvoiceById(int id)
        {
            return _context.Invoices.Include(i => i.Items).FirstOrDefault(i => i.InvoiceId == id);
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _context.Invoices.ToList();
        }

        public void AddInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            _context.SaveChanges();
        }

        public void DeleteInvoice(int id)
        {
            var invoice = _context.Invoices.FirstOrDefault(i => i.InvoiceId == id);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                _context.SaveChanges();
            }
        }

        public IEnumerable<InvoiceViewModel> GetAllInvoicesWithItems()
        {
            return _context.Invoices
                .Include(i => i.Items)
                .Select(i => new InvoiceViewModel
                {
                    InvoiceId = i.InvoiceId,
                    CustomerName = i.CustomerName,
                    InvoiceDate = i.InvoiceDate,
                    CashierId = i.CashierId,
                    CashierName = i.Cashier.Name,
                    Items = i.Items.Select(item => new InvoiceItemViewModel
                    {
                        InvoiceItemId = item.InvoiceItemId,
                        Description = item.Description,
                        Price = item.Price,
                        Quantity = item.Quantity
                    }).ToList(),
                    TotalPrice = i.Items.Sum(item => item.Price * item.Quantity)
                })
                .ToList();
        }

        public InvoiceViewModel GetInvoiceWithItemsById(int id)
        {

            var invoice = _context.Invoices
                .Include(i => i.Items)
                .Include(i => i.Cashier) // Include Cashier navigation property
                .FirstOrDefault(i => i.InvoiceId == id);
            if (invoice == null)
                return null;

            return new InvoiceViewModel
            {
                InvoiceId = invoice.InvoiceId,
                CustomerName = invoice.CustomerName,
                InvoiceDate = invoice.InvoiceDate,
                CashierId = invoice.CashierId,
                CashierName = invoice.Cashier.Name,
                Items = invoice.Items.Select(item => new InvoiceItemViewModel
                {
                    InvoiceItemId = item.InvoiceItemId,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList(),
                TotalPrice = invoice.Items.Sum(item => item.Price * item.Quantity)
            };
        }
        public Invoice GetInvoiceByItemId(int itemId)
        {
            // Implement logic to retrieve an invoice by item ID from the database
            // For example:
            var invoice = _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefault(i => i.Items.Any(item => item.InvoiceItemId == itemId));

            return invoice;
        }
    }
}