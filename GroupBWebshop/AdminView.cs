using GroupBWebshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBWebshop
{
    internal class AdminView
    {
        public static void View()
        {
            //Lägga till produkter
            //AddProduct();
            //Produkt: Ändra produktnamn, infotext, pris, produktkategori, leverantör, lagersaldo
            var name = GetProduct(1);
            Console.WriteLine(name.Categories);
            
            //Ta bort Produkter

            //CRUD Kategori

            //CRUD kunduppgifter



        }

        public static void AddProduct()
        {
            using (var myDb = new MyDbContext())
            {
                
                Console.WriteLine("Name: ");
                string name = Console.ReadLine();
                Console.WriteLine("Display product? yes/no");
                string display = Console.ReadLine();

                bool inputDisplay = false;
                if (display == "yes")
                { 
                    inputDisplay = true;
                }

                Console.Write("Size: | ");
                foreach (int i in Enum.GetValues(typeof(MyEnums.EnumSize)))
                {
                    Console.Write(Enum.GetName(typeof(MyEnums.EnumSize), i) + " | ");
                }
                Console.WriteLine();
                string size = Console.ReadLine().ToUpper();

                Console.Write("Material: | ");
                foreach (int i in Enum.GetValues(typeof(MyEnums.Material)))
                {
                    Console.Write(i + ". " + Enum.GetName(typeof(MyEnums.Material), i) + " | ");
                }
                Console.WriteLine();
                int number1;
                string materialChoice = "";

                if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out number1))
                {
                    MyEnums.Material selection1 = (MyEnums.Material)number1;


                    switch (selection1)
                    {
                        case MyEnums.Material.Cotton:
                            materialChoice = MyEnums.Material.Cotton.ToString();
                            break;

                        case MyEnums.Material.Polyester:
                            materialChoice = MyEnums.Material.Polyester.ToString();
                            break;

                        case MyEnums.Material.Wool:
                            materialChoice = MyEnums.Material.Wool.ToString();
                            break;

                        case MyEnums.Material.Cashmere:
                            materialChoice = MyEnums.Material.Cashmere.ToString();
                            break;

                        case MyEnums.Material.Leather:
                            materialChoice = MyEnums.Material.Leather.ToString();
                            break;

                        default:
                            Console.Clear();
                            View();
                            break;
                    }

                }



                ShowSuppliers();
                int supplierId = int.Parse(Console.ReadLine());

                Console.WriteLine("Price: ");
                float price = float.Parse(Console.ReadLine());

                Console.WriteLine("Info text: ");
                string info = Console.ReadLine();

                Console.WriteLine("Stock status: ");
                int stockStatus = int.Parse(Console.ReadLine());

                List<Category> categories = new List<Category>();

                ShowCategories();
                Console.WriteLine("How many categories? ");
                int firstCat = int.Parse(Console.ReadLine());


                List<Category> newCategory = CategoryInput(firstCat);
                foreach (Category category in newCategory)
                {
                    Console.WriteLine("[" + category.Id + "] " + category.Name);
                }

                myDb.Add(new Product 
                { 
                    Name = name,
                    DisplayProduct = inputDisplay,
                    Size = size,
                    Material = materialChoice,
                    SupplierId = supplierId,
                    Price = price,
                    Info = info,
                    StockStatus = stockStatus,
                    Categories = categories

                });

                myDb.SaveChanges();
            }
            
        }
        public static Product GetProduct(int id)
        {
            using (var myDb = new MyDbContext())
            {
                var getProduct = (from g in myDb.Products
                                  where g.Id == id
                                  select g).SingleOrDefault();
                return getProduct;
            }
        }
        public static List<Category> CategoryInput(int numCategory)
        {
            using (var myDb = new MyDbContext())
            {
                List<Category> categories = new List<Category>();

                for (int i = 0; i < numCategory; i++)
                {
                    Console.WriteLine("Enter category Id: " + i);

                    int catText = int.Parse(Console.ReadLine());

                    Category category = GetCategory(catText);

                    categories.Add(category);
                }

                return categories;

            }
        }

        public static Category GetCategory(int id)
        {
            using (var myDb = new MyDbContext())
            {
                var getCat = (from g in myDb.Categories
                             where g.Id == id
                             select g).SingleOrDefault();

                return getCat;
            }
        }

        public static void ShowCategories()
        {
            using (var myDb = new MyDbContext())
            {
                var categories = from c in myDb.Categories
                                select c;

                foreach (Category category in categories)
                {
                    Console.WriteLine("[" + category.Id + "] " + category.Name);
                }
            }
        }

        public static void ShowSuppliers()
        {
            using (var myDb = new MyDbContext())
            {
                var suppliers = from s in myDb.Suppliers
                                select s;

                foreach( Supplier supplier in  suppliers )
                {
                    Console.WriteLine("[" + supplier.Id + "] " + supplier.Name);
                }
            }            
        }
    }
}
