using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNJGoldValue.Models
{
    public  class GoldValue
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string BuyPrice { get; set; }
        [Required]
        public string SellPrice { get; set; }
        public GoldValue(string name, string buyPrice, string sellPrice)
        {
            Name = name;
            BuyPrice = buyPrice;
            SellPrice = sellPrice;
        }
    }
}
