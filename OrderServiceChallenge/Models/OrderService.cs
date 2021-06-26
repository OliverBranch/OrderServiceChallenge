using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceChallenge.Models
{
    public class OrderService
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public string NumberOS { get; set; }
        public string ServiceTitle { get; set; }
        public double Value { get; set; }
        public DateTime ExecutionDate { get; set; }
        public Employee Employee { get; set; }
        public Company Company { get; set; }

        public OrderService()
        {
            
        }

        public OrderService(int id, int companyId, int employeeId,string numberOS, string serviceTitle, double value, DateTime executionDate, Employee employee, Company company)
        {
            Id = id;
            CompanyId = companyId;
            EmployeeId = employeeId;
            NumberOS = numberOS;
            ServiceTitle = serviceTitle;
            Value = value;
            ExecutionDate = executionDate;
            Employee = employee;
            Company = company;
        }
    }
}
