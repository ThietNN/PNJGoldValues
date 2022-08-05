using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using PNJGoldValue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNJGoldValue.Services
{
    public class DbHelper
    {
        private AppDbContext dbContext;
        int i = 0;
        private DbContextOptions<AppDbContext> getAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(AppSettings.ConnectionString);
            return optionBuilder.Options;
        }
        public List<GoldValue> GetGoldValues()
        {
            using (dbContext = new AppDbContext(getAllOptions()))
            {
                var goldValues = dbContext.goldValues.FromSqlRaw("SELECT * FROM goldValues WHERE [id] > (SELECT MAX([id]) - {0} FROM goldValues);", i).ToList();
                i = 0;
                if (goldValues != null)
                    return goldValues;
                else
                    return new List<GoldValue>();
            }
        } 
        public void addGoldValues(String zone)
        {
            using (dbContext = new AppDbContext(getAllOptions()))
            {
                List<GoldValue> goldValues = getData("hn");
                foreach (GoldValue goldValue in goldValues)
                {
                    dbContext.goldValues.Add(goldValue);
                    i++;
                }
                dbContext.SaveChanges();       
            }
        }
        public List<GoldValue> getData(string zone)
        {
            var html = new HtmlWeb();
            var hcmcUrl = "https://www.pnj.com.vn/blog/gia-vang/?zone=00";
            var hanoiUrl = "https://www.pnj.com.vn/blog/gia-vang/?zone=11";
            var canthoUrl = "https://www.pnj.com.vn/blog/gia-vang/?zone=07";
            var danangUrl = "https://www.pnj.com.vn/blog/gia-vang/?zone=13";
            var taynguyenUrl = "https://www.pnj.com.vn/blog/gia-vang/?zone=14";
            var dongnamboUrl = "https://www.pnj.com.vn/blog/gia-vang/?zone=21";
            string url;
            switch (zone)
            {
                case "hcmc":
                    url = hcmcUrl;
                    break;
                case "hn":
                    url = hanoiUrl;
                    break;
                case "ct":
                    url = canthoUrl;
                    break;
                case "dn":
                    url = danangUrl;
                    break;
                case "tn":
                    url = taynguyenUrl;
                    break;
                case "dnb":
                    url = dongnamboUrl;
                    break;
                default:
                    url = hanoiUrl;
                    break;
            }
            var document = html.Load(url);
            List<GoldValue> goldValues = new List<GoldValue>();
            var container = document.DocumentNode.QuerySelector("#content-price");
            var goldValueList = container.QuerySelectorAll("tr");

            foreach (var item in goldValueList)
            {
                var content = item.InnerText.ToString();
                string stringSeparators = "\r\n";
                string[] list = content.Replace(stringSeparators, "-").Split('-');
                var newList = list.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();

                var name = newList[0].Trim();
                var buyPrice = newList[1].Trim();
                var sellPrice = newList[2].Trim();
                var datetime = DateTime.Now;
                GoldValue goldValue = new GoldValue(name, buyPrice, sellPrice, datetime);
                goldValues.Add(goldValue);
            }
            return goldValues;
        }

    }
}
