using Employee_APP.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_APP.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
    }
}
