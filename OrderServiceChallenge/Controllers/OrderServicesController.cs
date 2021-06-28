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

        // GET: OrderServices/Details/5
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

        // POST: OrderServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: OrderServices/Delete/5
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

        // POST: OrderServices/Delete/5
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



        public FileResult PdfListGenerator()
        {
            var list = _orderServiceService.FindAllAsync().Result;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // New Document
            PdfDocument document = new PdfDocument();
            document.Info.Title = " Order Services ";

            // New Page
            PdfPage page = document.AddPage();



            XGraphics gfx = XGraphics.FromPdfPage(page);

            // Generate Header
            gfx.DrawString("Order Service List", new XFont("Arial", 40, XFontStyle.Bold), XBrushes.Red, new XPoint(200, 70));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(100, 100), new XPoint(500, 100));

            //Generate Table
            gfx.DrawString("Title", new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(100, 280));
            gfx.DrawString("Employee", new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(250, 280));
            gfx.DrawString("Company", new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(400, 280));


            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(50, 290), new XPoint(550, 290));

            int currentYPositionValues = 300;
            int currentYPositionLines = 310;

            if (list.Count <= 20)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    gfx.DrawString(list[i].ServiceTitle, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(100, currentYPositionValues));
                    gfx.DrawString(list[i].Employee.Name, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(250, currentYPositionValues));
                    gfx.DrawString(list[i].Company.Name, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(400, currentYPositionValues));
                    gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 90)), new XPoint(50, currentYPositionLines), new XPoint(550, currentYPositionLines));

                    currentYPositionValues += 20;
                    currentYPositionLines += 20;
                }

            }
            else
            {
                for (int i = 0; i < 15; i++)
                {
                    gfx.DrawString(list[i].ServiceTitle, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(100, currentYPositionValues));
                    gfx.DrawString(list[i].Employee.Name, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(250, currentYPositionValues));
                    gfx.DrawString(list[i].Company.Name, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(400, currentYPositionValues));
                    gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 90)), new XPoint(50, currentYPositionLines), new XPoint(550, currentYPositionLines));

                    currentYPositionValues += 20;
                    currentYPositionLines += 20;
                    list.Remove(list[i]);
                }
                page = document.AddPage();
                gfx = XGraphics.FromPdfPage(page);
                currentYPositionValues = 33;
                currentYPositionLines = 40;

                for (int i = 0; i < list.Count; i++)
                {
                    if (i != 0 && i % 30 == 0)
                    {
                        page = document.AddPage();
                        gfx = XGraphics.FromPdfPage(page);
                        currentYPositionValues = 33;
                        currentYPositionLines = 40;
                    }

                    gfx.DrawString(list[i].ServiceTitle, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(100, currentYPositionValues));
                    gfx.DrawString(list[i].Employee.Name, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(250, currentYPositionValues));
                    gfx.DrawString(list[i].Company.Name, new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(400, currentYPositionValues));
                    gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 90)), new XPoint(50, currentYPositionLines), new XPoint(550, currentYPositionLines));

                }



            }






            var path = $"C:\\Users\\israel.barbosa\\source\\repos\\OrderServiceChallenge\\OrderServiceChallenge\\Data\\temp\\OrderList.pdf";
            document.Save(path);

            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "text/pain", "test.pdf");
        }






    }
}



/* public FileResult PdfGenerator(int id)
{
    var obj = _orderServiceService.FindByIdAsync(id).Result;

    /*StringBuilder sb = new StringBuilder();
    sb.AppendLine($"Nº OS: {obj.NumberOS}");
    sb.AppendLine($"Value: ${obj.Value}");
    sb.AppendLine($"Execution Date: {obj.ExecutionDate}");
    sb.AppendLine();
    sb.AppendLine($"Employee: {obj.Employee.Name}");
    sb.AppendLine($"CPF: {obj.Employee.CPF}");
    sb.AppendLine();
    sb.AppendLine($"Company: {obj.Company.Name}");
    sb.AppendLine($"CNPJ: {obj.Company.CNPJ}");*/



/*  Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

  // New Document
  PdfDocument document = new PdfDocument();

  // New Page
  PdfPage page = document.AddPage();

  XGraphics gfx = XGraphics.FromPdfPage(page);

  XFont font = new XFont("Arial", 20);

  gfx.DrawString($"Nº OS: {obj.NumberOS}", font, XBrushes.Black,
      new XRect(0, 0, page.Width, page.Height),
      XStringFormats.BottomLeft); // could be center right etc

  gfx.DrawString($"Employee: {obj.Employee.Name}", font, XBrushes.DarkCyan,
      new XPoint(100, 20));


  var path = $"C:\\Users\\israel.barbosa\\source\\repos\\OrderServiceChallenge\\OrderServiceChallenge\\Data\\temp\\{obj.ServiceTitle}.pdf";
  document.Save(path);

  byte[] bytes = System.IO.File.ReadAllBytes(path);
  return File(bytes, "text/pain", "test.pdf");*/
//} 
