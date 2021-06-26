using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderServiceChallenge.Models;

namespace OrderServiceChallenge.Data
{
    public class OrderServiceChallengeContext : DbContext
    {
        public OrderServiceChallengeContext (DbContextOptions<OrderServiceChallengeContext> options)
            : base(options)
        {
        }

        public DbSet<OrderServiceChallenge.Models.Employee> Employee { get; set; }
    }
}
