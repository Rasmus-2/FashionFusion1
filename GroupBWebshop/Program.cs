using GroupBWebshop.Models;

namespace GroupBWebshop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*List<string> clothes = new List<string>() { "Pants", "Sweater", "Socks", "T-shirt"};
            Window window = new Window("Kategori", 2, 10, clothes);
            window.Draw();*/

            using (var myDb = new MyDbContext())
            {
                Models.Category category1 = new Models.Category { Name = "Women" };
                Models.Category category2 = new Models.Category { Name = "Men" };
                Models.Category category3 = new Models.Category { Name = "Kids" };
                Models.Category category4 = new Models.Category { Name = "Non-binary" };

                myDb.AddRange(category1, category2, category3, category4);

                Models.Type type1 = new Models.Type { Name = "Pants", Categories = new List<Models.Category> { category1, category2, category3, category4 } };
                Models.Type type2 = new Models.Type { Name = "Sweaters", Categories = new List<Models.Category> { category1, category2, category3, category4 } };
                Models.Type type3 = new Models.Type { Name = "T-shirts", Categories = new List<Models.Category> { category1, category2, category3, category4 } };
                Models.Type type4 = new Models.Type { Name = "Underwear", Categories = new List<Models.Category> { category1, category2, category3, category4 } };
                Models.Type type5 = new Models.Type { Name = "Skirt", Categories = new List<Models.Category> { category1, category3, category4 } };
                Models.Type type6 = new Models.Type { Name = "Dress", Categories = new List<Models.Category> { category1, category3, category4 } };
                Models.Type type7 = new Models.Type { Name = "Formal wear", Categories = new List<Models.Category> { category1, category2, category4 } };
                Models.Type type8 = new Models.Type { Name = "Shoes", Categories = new List<Models.Category> { category1, category2, category3, category4 } };
                Models.Type type9 = new Models.Type { Name = "Socks", Categories = new List<Models.Category> { category1, category2, category3, category4 } };
                Models.Type type10 = new Models.Type { Name = "Ties", Categories = new List<Models.Category> { category2, category4 } };

                myDb.AddRange(type1, type2, type3, type4, type5, type6, type7, type8, type9, type10);

                Models.Supplier supplier1 = new Models.Supplier { Name = "KidsAtWorkInc" };

                myDb.Add(supplier1);

                Models.Product product1 = new Models.Product
                { CategoryId = 1, TypeId = 2, Name = "Red christmas sweater", Material = "Cashmere", Size = "XS", Info = "Beautiful sweater for christmas, only mild itching reported.", Price = 890, StockStatus = 22, SupplierId = 1, DisplayProduct = false };

                myDb.Add(product1);

                myDb.SaveChanges();
            }
        }
    }
}
