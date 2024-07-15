using Microsoft.EntityFrameworkCore;

namespace Model;

public class TwoFactorAuthAPIDbContext : DbContext
{
    public DbSet<UserData> UserData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(
            "Server=Notebook-Felipe\\SQLEXPRESS; Database=TwoFactorAuthAPI; Trusted_Connection=True; TrustServerCertificate=True;"
            );
}
