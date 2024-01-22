namespace GroupBWebshop.Models
{
    internal class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }

    }

    internal class PopularCategoryList
    { 
        public string Name { get; set; }
        public int PopularCategory { get; set; }
    }
}
