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

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var list = await _orderServiceService.FindAllAsync();
            return View(list);
        }

        // GET: Details
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

        // GET: Create
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.FindAllAsync();
            var companies = await _companyService.FindAllAsync();
            var viewModel = new OrderServiceViewModel { Employees = employees, Companies = companies };
            return View(viewModel);
        }

        // POST: Create
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

        // GET: Edit
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

        // POST: Edit
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

        // GET: Delete
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

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderServiceService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }



        // Returns the error page
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

        //Simple Search of Order Service
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


        //Order Service Generator

        public async Task<FileResult> PdfGenerator(int id)
        {
            var path = $"C:\\Users\\israel.barbosa\\source\\repos\\OrderServiceChallenge\\OrderServiceChallenge\\Data\\temp\\";
            var orderService = await _orderServiceService.FindByIdAsync(id);

            string line = null;
            int line_number = 0;

            using (StreamReader reader = new StreamReader(path + "orderservicetxt.txt"))
            {
                using (StreamWriter writer = new StreamWriter(path + "orderservicehtml.html"))
                {


                    //It Won't write as lines that i want to delete and lines that contain the text

                    while ((line = reader.ReadLine()) != null)
                    {
                        line_number++;

                        //if (line.Contains("href=\"/OrderServices/"))
                        //   continue;
                             if (line_number == 86)
                        {
                            writer.WriteLine($"<a href=\"https://localhost:5001/OrderServices/Details/{orderService.Id}\"> <IMG src = \"./LogoProsperi-300_300.png\" ></a>");
                            continue;
                        }

                        if (line_number == 97)
                        {
                            writer.WriteLine($"<TD class=\"tr0 td2\"><P class=\"p2 ft3\">Nº OS: {orderService.NumberOS}</P></TD>");
                            continue;
                        }

                        if (line_number == 103)
                        {
                            writer.WriteLine($"<TD rowspan = 2 class=\"tr0 td2\"><P class=\"p3 ft7\">Data da Execução: {orderService.ExecutionDate.ToString("dd/MM/yyyy")}</P></TD><TD class=\"tr1 td1\"><P class=\"p1 ft5\">&nbsp;</P></TD>");
                            continue;
                        }


                        if (line_number == 122)
                        {
                            var addLine = $"<TD class=\"tr5 td3\"><P class=\"p1 ft3\">Nome: {orderService.Company.Name}</P></TD><TD class=\"tr5 td4\"><P class=\"p4 ft3\">CNPJ: {Convert.ToInt64(orderService.Company.CNPJ).ToString(@"000\.000\.000\-00")}</P></TD>";
                            writer.WriteLine(addLine);
                            continue;
                        }


                        if (line_number == 142)
                        {
                            writer.WriteLine($"<P class=\"p7 ft3\">Funcionário: {orderService.Employee.Name}</P> <P class=\"p8 ft3\">CPF: {Convert.ToInt64(orderService.Employee.CPF).ToString(@"000\.000\.000\-00")}</P>");
                            continue;
                        }
                        if (line_number == 156)
                        {
                            writer.WriteLine($"<TD class=\"tr5 td5\"><P class=\"p1 ft6\">Valor dos serviços:</P></TD><TD class=\"tr5 td6\"><P class=\"p16 ft13\">${orderService.Value * 0.7}</P></TD>");
                            continue;
                        }
                        if (line_number == 160)
                        {
                            writer.WriteLine($"<TD class=\"tr7 td5\"><P class=\"p1 ft14\">Valor de peças/produtos:</P></TD><TD class=\"tr7 td6\"><P class=\"p16 ft13\">${orderService.Value * 0.3}</P></TD>");
                            continue;
                        }

                        if (line_number == 164)
                        {
                            writer.WriteLine($"<TD class=\"tr8 td5\"><P class=\"p1 ft3\">Valor total:</P></TD><TD class=\"tr8 td6\"><P class=\"p16 ft3\">${orderService.Value}</P></TD>");
                            continue;
                        }

                        writer.WriteLine(line);

    
            	




                    }
                }
            }







            await RenderIronPdfAsync(path + "orderservicehtml.html", path + "OrderService.pdf");

            byte[] bytes = await PdfToBytesAsync(path + "OrderService.pdf");
            return File(bytes, "text/pain", "OrderService.pdf");


        }


        // Text editing and pdf rendering
        public async Task<FileResult> PdfListGenerator()
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

                    //It Won't write as lines that i want to delete and lines that contain the text

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
            await RenderIronPdfAsync(path + "indexout.html", path + "list.pdf");

            byte[] bytes = await PdfToBytesAsync(path + "List.pdf");
            return File(bytes, "text/pain", "testando.pdf");

        }

        //Convert pdf to Byte[]
        public async Task<byte[]> PdfToBytesAsync(string path)
        {
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);
            return bytes;

        }

        //Render Html to Pdf using IronPDF
        public async Task RenderIronPdfAsync(string path, string outputPath)
        {
            var render = new IronPdf.HtmlToPdf();
            var pdf = await render.RenderHTMLFileAsPdfAsync(path);
            pdf.SaveAs(outputPath);

        }

        //Render Html to Pdf using Aspose
        public void RenderAsposePdf(string path, string outputPath)
        {
            HtmlLoadOptions htmloptions = new HtmlLoadOptions();
            Document doc = new Document(path, htmloptions);
            doc.Save(outputPath);
        }

    }
}


