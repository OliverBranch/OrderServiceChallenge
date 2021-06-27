using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderServiceChallenge.Data;
using OrderServiceChallenge.Models;

namespace OrderServiceChallenge.Controllers
{
    public class OrderServicesController : Controller
    {
        private readonly OrderServiceChallengeContext _context;

        public OrderServicesController(OrderServiceChallengeContext context)
        {
            _context = context;
        }

        // GET: OrderServices
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrderService.ToListAsync());
        }

        // GET: OrderServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _context.OrderService
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // GET: OrderServices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NumberOS,ServiceTitle,Value,ExecutionDate")] OrderService orderService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orderService);
        }

        // GET: OrderServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _context.OrderService.FindAsync(id);
            if (orderService == null)
            {
                return NotFound();
            }
            return View(orderService);
        }

        // POST: OrderServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NumberOS,ServiceTitle,Value,ExecutionDate")] OrderService orderService)
        {
            if (id != orderService.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderServiceExists(orderService.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(orderService);
        }

        // GET: OrderServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _context.OrderService
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // POST: OrderServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderService = await _context.OrderService.FindAsync(id);
            _context.OrderService.Remove(orderService);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderServiceExists(int id)
        {
            return _context.OrderService.Any(e => e.Id == id);
        }
    }
}
