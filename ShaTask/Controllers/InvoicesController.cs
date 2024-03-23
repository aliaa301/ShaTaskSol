

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaTask.Core.Models;
using ShaTask.Core.ViewModels;
using ShaTask.Services.Interfaces;
using System.Linq;

namespace ShaTask.Controllers
{
    [Authorize(Roles = "Admin")]

    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICashierRepository _cashierRepository;

        public InvoiceController(IInvoiceRepository invoiceRepository, ICashierRepository cashierRepository)
        {
            _invoiceRepository = invoiceRepository;
            _cashierRepository = cashierRepository;
        }

        // GET: /Invoice
        public IActionResult Index()
        {
            var invoices = _invoiceRepository.GetAllInvoicesWithItems()
                .Select(invoice =>
                {
                    invoice.TotalPrice = invoice.Items.Sum(item => item.Price * item.Quantity);
                    return invoice;
                });

            return View(invoices);
        }


       
        // GET: /Invoice/Details/5
        public IActionResult Details(int id)
        {
            var invoice = _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound();
            }
            // Calculate total price
            decimal totalPrice = 0;
            foreach (var item in invoice.Items)
            {
                totalPrice += item.Price * item.Quantity;
            }

            // Pass the invoice and total price to the view
            ViewBag.TotalPrice = totalPrice;
            return View(invoice);
        }


        // GET: /Invoice/Create
        public IActionResult Create()
        {
            var availableCashiers = _cashierRepository.GetAllCashiers()
                .Select(c => new CashierViewModel
                {
                    CashierId = c.CashierId,
                    Name = c.Name
                })
                .ToList();

            var viewModel = new InvoiceViewModel
            {
                AvailableCashiers = availableCashiers
            };

            return View(viewModel);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InvoiceViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                // Map ViewModel to Domain Model
                var invoice = new Invoice
                {
                    CustomerName = viewModel.CustomerName,
                    InvoiceDate = viewModel.InvoiceDate,
                    CashierId = viewModel.CashierId,
                    Items = new List<InvoiceItem>(), // Initialize the Items collection
                    TotalPrice = 0 // Initialize the TotalPrice
                };

                // Calculate total price and add invoice items
                decimal totalPrice = 0;
                foreach (var item in viewModel.Items)
                {
                    var invoiceItem = new InvoiceItem
                    {
                        Description = item.Description,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };

                    invoice.Items.Add(invoiceItem);
                    totalPrice += invoiceItem.Price * invoiceItem.Quantity; // Update total price
                }

                // Set the calculated total price
                invoice.TotalPrice = totalPrice;

                // Add the invoice
                _invoiceRepository.AddInvoice(invoice);
                return RedirectToAction(nameof(Index));
            }

            // If the model state is not valid, return to the Create view with the existing view model data
            viewModel.AvailableCashiers = _cashierRepository.GetAllCashiers()
                .Select(c => new CashierViewModel
                {
                    CashierId = c.CashierId,
                    Name = c.Name
                    // Assign other properties as needed
                })
                .ToList();

            return View(viewModel);
        }


        // GET: /Invoice/Edit/5
        public IActionResult Edit(int id)
        {
            var invoice = _invoiceRepository.GetInvoiceWithItemsById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            var availableCashiers = _cashierRepository.GetAllCashiers()
                .Select(c => new CashierViewModel
                {
                    CashierId = c.CashierId,
                    Name = c.Name
                })
                .ToList();

            var viewModel = new InvoiceViewModel
            {
                InvoiceId = invoice.InvoiceId,
                CustomerName = invoice.CustomerName,
                InvoiceDate = invoice.InvoiceDate,
                CashierId = invoice.CashierId,
                CashierName = invoice.CashierName,
                Items = invoice.Items.Select(item => new InvoiceItemViewModel
                {
                    InvoiceItemId = item.InvoiceItemId,
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList(),
                TotalPrice = invoice.TotalPrice,
                AvailableCashiers = availableCashiers
            };

            return View(viewModel);
        }

        // POST: /Invoice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, InvoiceViewModel viewModel)
        {
            if (id != viewModel.InvoiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Reload available cashiers
                viewModel.AvailableCashiers = _cashierRepository.GetAllCashiers()
                    .Select(c => new CashierViewModel
                    {
                        CashierId = c.CashierId,
                        Name = c.Name
                    })
                    .ToList();

                return View(viewModel);
            }

            var invoice = _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            // Update invoice properties
            invoice.CustomerName = viewModel.CustomerName;
            invoice.InvoiceDate = viewModel.InvoiceDate;
            invoice.CashierId = viewModel.CashierId;

            // Clear existing items and add new ones
            invoice.Items.Clear();
            foreach (var item in viewModel.Items)
            {
                invoice.Items.Add(new InvoiceItem
                {
                    Description = item.Description,
                    Price = item.Price,
                    Quantity = item.Quantity
                });
            }

            // Update invoice in the repository
            _invoiceRepository.UpdateInvoice(invoice);

            return RedirectToAction(nameof(Index));
        }




        // GET: /Invoice/Delete/5
        public IActionResult Delete(int id)
        {
            var invoice = _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: /Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _invoiceRepository.DeleteInvoice(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: /Invoice/AddItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddItem(int id, InvoiceItemViewModel item)
        {
            var invoice = _invoiceRepository.GetInvoiceById(id);
            if (invoice == null)
            {
                return NotFound();
            }

            // Create a new InvoiceItem based on the provided ViewModel data
            var newItem = new InvoiceItem
            {
                Description = item.Description,
                Price = item.Price,
                Quantity = item.Quantity
            };

            // Add the new item to the invoice
            invoice.Items.Add(newItem);

            // Update the total price of the invoice
            invoice.TotalPrice += newItem.Price * newItem.Quantity;

            // Update the invoice in the repository
            _invoiceRepository.UpdateInvoice(invoice);

            return RedirectToAction(nameof(Details), new { id = invoice.InvoiceId });
        }

        // POST: /Invoice/UpdateItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateItem(int id, InvoiceItemViewModel item)
        {
            var invoice = _invoiceRepository.GetInvoiceByItemId(id);
            if (invoice == null)
            {
                return NotFound();
            }

            // Find the invoice item by its ID
            var invoiceItem = invoice.Items.FirstOrDefault(i => i.InvoiceItemId == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }

            // Update the properties of the invoice item
            invoiceItem.Description = item.Description;
            invoiceItem.Price = item.Price;
            invoiceItem.Quantity = item.Quantity;

            // Update the total price of the invoice
            invoice.TotalPrice = invoice.Items.Sum(i => i.Price * i.Quantity);

            // Update the invoice in the repository
            _invoiceRepository.UpdateInvoice(invoice);

            return RedirectToAction(nameof(Details), new { id = invoice.InvoiceId });
        }

        // POST: /Invoice/DeleteItem/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteItem(int id)
        {
            var invoice = _invoiceRepository.GetInvoiceByItemId(id);
            if (invoice == null)
            {
                return NotFound();
            }

            // Find the invoice item by its ID
            var invoiceItem = invoice.Items.FirstOrDefault(i => i.InvoiceItemId == id);
            if (invoiceItem == null)
            {
                return NotFound();
            }

            // Remove the item from the invoice
            invoice.Items.Remove(invoiceItem);

            // Update the total price of the invoice
            invoice.TotalPrice = invoice.Items.Sum(i => i.Price * i.Quantity);

            // Update the invoice in the repository
            _invoiceRepository.UpdateInvoice(invoice);

            return RedirectToAction(nameof(Details), new { id = invoice.InvoiceId });
        }


    }
}