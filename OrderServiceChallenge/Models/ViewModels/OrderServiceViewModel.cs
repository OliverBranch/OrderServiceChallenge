using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderServiceChallenge.Models.ViewModels
{
    public class OrderServiceViewModel
    {
        public OrderService OrderService { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<Company> Companies { get; set; }
    }
}
