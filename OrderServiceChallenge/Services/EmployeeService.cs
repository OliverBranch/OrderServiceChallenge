using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServiceChallenge.Models;
using OrderServiceChallenge.Data;

namespace OrderServiceChallenge.Services
{
    public class EmployeeService
    {
        private readonly OrderServiceChallengeContext _context;

        public EmployeeService(OrderServiceChallengeContext context)
        {
            _context = context;
        }

        public List<Employee> FindAllAsync()
        {
            return _context.Employee.ToList();
        }

    }
}
