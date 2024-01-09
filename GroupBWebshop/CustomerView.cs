using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBWebshop.Migrations;
using GroupBWebshop.Models;
using Microsoft.Identity.Client;
using static System.Net.Mime.MediaTypeNames;

namespace GroupBWebshop
{
    internal class CustomerView
    {
        public static void View(int customerId)
        {
            List<string> welcomeText = new List<string>() { "Welcome to Fashion Fushion", "The fashion for sustainable generation" };
            Window text = new Window("", 5, 1, welcomeText);
            text.Draw();

            //List<string> productDisplay = new List<string>() ;


            List<string> clothes = new List<string>() { "Pants", "Sweater", "Socks", "T-shirt" };
            Window window = new Window("Kategori", 2, 10, clothes);
            window.Draw();

            using (var myDb = new MyDbContext())
            {
                var productDisplay =
                    from product in myDb.Products
                    where product.DisplayProduct
                    select product.Name;
                //List<string> productsDisplay = new List<string>();
                Window display = new Window("", 2, 5, productDisplay.ToList());
                display.Draw();

                List<string> cart = new List<string>() { "empty" };
                Window box = new Window("Cart", 55, 1, cart);
                box.Draw();

                Console.WriteLine("What would you like to do today?");
                Console.WriteLine("1. View products");
                Console.WriteLine("2. View cart");
                Console.WriteLine("3. View account information");
                Console.WriteLine("4. View history");
                Console.WriteLine("5. Search");

                var key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case '1':
                        bool quit = false;
                        while (quit == false)
                        {
                            Console.Clear();
                            ViewAllProducts();
                            var inputId = int.Parse(Console.ReadLine());
                            ViewProduct(inputId);

                            Console.WriteLine("1. Add to cart");
                            Console.WriteLine("2. View another product");
                            Console.WriteLine("3. Home");
                            var getout = Console.ReadKey();
                            if (getout.KeyChar == '3')
                            {
                                quit = true;
                                Console.Clear();
                                View(customerId);

                            }
                            else if (getout.KeyChar == '2')
                            {
                                ViewAllProducts();
                            }
                            else if (getout.KeyChar == '1')
                            {
                                Console.WriteLine("Enter quantity");
                                int quantity = int.Parse(Console.ReadLine());

                                //var existingOrder = (from o in myDb.Orders
                                //                     where o.CustomerId == customerId && !o.Completed
                                //                     select o).SingleOrDefault();

                                var existingOrder = GetOrderId(customerId);

                                if (existingOrder == null)
                                {
                                    myDb.Add(new Order { CustomerId = customerId, Completed = false });
                                }
                                var getOrderId = GetOrderId(customerId);

                                OrderDetails orderDetails = new OrderDetails() { OrderId = getOrderId.Id, ProductId = inputId, Quantity = quantity };
                                myDb.Add(orderDetails);

                                /*   var getOrder = (from o in myDb.Orders
                                                   where o.CustomerId == customerId && !o.Completed
                                                   select o.Id).SingleOrDefault();

                                   OrderDetails orderDetails = new OrderDetails() { OrderId = getOrder, ProductId = inputId, Quantity = quantity };
                                   myDb.Add(orderDetails);
                               }
                               else
                               {
                                   OrderDetails orderDetails = new OrderDetails() { OrderId = test, ProductId = inputId, Quantity = quantity };
                                   myDb.Add(orderDetails);
                               }*/



                                myDb.SaveChanges();
                            }
                        }

                        break;

                    case '2':
                        Console.WriteLine("");
                        break;
                    case '3':
                        break;
                    case '4':
                        break;
                    case '5':
                        break;
                }


            }


        }

        public static Order GetOrderId(int customerId)
        {
            using (var myDb = new MyDbContext())
            {
                var existingOrder = (from o in myDb.Orders
                                     where o.CustomerId == customerId && !o.Completed
                                     select o).SingleOrDefault();
                return existingOrder;
            }
        }

        public static void ViewProduct(int id)
        {
            using (var myDb = new MyDbContext())
            {
                var productChoosen = (from product in myDb.Products
                                      where product.Id == id
                                      select product).SingleOrDefault();
                Console.WriteLine(productChoosen.Name + "\t" + productChoosen.Info + "\t" + productChoosen.Price + "kr\t" + productChoosen.Size);
            }
        }
        public static void ViewAllProducts()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var p in myDb.Products)
                {
                    Console.WriteLine(p.Id + "" + p.Name + " " + p.Price);
                }

                Console.WriteLine("View product's info");
                Console.WriteLine("Enter product's Id: ");
            }
        }

        public static void AddToCart(int id)
        {
            using (var myDb = new MyDbContext())
            {

                var productChoosen = (from product in myDb.Products
                                      where product.Id == id
                                      select product).SingleOrDefault();
                myDb.Add(new OrderDetails { ProductId = productChoosen.Id });
            }
            Console.WriteLine("Order placed!");
        }
    }
}
