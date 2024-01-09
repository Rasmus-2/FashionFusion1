using System;
using System.Collections.Generic;
using System.Diagnostics;
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

                var cart = CartView(customerId);

                Window box = new Window("Cart", 55, 1, cart);
                box.Draw();

                Console.WriteLine("What would you like to do today?");
                Console.WriteLine("1. View products");
                Console.WriteLine("2. View cart");
                Console.WriteLine("3. View account information");
                Console.WriteLine("4. View history");
                Console.WriteLine("5. Search");
                Console.WriteLine("6. Log out");

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

                                var existingOrder = GetOrderId(customerId);

                                if (existingOrder == null)
                                {
                                    myDb.Add(new Order { CustomerId = customerId, Completed = false });
                                    myDb.SaveChanges();
                                }
                                var getOrderId = GetOrderId(customerId);

                                OrderDetails orderDetails = new OrderDetails() { OrderId = getOrderId.Id, ProductId = inputId, Quantity = quantity };
                                myDb.Add(orderDetails);

                                myDb.SaveChanges();
                            }
                        }

                        break;

                    case '2':
                        Console.Clear();
                        var finalCart = CartView(customerId);
                        var total = CartPrice(customerId);
                        finalCart.Add("");
                        finalCart.Add("Total sum: " + total + " SEK");
                        Window cartBox = new Window("Your cart", 1, 1, finalCart);
                        cartBox.Draw();
                        Console.WriteLine("1. Continue to payment");
                        Console.WriteLine("2. Continue shopping");
                        var exitCart = Console.ReadKey();
                        if(exitCart.KeyChar == '1')
                        {
                            Console.WriteLine("payment methods");
                            foreach (var i in Enum.GetValues(typeof(MyEnums.PaymentMethod)))
                            {
                                Console.WriteLine(Enum.GetName(typeof(MyEnums.PaymentMethod), i).Replace('_', ' '));
                            }

                        }
                        else if(exitCart.KeyChar == '2')
                        {
                            Console.WriteLine("Shopping!");
                        }
                        else
                        {
                            Console.WriteLine("Try again loser");
                        }
                        break;

                    case '3':
                        break;
                    case '4':
                        break;
                    case '5':
                        break;
                    case '6':
                        break;
                }


            }


        }

        public static double CartPrice(int customerId)
        {
            double total = 0;

            using (var myDb = new MyDbContext())
            {
                var orderId = GetOrderId(customerId);

                var productsInCart = (from o in myDb.OrderDetails
                                      where o.OrderId == orderId.Id
                                      select o.ProductId).ToList();

                foreach (var product in productsInCart)
                {
                    var quantityInCart = (from o in myDb.OrderDetails
                                          where o.ProductId == product
                                          && o.OrderId == orderId.Id
                                          select o.Quantity).FirstOrDefault();

                    var productInfo = (from p in myDb.Products
                                       where p.Id == product
                                       select p).SingleOrDefault();

                    total += quantityInCart * productInfo.Price;
                }
            }
            return total;
        }

        public static List<string> CartView(int customerId)
        {
            List<string> cart = new List<string>() { };

            using (var myDb = new MyDbContext())
            {
                var orderId = GetOrderId(customerId);

                var productsInCart = (from o in myDb.OrderDetails
                                      where o.OrderId == orderId.Id
                                      select o.ProductId).ToList();

                foreach (var product in productsInCart)
                {
                    var quantityInCart = (from o in myDb.OrderDetails
                                          where o.ProductId == product
                                          && o.OrderId == orderId.Id
                                          select o.Quantity).FirstOrDefault();

                    var productInfo = (from p in myDb.Products
                                       where p.Id == product
                                       select p).SingleOrDefault();
                    cart.Add(productInfo.Name + ", antal: " + quantityInCart + ", " + productInfo.Price + " SEK styck. Totalt: " + (quantityInCart * productInfo.Price) + " SEK");
                }
            }
            return cart;
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
