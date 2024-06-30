using Microsoft.EntityFrameworkCore;

namespace WebServer.DatabaseManagement.Auth;

public class ArmpitAuthDbContext : DbContext
{
    public DbSet<AccountInformation> Accounts { get; set; }
    public ArmpitAuthDbContext(DbContextOptions options) : base(options)
    {
        
    }
}
