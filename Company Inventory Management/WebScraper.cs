using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace CompanyInventory; // Ez KELL, hogy a Program.cs lássa!

public class WebScraper // Ez a CLASS hiányzott a kódodból!
{
    public List<Laptop> GetLaptopsFromBazos()
    {
        var list = new List<Laptop>();
        var web = new HtmlWeb();

        // User-Agent hozzáadása, hogy ne nézzenek robotnak
        web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";

        try
        {
            var doc = web.Load("https://pc.bazos.sk/notebook/");
            // Frissített XPath, ami a Bazoš jelenlegi kódjához jó
            var nodes = doc.DocumentNode.SelectNodes("//h2[@class='nadpis']//a");

            if (nodes == null)
            {
                Console.WriteLine("Hiba: Nem találtam hirdetéseket. Lehet, hogy változott a Bazoš kódja.");
            }
            else
            {
                foreach (var node in nodes)
                {
                    string tisztaNev = HtmlEntity.DeEntitize(node.InnerText).Trim();

                    // Megkeressük az árat is (a Bazošon ez általában egy 'cena' osztályú div-ben van a hirdetés mellett)
                    // Az XPath-ot kicsit trükkösen kell megadni, hogy a névhez tartozó árat találjuk meg
                    var arNode = node.SelectSingleNode("./ancestor::div[@class='listainzerat']//div[@class='cena']");
                    string arSzoveg = arNode != null ? HtmlEntity.DeEntitize(arNode.InnerText).Trim() : "Nincs ár";

                    list.Add(new Laptop
                    {
                        Brand = "Bazos Import",
                        Model = tisztaNev,
                        Price = arSzoveg, // Most már az igazi árat mentjük!
                        SerialNumber = "WEB-" + Guid.NewGuid().ToString().Substring(0, 5)
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hiba történt a letöltés közben: " + ex.Message);
        }

        return list;
    }
}