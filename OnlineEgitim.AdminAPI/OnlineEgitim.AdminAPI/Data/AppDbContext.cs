using Microsoft.EntityFrameworkCore;
using OnlineEgitim.AdminAPI.Models;

namespace OnlineEgitim.AdminAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public DbSet<Course> Courses { get; set; }


        public DbSet<User> Users { get; set; }

        
        public DbSet<Cart> Carts { get; set; }

        
        public DbSet<Order> Orders { get; set; }

        
        public DbSet<OrderItem> OrderItems { get; set; }

      
        public DbSet<PurchasedCourse> PurchasedCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Course)
                .WithMany(c => c.OrderItems)
                .HasForeignKey(oi => oi.CourseId)
                .OnDelete(DeleteBehavior.Restrict); 

            
            modelBuilder.Entity<PurchasedCourse>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.PurchasedCourses)
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            
            modelBuilder.Entity<PurchasedCourse>()
                .HasOne(pc => pc.Course)
                .WithMany(c => c.PurchasedCourses)
                .HasForeignKey(pc => pc.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
