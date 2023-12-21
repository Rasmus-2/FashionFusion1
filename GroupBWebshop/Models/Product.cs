using System.ComponentModel.DataAnnotations;

namespace GroupBWebshop.Models
{
    internal class Product
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public bool DisplayProduct { get; set; }

        public string Size { get; set; }
        public string Material { get; set; }
        public int SupplierId { get; set; }
        public float Price { get; set; }
        [MaxLength(128)]
        public string Info { get; set; }
        public int? StockStatus { get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
