using Microsoft.AspNetCore.Mvc;
using ShaTask.Core.Models;
using ShaTask.Services.Interfaces;
using System.Linq;
namespace ShaTask.Controllers
{
   

    public class CashiersController : Controller
    {
        private readonly ICashierRepository _cashierRepository;

        public CashiersController(ICashierRepository cashierRepository)
        {
            _cashierRepository = cashierRepository;
        }

        // GET: Cashiers
        public IActionResult Index()
        {
            var cashiers = _cashierRepository.GetAllCashiers();
            return View(cashiers);
        }

        // GET: Cashiers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = _cashierRepository.GetCashierById(id.Value);
            if (cashier == null)
            {
                return NotFound();
            }

            return View(cashier);
        }

        // GET: Cashiers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cashiers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cashier cashier)
        {
            if (!ModelState.IsValid)
            {
                _cashierRepository.AddCashier(cashier);
                return RedirectToAction(nameof(Index));
            }
            return View(cashier);
        }

        // GET: Cashiers/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cashier = _cashierRepository.GetCashierById(id.Value);
            if (cashier == null)
            {
                return NotFound();
            }
            return View(cashier);
        }

        // POST: Cashiers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Cashier cashier)
        {
            if (id != cashier.CashierId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _cashierRepository.UpdateCashier(cashier);
                return RedirectToAction(nameof(Index));
            }
            return View(cashier);
        }

        // GET: Cashiers/Delete/5
        public IActionResult Delete(int id)
        {
          _cashierRepository.DeleteCashier(id);
                return RedirectToAction(nameof(Index));
            }
        }

       
    }


