using Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Test.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Address> Address { get; set; }
    }
}
