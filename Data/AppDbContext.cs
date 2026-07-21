using HomeEase_2._0_MVC.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CategoryModel> Category { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
