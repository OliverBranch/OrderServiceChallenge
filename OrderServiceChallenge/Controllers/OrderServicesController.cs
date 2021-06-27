﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OrderServiceChallenge.Data;
using OrderServiceChallenge.Models;
using OrderServiceChallenge.Services;
using OrderServiceChallenge.Models.ViewModels;
using OrderServiceChallenge.Services.Exceptions;

namespace OrderServiceChallenge.Controllers
{
    public class OrderServicesController : Controller
    {
        private readonly OrderServiceService _orderServiceService;
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;
        private readonly OrderServiceChallengeContext _context;

        public OrderServiceViewModel OrderServiceViewModel { get; private set; }

        public OrderServicesController(OrderServiceService orderServiceService, EmployeeService employeeService, CompanyService companyService)
        {
            _orderServiceService = orderServiceService;
            _employeeService = employeeService;
            _companyService = companyService;

        }

        // GET: OrderServices
        public async Task<IActionResult> Index()
        {
            var list = await _orderServiceService.FindAllAsync();
            return View(list);
        }

        // GET: OrderServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _orderServiceService.FindByIdAsync(id.Value);
            if (orderService == null)
            {
                return NotFound();
            }

            return View(orderService);
        }

        // GET: OrderServices/Create
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.FindAllAsync();
            var companies = await _companyService.FindAllAsync();
            var viewModel = new OrderServiceViewModel { Employees = employees, Companies = companies };
            return View(viewModel);
        }

        // POST: OrderServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderService orderService)
        {
            if (ModelState.IsValid)
            {
                await _orderServiceService.InsertAsync(orderService);
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

            var orderService = await _orderServiceService.FindByIdAsync(id.Value);
            if (orderService == null)
            {
                return NotFound();
            }
            List<Employee> employees = await _employeeService.FindAllAsync();
            List<Company> companies = await _companyService.FindAllAsync();
            OrderServiceViewModel viewModel = new OrderServiceViewModel { OrderService = orderService, Employees = employees, Companies = companies };
            return View(viewModel);
        }

        // POST: OrderServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderService orderService)
        {
            if (id != orderService.Id)
            {
                return NotFound();
            }

            try
            {
                await _orderServiceService.UpdateAsync(orderService);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }
            catch (NotFoundException)
            {
                return BadRequest();
            }
        }

        // GET: OrderServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderService = await _orderServiceService.FindByIdAsync(id.Value);
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
            await _orderServiceService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool OrderServiceExists(int id)
        {
            return _context.OrderService.Any(e => e.Id == id);
        }
    }
}
