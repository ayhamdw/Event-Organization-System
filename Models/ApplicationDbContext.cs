using Microsoft.EntityFrameworkCore;

namespace Event_Organization_System.model;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    public DbSet<User> Users { get; set; }
}