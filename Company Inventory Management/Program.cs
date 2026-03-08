using CompanyInventory;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;



Console.WriteLine("--- Bazoš Adatgyűjtő Indítása ---");

var scraper = new WebScraper();
var talaltAdatok = scraper.GetLaptopsFromBazos();

// Itt ellenőrizzük, hogy a lista nem üres-e
if (talaltAdatok != null && talaltAdatok.Any())
{
    Console.WriteLine($"\nSzuper! Találtam {talaltAdatok.Count} hirdetést.\n");

    using (var db = new InventoryContext())
    {
        db.Database.EnsureCreated();

        int ujGepekSzama = 0;

        foreach (var laptop in talaltAdatok)
        {
            // ELLENŐRZÉS: Benne van-e már ez a Modell az SQL-ben?
            bool marLetezik = db.Laptops.Any(l => l.Model == laptop.Model);

            if (!marLetezik)
            {
                db.Laptops.Add(laptop);
                Console.WriteLine($"[ÚJ] Mentés: {laptop.Model}");
                ujGepekSzama++;
            }
            else
            {
                Console.WriteLine($"[SKIPPED] Már az adatbázisban van: {laptop.Model}");
            }
        }


        if (ujGepekSzama > 0)
        {
            // ELŐSZÖR MENTÜNK (beküldjük az adatokat az SQL-be)
            db.SaveChanges();
            Console.WriteLine($"\nSiker! {ujGepekSzama} új gépet adtam hozzá.");
        }
        else
        {
            Console.WriteLine("\nNem volt új hirdetés, az adatbázis naprakész.");
        }

        // MAJD ELLENŐRIZZÜK (most már az SQL a friss adatot adja vissza)
        var osszesAzSqlben = db.Laptops.Count();
        Console.WriteLine($"\nELLENŐRZÉS: Jelenleg összesen {osszesAzSqlben} laptop van az SQL táblában.");
    }
}
else
{
    Console.WriteLine("Sajnos nem találtam adatot. Ellenőrizd az internetet vagy az XPath-ot!");
}

Console.WriteLine("\nNyomj Entert a bezáráshoz...");
Console.ReadLine();