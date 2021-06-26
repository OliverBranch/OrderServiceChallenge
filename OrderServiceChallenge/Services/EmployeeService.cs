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

        public void Insert(Employee obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Employee FindById(int id)
        {
            return _context.Employee.FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Employee.Find(id);
            _context.Employee.Remove(obj);
            _context.SaveChanges();
        }

    }
}
