using Microsoft.EntityFrameworkCore;

namespace Stackoverflow_Lite.Configurations;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}