using Microsoft.EntityFrameworkCore;

namespace CompanyInventory; // Ez segít, hogy a fájlok "lássák" egymást
public class Laptop
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string SerialNumber { get; set; }
    public string? Price { get; set; } // Új mező a webről!
}