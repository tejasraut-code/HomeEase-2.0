using HomeEase_2._0_MVC.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace HomeEase_2._0_MVC.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<CategoryModel> Category { get; set; }
        public DbSet<ServiceModel> Services { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<BookingModel> Bookings { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        //for Restrict Deletion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ServiceModel>()
                .HasOne(x => x.Category)
                .WithMany(x => x.Services)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserModel>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<BookingModel>()
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey( x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingModel>()
                .HasOne( x => x.Service)
                .WithMany()
                .HasForeignKey( x=>x.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
