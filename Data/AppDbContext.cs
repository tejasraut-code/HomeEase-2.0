using HomeEase_2._0_MVC.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ServiceModel>()
                .HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
