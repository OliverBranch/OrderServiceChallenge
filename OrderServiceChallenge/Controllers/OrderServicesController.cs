using System;
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
using System.Diagnostics;
using Aspose.Pdf;
using Aspose.Pdf.Text;
using System.Text;
using System.Web;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.Net;
using Microsoft.IdentityModel.Protocols;
using System.IO;
using IronPdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;


namespace OrderServiceChallenge.Controllers
{
    public class OrderServicesController : Controller
    {
        private readonly OrderServiceService _orderServiceService;
        private readonly EmployeeService _employeeService;
        private readonly CompanyService _companyService;

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

        // GET: OrderServices/Details/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {

                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var orderService = await _orderServiceService.FindByIdAsync(id.Value);
            if (orderService == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
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

        // GET: OrderServices/Edit/
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var orderService = await _orderServiceService.FindByIdAsync(id.Value);
            if (orderService == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }
            List<Employee> employees = await _employeeService.FindAllAsync();
            List<Company> companies = await _companyService.FindAllAsync();
            OrderServiceViewModel viewModel = new OrderServiceViewModel { OrderService = orderService, Employees = employees, Companies = companies };
            return View(viewModel);
        }

        // POST: OrderServices/Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderService orderService)
        {
            if (id != orderService.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" });
            }

            try
            {
                await _orderServiceService.UpdateAsync(orderService);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        // GET: OrderServices/Delete/
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var orderService = await _orderServiceService.FindByIdAsync(id.Value);
            if (orderService == null)
            {

                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(orderService);
        }

        // POST: OrderServices/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderServiceService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }

        public IActionResult Search()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue)
                maxDate = DateTime.Now;

            ViewData["minDate"] = minDate.Value.ToString("yyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyy-MM-dd");

            var result = await _orderServiceService.FindByDateAsync(minDate, maxDate);
            return View(result);
        }
        public async Task<IActionResult> CompanySearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue)
                maxDate = DateTime.Now;

            ViewData["minDate"] = minDate.Value.ToString("yyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyy-MM-dd");

            var result = await _orderServiceService.FindByDateCompanyAsync(minDate, maxDate);
            return View(result);
        }
        public async Task<IActionResult> EmployeeSearch(DateTime? minDate, DateTime? maxDate)
        {
            if (!minDate.HasValue)
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (!maxDate.HasValue)
                maxDate = DateTime.Now;

            ViewData["minDate"] = minDate.Value.ToString("yyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyy-MM-dd");

            var result = await _orderServiceService.FindByDateEmployeeAsync(minDate, maxDate);
            return View(result);
        }



        public void PdfListGenerator()
        {
            //FIleResult

        }


        public FileResult Test()
        {

            var path = $"C:\\Users\\israel.barbosa\\source\\repos\\OrderServiceChallenge\\OrderServiceChallenge\\Data\\temp\\";
            using (WebClient client = new WebClient())
            {
                client.DownloadFile("https://localhost:5001/OrderServices", path + "index.txt");




            }


            string line = null;
            int line_number = 1;
            int line_to_delete = 14 + 49 + 20;
            string addline = "<!DOCTYPE html><html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>Index - OrderServiceChallenge</title><link rel=\"stylesheet\" href=\"./completa_files/bootstrap-darkly.css\"><link rel=\"stylesheet\" href=\"./completa_files/site.css\"></head><body><div class=\"container body-content\"><h2> Orders </h2> ";

            using (StreamReader reader = new StreamReader(path + "index.txt"))
            {
                using (StreamWriter writer = new StreamWriter(path + "indexout.html"))
                {
                    writer.WriteLine(addline);
                    while ((line = reader.ReadLine()) != null)
                    {
                        line_number++;

                        if (line.Contains("href=\"/OrderServices/"))
                            continue;




                        if (line_number <= line_to_delete)
                            continue;

                        writer.WriteLine(line);

                    }
                }
            }
            var Renderer = new IronPdf.HtmlToPdf();
            var PDF = Renderer.RenderHTMLFileAsPdf(path+"indexout.html");
            var OutputPath = path+"List.pdf";
            PDF.SaveAs(OutputPath);

            /* ASPOSE
             * HtmlLoadOptions htmloptions = new HtmlLoadOptions();
            Document doc = new Document(path+ "indexout.html", htmloptions);
            doc.Save(path + "List.pdf");*/

            byte[] bytes = System.IO.File.ReadAllBytes(path+"List.pdf");
            return File(bytes, "text/pain", "testando.pdf");

        }

    }
}


