using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataAccessLayer
{
    public class BRMSContext : DbContext
    {
        public BRMSContext(DbContextOptions<BRMSContext> options) : base(options) { }

        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Alert> Alerts { get; set; }
    }
}