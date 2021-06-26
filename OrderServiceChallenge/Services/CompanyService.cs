using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServiceChallenge.Data;
using OrderServiceChallenge.Models;
using OrderServiceChallenge.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace OrderServiceChallenge.Services
{
    public class CompanyService
    {
        private readonly OrderServiceChallengeContext _context;

        public CompanyService(OrderServiceChallengeContext context)
        {
            _context = context;
        }
        public async Task<List<Company>> FindAllAsync()
        {
            return await _context.Company.ToListAsync();
        }

        public async void InsertAsync(Company obj)
        {
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Company> FindByIdAsync(int id)
        {
            return await _context.Company.FirstOrDefaultAsync(obj => obj.Id == id);
        }

        public async Task RemoveAsync(int id)
        {
            var obj = await _context.Company.FindAsync(id);
            _context.Company.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Company obj)
        {
            bool hasAny = await _context.Company.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
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

    }
}
