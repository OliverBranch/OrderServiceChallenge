using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServiceChallenge.Models;
using OrderServiceChallenge.Data;
using OrderServiceChallenge.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace OrderServiceChallenge.Services
{
    public class EmployeeService
    {
        private readonly OrderServiceChallengeContext _context;

        public EmployeeService(OrderServiceChallengeContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> FindAllAsync()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task InsertAsync(Employee obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Employee> FindByIdAsync(int id)
        {
            return await _context.Employee.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Employee.FindAsync(id);
            _context.Employee.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee obj)
        {
            bool hasAny = await _context.Employee.AnyAsync(x => x.Id == obj.Id);
            if(!hasAny)
            {
                throw new NotFoundException("Id not found");
            }
            try
            {
            _context.Update(obj);
            await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);

            }

        }

        public bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }

    }
}
