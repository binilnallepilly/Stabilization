using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCPServiceRepository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DCPServiceRepository
{
    public class DenormRepoDbContext : DbContext
    {
        public DenormRepoDbContext(DbContextOptions<DenormRepoDbContext> contextOptions)
            :base(contextOptions)
        {
        }
        public DbSet<ConfigCallLog> CallLog { get; set; }
        public DbSet<OrderCode> OrderCode { get; set; }

    }
}
