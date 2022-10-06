using Microsoft.EntityFrameworkCore;
using Update_DB_Daily.Models;

namespace Update_DB_Daily.Data
{
    public class ProjectDBContext : DbContext
    {
        public ProjectDBContext(DbContextOptions<ProjectDBContext> options): base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>().ToTable("Projects");   
        }
    } 
}
