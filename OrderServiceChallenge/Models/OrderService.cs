using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderServiceChallenge.Models
{
    public class OrderService
    {
        public int Id { get; set; }
        [Display(Name = "Nº OS")]
        [Required(ErrorMessage = "{0} Required")]
        public string NumberOS { get; set; }
        [Display(Name = "Service Title")]
        [Required(ErrorMessage = "{0} Required")]
        public string ServiceTitle { get; set; }
        [Required(ErrorMessage = "{0} Required")]
        [DataType(DataType.Currency)]
        public double Value { get; set; }
        [Display(Name = "Execution Date")]
        [Required(ErrorMessage = "{0} Required")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime ExecutionDate { get; set; }
        public Employee Employee { get; set; }
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public Company Company { get; set; }
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public OrderService()
        {
            
        }

        public OrderService(int id, string numberOS, string serviceTitle, double value, DateTime executionDate, Employee employee, Company company)
        {
            Id = id;
            NumberOS = numberOS;
            ServiceTitle = serviceTitle;
            Value = value;
            ExecutionDate = executionDate;
            Employee = employee;
            Company = company;
        }
    }
}
