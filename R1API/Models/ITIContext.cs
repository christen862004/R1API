using Microsoft.EntityFrameworkCore;

namespace R1API.Models
{
    public class ITIContext:DbContext
    {
        public DbSet<Department> Departments { get; set; }

        public ITIContext(DbContextOptions<ITIContext> options):base(options)
        {
            
        }
    }
}
