using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBWebshop.Models
{
    internal class Product
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        
        public bool DisplayProduct { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public string Size { get; set; }
        public string Material { get; set; }
        public int SupplierId { get; set; }
        public float Price { get; set; }
        public string Info { get; set; }
        public int StockStatus { get; set; }
    }
}
