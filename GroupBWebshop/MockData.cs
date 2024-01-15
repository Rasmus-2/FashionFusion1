using GroupBWebshop.Models;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBWebshop
{
    internal class MockData
    {
        public static void FakeFunction()
        {
            using (var myDb = new MyDbContext())
            {
                Models.Category forWomen = new Models.Category { Name = "Women" };
                Models.Category forMen = new Models.Category { Name = "Men" };
                Models.Category forKids = new Models.Category { Name = "Kids" };
                Models.Category forNonbinary = new Models.Category { Name = "Non-binary" };
                Models.Category pants = new Models.Category { Name = "Pants" };
                Models.Category sweaters = new Models.Category { Name = "Sweater" };
                Models.Category tshirt = new Models.Category { Name = "T-shirt" };
                Models.Category shoes = new Models.Category { Name = "Shoes" };


                myDb.AddRange(forWomen, forMen, forKids, forNonbinary, pants, sweaters, tshirt, shoes);

                Models.Supplier supplier1 = new Models.Supplier { Name = "KidsAtWorkInc" };
                Models.Supplier supplier2 = new Models.Supplier { Name = "Comfy Fabric Inc" };
                Models.Supplier supplier3 = new Models.Supplier { Name = "Recycle Textiles AB" };

                myDb.AddRange(supplier1, supplier2, supplier3);

                var x = new List<Category> { forWomen, forNonbinary, sweaters };

            Models.Product product1 = new Models.Product
                { Categories = new List<Category> { forWomen, forNonbinary, sweaters }, Name = "Red christmas sweater", Material = Models.MyEnums.Material.Cashmere.ToString(), Size = Models.MyEnums.EnumSize.XS.ToString(), Info = "Beautiful sweater for christmas, only mild itching reported.", Price = 890, StockStatus = 22, SupplierId = 1, DisplayProduct = false };
                Models.Product product2 = new Models.Product
                { Categories = new List<Category> { forKids, forNonbinary, tshirt }, Name = "Urban disco T-Shirt", Material = Models.MyEnums.Material.Cotton.ToString(), Size = Models.MyEnums.EnumSize.M.ToString(), Info = "Latest trend on tiktok, comfortable fabric.", Price = 189, StockStatus = 13, SupplierId = 2, DisplayProduct = true };
                Models.Product product3 = new Models.Product
                { Categories = new List<Category> { forWomen, forNonbinary, shoes }, Name = "Boots for good", Material = Models.MyEnums.Material.Leather.ToString(), Size = Models.MyEnums.EnumSize.M.ToString(), Info = "Comfortable footwear, good for all seasons.", Price = 1499, StockStatus = 18, SupplierId = 3, DisplayProduct = false };
                Models.Product product4 = new Models.Product
                { Categories = new List<Category> { forMen, pants }, Name = "Beige cargo pants", Material = Models.MyEnums.Material.Cotton.ToString(), Size = Models.MyEnums.EnumSize.XL.ToString(), Info = "Functional cargo pants with over 19 pockets.", Price = 599, StockStatus = 7, SupplierId = 1, DisplayProduct = false };
                Models.Product product5 = new Models.Product
                { Categories = new List<Category> { forMen, forWomen, forNonbinary, pants }, Name = "Marine blue chinos", Material = Models.MyEnums.Material.Polyester.ToString(), Size = Models.MyEnums.EnumSize.L.ToString(), Info = "Regular fit, for both formal and casual occasions.", Price = 250, StockStatus = 25, SupplierId = 3, DisplayProduct = true };
                Models.Product product6 = new Models.Product
                { Categories = new List<Category> { forKids, forNonbinary, sweaters }, Name = "Neon fleece sweater", Material = Models.MyEnums.Material.Polyester.ToString(), Size = Models.MyEnums.EnumSize.XL.ToString(), Info = "Perfect for snuggling, made from recycled polyester.", Price = 999, StockStatus = 8, SupplierId = 1, DisplayProduct = false };
                Models.Product product7 = new Models.Product
                { Categories = new List<Category> { forKids, forWomen, tshirt }, Name = "The flash, kid t-shirt", Material = Models.MyEnums.Material.Wool.ToString(), Size = Models.MyEnums.EnumSize.XS.ToString(), Info = "For kids in desperate times.", Price = 49, StockStatus = 1, SupplierId = 1, DisplayProduct = false };
                Models.Product product8 = new Models.Product
                { Categories = new List<Category> { forKids, forNonbinary, forWomen, forMen, tshirt }, Name = "Disney Characters tshirt", Material = Models.MyEnums.Material.Cotton.ToString(), Size = Models.MyEnums.EnumSize.M.ToString(), Info = "All your favourite disney characters in one comfy t-shirt.", Price = 149, StockStatus = 60, SupplierId = 3, DisplayProduct = false };
                Models.Product product9 = new Models.Product
                { Categories = new List<Category> { forMen, forNonbinary, shoes }, Name = "Sneakers for every season", Material = Models.MyEnums.Material.Wool.ToString(), Size = Models.MyEnums.EnumSize.L.ToString(), Info = "Great for all seasons of the year.", Price = 1899, StockStatus = 0, SupplierId = 2, DisplayProduct = true };
                Models.Product product10 = new Models.Product
                { Categories = new List<Category> { forMen, forWomen, sweaters }, Name = "Lovely couple sweaters", Material = Models.MyEnums.Material.Wool.ToString(), Size = Models.MyEnums.EnumSize.S.ToString(), Info = "Good for couples.", Price = 299, StockStatus = 10, SupplierId = 1, DisplayProduct = false };
                Models.Product product11 = new Models.Product
                { Categories = new List<Category> { forKids, pants }, Name = "Red sweatpants", Material = Models.MyEnums.Material.Wool.ToString(), Size = Models.MyEnums.EnumSize.XS.ToString(), Info = "For days you don't want to go outside.", Price = 99, StockStatus = 78, SupplierId = 1, DisplayProduct = false };


                myDb.AddRange(product1, product2, product3, product4, product5, product6, product7, product8, product9, product10, product11);

                myDb.AddRange(
                    new Country
                    {
                        Name = "Sweden"
                    },
                    new Country
                    {
                        Name = "Great Britain"
                    },
                    new Country
                    {
                        Name = "Denmark"
                    }
                    );
                myDb.AddRange(
                    new Customer
                    {
                        Name = "Björn Hävelid",
                        Email = "bjorn@gmail.com",
                        Phone = "+46736132313",
                        BirthDate = new DateTime(1950, 5, 1),
                        StreetName = "Äppelstigen 155",
                        PostalCode = "633 48",
                        City = "Eskilstuna",
                        CountryId = 1
                    },
                    new Customer
                    {
                        Name = "Jenny McDonalds",
                        Email = "jenny@hotmail.com",
                        Phone = "+44433335100",
                        BirthDate = new DateTime(1983, 1, 23),
                        StreetName = "Colombia Road 5",
                        PostalCode = "8987 AA",
                        City = "London",
                        CountryId = 2
                    },
                    new Customer
                    {
                        Name = "Jens Madsen",
                        Email = "madsen@gmail.com",
                        Phone = "+45192788651",
                        BirthDate = new DateTime(1971, 9, 8),
                        StreetName = "Pølsegatan 11",
                        PostalCode = "5433",
                        City = "Copenhagen",
                        CountryId = 3

                    },
                    new Customer
                    {
                        Name = "Huda Karim",
                        Email = "huda1@gmail.com",
                        Phone = "+46705142414",
                        BirthDate = new DateTime(2001, 11, 5),
                        StreetName = "Flöjtvägen 60",
                        PostalCode = "151 32",
                        City = "Södertälje",
                        CountryId = 1
                    }

                    );
                myDb.SaveChanges();
            }
        }
    }
}
