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
            AddProduct();
            //Produkt: Ändra produktnamn, infotext, pris, produktkategori, leverantör, lagersaldo

            //Ta bort Produkter

            //CRUD Kategori

            //CRUD kunduppgifter



        }

        public static void AddProduct()
        {
            Console.WriteLine("Name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Display product?");
            string display = Console.ReadLine();
            Console.Write("Size: | ");
            foreach (int i in Enum.GetValues(typeof(MyEnums.EnumSize)))
            {
                Console.Write(i + ". " + Enum.GetName(typeof(MyEnums.EnumSize), i) + " | ");
            }
            Console.WriteLine();
            int size = int.Parse(Console.ReadLine());

            Console.Write("Material: | ");
            foreach (int i in Enum.GetValues(typeof(MyEnums.Material)))
            {
                Console.Write(i + ". " + Enum.GetName(typeof(MyEnums.Material), i) + " | ");
            }
            Console.WriteLine();
            int material = int.Parse(Console.ReadLine());

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
