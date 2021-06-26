using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderServiceChallenge.Data;
using OrderServiceChallenge.Models;
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
            return await _context.Employee.ToListAsync();
        }
    }
}
