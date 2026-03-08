using Microsoft.EntityFrameworkCore;

namespace CompanyInventory;

public class InventoryContext : DbContext
{
    public DbSet<Laptop> Laptops { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=CompanyInventoryDb;Trusted_Connection=True;");
    }
}