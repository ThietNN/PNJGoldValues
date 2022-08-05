using Microsoft.EntityFrameworkCore;
using PNJGoldValue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNJGoldValue.Services
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<GoldValue> goldValues {get; set; }
    }
}
