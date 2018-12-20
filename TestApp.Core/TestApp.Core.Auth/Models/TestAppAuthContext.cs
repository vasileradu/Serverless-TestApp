using Microsoft.EntityFrameworkCore;

namespace TestApp.Core.Auth.Models
{
    public partial class TestAppAuthContext : DbContext
    {
        public TestAppAuthContext()
        {
        }

        public TestAppAuthContext(DbContextOptions<TestAppAuthContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(256);
            });
        }
    }
}
