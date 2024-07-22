using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TMSMVC.Models;

namespace TMSMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<TMSMVC.Models.Company> Company { get; set; } = default!;
        public DbSet<TMSMVC.Models.Department> Department { get; set; } = default!;
        public DbSet<TMSMVC.Models.TaskItem> TaskItem { get; set; } = default!;
    }
}
