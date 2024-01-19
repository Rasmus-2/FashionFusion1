using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GroupBWebshop.Migrations;
using GroupBWebshop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using static System.Net.Mime.MediaTypeNames;

namespace GroupBWebshop
{
    internal class CustomerView
    {
        public static void LoginOrAdmin()
        {
            string password = "asd";
            Console.WriteLine("Admin or customer? a/c");
            while (true)
            {
                var input = Console.ReadKey();

                switch (input.KeyChar)
                {
                    case 'a':
                        Console.WriteLine("Enter password:");
                        var pass = Console.ReadLine();
                        while(pass != password)
                        {
                            Console.Clear();
                            Console.WriteLine("Wrong password, try again");
                            pass = Console.ReadLine();
                        }
                        AdminView.View();
                        break;

                    case 'c':
                        Console.Clear();
                        Console.WriteLine("New or Existing customer? n/e");
                        var key = Console.ReadKey();
                        using (var myDb = new MyDbContext())
                        {


                            switch (key.KeyChar)
                            {
                                case 'n':
                                    Console.WriteLine("Enter name: ");
                                    string name = Console.ReadLine();
                                    Console.WriteLine("Enter email address:");
                                    string email = Console.ReadLine();
                                    Console.WriteLine("Enter phone number: ");
                                    string phoneNumber = Console.ReadLine();
                                    Console.WriteLine("Enter birth date: yyyy-mm-dd ");
                                    DateTime birthDate = DateTime.Parse(Console.ReadLine());
                                    Console.WriteLine("Enter street name: ");
                                    string street = Console.ReadLine();
                                    Console.WriteLine("Enter postal code: ");
                                    string postal = Console.ReadLine();
                                    Console.WriteLine("Enter city: ");
                                    string city = Console.ReadLine();
                                    Console.WriteLine("Enter country: ");
                                    string country = Console.ReadLine();

                                    var countryName = (from c in myDb.Countries where country == c.Name select c.Id).SingleOrDefault();
                                    myDb.Add(new Customer { Name = name, Email = email, Phone = phoneNumber, BirthDate = birthDate, StreetName = street, PostalCode = postal, City = city, CountryId = countryName });
                                    myDb.SaveChanges();

                                    var loginId = (from l in myDb.Customers where l.Name == name select l.Id).SingleOrDefault();


                                    Console.Clear();
                                    Console.WriteLine("Welcome " + name);
                                    CustomerView.View(loginId);
                                    break;
                                case 'e':
                                    Console.WriteLine("Enter your name: ");
                                    string login = Console.ReadLine();

                                    var lo = (from l in myDb.Customers where l.Name == login select l).SingleOrDefault();
                                    if (lo == null)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("WRONG");
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Welcome back " + lo.Name!);
                                        CustomerView.View(lo.Id);
                                    }

                                    break;
                            }
                        }
                        //CustomerView.View();
                        break;
                }
            }
        }
        public static void View(int customerId)
        {
            List<string> welcomeText = new List<string>() { "Welcome to Fashion Fushion", "The fashion for sustainable generation" };
            Window text = new Window("", 5, 1, welcomeText);
            text.Draw();

            //List<string> productDisplay = new List<string>() ;


           
            

            using (var myDb = new MyDbContext())
            {
                var clothes = from category in myDb.Categories
                              select category.Name;
                Window window = new Window("Category", 2, 10, clothes.ToList());
                window.Draw();
                var productDisplay =
                    from product in myDb.Products
                    where product.DisplayProduct
                    select product.Name;
                //List<string> productsDisplay = new List<string>();
                Window display = new Window("Favorites", 2, 5, productDisplay.ToList());
                display.Draw();

                var cart = CartView(customerId);


                Window box = new Window("Cart", 55, 1, cart);
                box.Draw();

                Console.WriteLine("What would you like to do today?");
                Console.WriteLine("1. View our favorites");
                Console.WriteLine("2. View categories");
                Console.WriteLine("3. View products");
                Console.WriteLine("4. View cart");
                Console.WriteLine("5. View account information");
                Console.WriteLine("6. View history");
                Console.WriteLine("7. Search");
                Console.WriteLine("8. Log out");
                

                var key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case '1':
                        Console.Clear();
                        ViewFavoritesCase(customerId);
                        break;

                    case '2':
                        Console.Clear();
                        ViewCategoryCase(customerId);
                        break;

                    case '3':
                        Console.Clear();
                        ViewProductsCase(customerId);
                        break;

                    case '4':
                        Console.Clear();
                        ViewCartCase(customerId);
                        break;
                        

                    case '5':
                        Console.Clear();
                        ViewCustomerAccCase(customerId);
                        break;

                    case '6':
                        Console.Clear();
                        ViewHistoryCase(customerId);
                        break;

                    case '7':
                        Console.Clear();
                        SearchCase(customerId);
                        break;

                    case '8':
                        Console.Clear();
                        Console.WriteLine("Thank you, see you soon!");
                        Thread.Sleep(3000);
                        Console.Clear();
                        LoginOrAdmin();
                        break;
                   
                }
            }
        }

        public static void ProductView(int customerId, int inputId, bool quit)
        {
            using (var myDb = new MyDbContext())
            {
                Console.WriteLine("1. Add to cart");
                Console.WriteLine("2. View another product");
                
                var getout = Console.ReadKey();
                if (getout.KeyChar == '0')
                {
                    quit = true;
                    Console.Clear();
                    View(customerId);

                }
                else if (getout.KeyChar == '2')
                {
                    Console.Clear();
                    ViewProductsCase(customerId);
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

                    Console.WriteLine("Your product was added to the cart");



                    Thread.Sleep(2000);
                    Console.Clear();
                    View(customerId);

                }
            }
        }
        public static void ViewCategoryCase(int customerId)
        {
            using(var myDb = new MyDbContext())
            {
                DrawHomeButton();
                Console.SetCursorPosition(0, 0);
                foreach(var c in myDb.Categories)
                {
                    Console.WriteLine(c.Id + " " + c.Name);
                }
                Console.WriteLine("Choose category Id to check all products in that category");
                var idInput = int.Parse(Console.ReadLine());
                if (idInput == 0)
                {
                    Console.Clear();
                    View(customerId);
                }
                else
                {
                    ViewProductInCategory(customerId, idInput);
                }

                
            }
        }

        public static void ViewProductInCategory(int customerId,int idInput)
        {
            using (var myDb = new MyDbContext())
            {
                Console.Clear();
                DrawHomeButton();
                Console.SetCursorPosition(0, 0);
                foreach (var c in myDb.Categories.Include(i=> i.Products).Where(i=> i.Id == idInput))
                {
                    Console.WriteLine("Category: " + c.Name);
                    foreach (var p in c.Products)
                    {
                        Console.WriteLine(p.Id + " " +  p.Name);
                    }

                    Console.WriteLine("Choose a product Id to check its info");

                    var inputId = int.Parse(Console.ReadLine());
                    if (inputId == 0)
                    {
                        Console.Clear();
                        View(customerId);
                    }
                    ViewProduct(inputId);
                    ProductView(customerId,inputId,false);
                }
            }
        }
        public static void ViewFavoritesCase(int customerId)
        { 
            using (var myDb = new MyDbContext()) 
            {
                DrawHomeButton(100);
                Console.SetCursorPosition(0, 0);
                foreach (var i in myDb.Products.Where(x => x.DisplayProduct))
                {
                    Console.WriteLine(i.Id + " " + i.Name + " " + i.Info + " " + i.Size + " " + i.Price + "SEK, " + i.StockStatus + " left in stock.");
                }
                Console.WriteLine("Choose product Id to add to cart: ");

                var idInput = int.Parse(Console.ReadLine());
                if (idInput != 0)
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

                    OrderDetails orderDetails = new OrderDetails() { OrderId = getOrderId.Id, ProductId = idInput, Quantity = quantity };
                    myDb.Add(orderDetails);

                    myDb.SaveChanges();


                }
                else if (idInput == 0)
                {
                    View(customerId);
                }


                AddToCart(idInput);
                myDb.SaveChanges();
                Console.Clear();
                View(customerId);

            }
        }
        public static void SearchCase(int customerId)
        { 
            using (var myDb = new MyDbContext()) 
            {
                Console.WriteLine("Search...");
                var search = Console.ReadLine();

                var productSearch = (from h in myDb.Products where h.Name.Contains(search) || h.Info.Contains(search) || h.Size.Contains(search) || h.Material.Contains(search) select h);
                foreach (var product in productSearch)
                {
                    Console.WriteLine(product.Name + " *" + product.Info + "* - " + product.Size + " " + product.Price + " SEK");
                    Console.WriteLine();
                }
                Console.WriteLine("Press enter to go back");
                Console.ReadLine();
                Console.Clear();
                View(customerId);
            }
        }
        public static void ViewHistoryCase(int customerId)
        { 
            using (var myDb = new MyDbContext()) 
            {
                var history = (
                                        from h in myDb.Orders
                                        join hi in myDb.OrderDetails on h.Id equals hi.OrderId
                                        join his in myDb.Products on hi.ProductId equals his.Id
                                        where (h.Completed == true && h.CustomerId == customerId)
                                        select new
                                        {
                                            id = h.Id,
                                            name = his.Name,
                                            price = his.Price,
                                            quantity = hi.Quantity,
                                            payment = h.Payment,
                                            delivery = h.Delivery
                                        });
                Console.WriteLine();
                Console.WriteLine("Order history: ");
                foreach (var h in history)
                {

                    Console.WriteLine("Order Id: " + h.id + "\nProduct: " + h.name + "\nPrice: " + h.price + "\nQuantity: " + h.quantity + "\nPayment: " + h.payment + "\nDelivery: " + h.delivery);
                    Console.WriteLine();
                }
                DrawHomeButton();
                var zero1 = Console.ReadKey();
                if (zero1.KeyChar == '0')
                {
                    Console.Clear();
                    View(customerId);
                }
            }
        }

        public static void ViewCustomerAccCase(int customerId)
        { 
            using(var myDb = new MyDbContext()) 
            {
                var infoAcc = (from c in myDb.Customers where c.Id == customerId select c).SingleOrDefault();
                Console.WriteLine("Name : " + infoAcc.Name + "\nEmail : " + infoAcc.Email + "\nBirth date : " + infoAcc.BirthDate + "\nAddress : " + infoAcc.StreetName + ", " + infoAcc.PostalCode + ", " + infoAcc.City + ".");

                DrawHomeButton();

                var zero = Console.ReadKey();
                if (zero.KeyChar == '0')
                {
                    Console.Clear();
                    View(customerId);
                }
            }
        }
        public static void ViewCartCase(int customerId)
        {
            using (var myDb = new MyDbContext())
            {
            
                var finalCart = CartView(customerId);
                var total = CartPrice(customerId);
                finalCart.Add("");
                finalCart.Add("Total sum: " + total + " SEK");
                Window cartBox = new Window("Your cart", 1, 1, finalCart);
                cartBox.Draw();
                DrawHomeButton(70);
                if (total == 0)
                {
                    Thread.Sleep(4000);
                    Console.Clear();

                    View(customerId);
                }
                else
                {
                    Console.WriteLine("Would you like to edit your order? y/n");
                    char answer = char.Parse(Console.ReadLine());
                    if (answer == 'y')
                    {
                        Console.WriteLine("Would you like to remove or add product? r/a");
                        var keyInput = Console.ReadKey();

                        switch (keyInput.KeyChar)
                        {
                            case 'r':

                                Console.WriteLine("Enter product's Id that you wish to remove: ");
                                int removeProdId = int.Parse(Console.ReadLine());

                                Console.WriteLine("How many would you like to remove? ");
                                int removeQuantity = int.Parse(Console.ReadLine());

                                var removeFromCart = (from o in myDb.OrderDetails
                                                      where o.ProductId == removeProdId
                                                      select o).SingleOrDefault();

                                removeFromCart.Quantity -= removeQuantity;

                                if (removeFromCart.Quantity <= 0)
                                {
                                    foreach (var o in myDb.OrderDetails.Where(x => x.ProductId == removeProdId))
                                    {
                                        myDb.OrderDetails.Remove(o);
                                    }
                                }

                                myDb.SaveChanges();
                                Console.Clear();
                                View(customerId);
                                break;

                            case 'a':
                                Console.WriteLine("Enter product's Id that you wish to add: ");
                                int addProdId = int.Parse(Console.ReadLine());

                                Console.WriteLine("How many would you like to add? ");
                                int addQuantity = int.Parse(Console.ReadLine());

                                var order = GetOrderId(customerId);

                                var addToCart = (from o in myDb.OrderDetails
                                                 where o.ProductId == addProdId && o.OrderId == order.Id
                                                 select o).SingleOrDefault();

                                addToCart.Quantity += addQuantity;

                                myDb.Update(addToCart);
                                myDb.SaveChanges();
                                Console.Clear();
                                View(customerId);
                                break;

                        }
                    }
                    else if (answer == '0')
                    {
                        Console.Clear();
                        View(customerId);
                    }
                    else
                    {
                        Console.WriteLine("Choose delivery method: ");

                        foreach (int i in Enum.GetValues(typeof(MyEnums.DeliveryMethod)))
                        {
                            Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.DeliveryMethod), i).Replace('_', ' '));
                        }
                        int number1;
                        string deliveryChoice = "";

                        if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out number1))
                        {
                            MyEnums.DeliveryMethod selection1 = (MyEnums.DeliveryMethod)number1;


                            switch (selection1)
                            {
                                case MyEnums.DeliveryMethod.DHL:
                                    deliveryChoice = MyEnums.DeliveryMethod.DHL.ToString();
                                    Console.WriteLine("You choose DHL!");
                                    break;

                                case MyEnums.DeliveryMethod.Postnord:
                                    deliveryChoice = MyEnums.DeliveryMethod.Postnord.ToString();
                                    Console.WriteLine("You choose Postnord!");
                                    break;

                                case MyEnums.DeliveryMethod.Schenker:
                                    deliveryChoice = MyEnums.DeliveryMethod.Schenker.ToString();
                                    Console.WriteLine("You choose Schenker!");
                                    break;


                                default:
                                    Console.Clear();
                                    View(customerId);
                                    break;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }






                        Console.Clear();
                        cartBox.Draw();
                        DrawHomeButton(70);
                        Console.WriteLine("Payment methods: ");
                        foreach (int i in Enum.GetValues(typeof(MyEnums.PaymentMethod)))
                        {
                            Console.WriteLine(i + ". " + Enum.GetName(typeof(MyEnums.PaymentMethod), i).Replace('_', ' '));
                        }
                        int number;
                        string paymentChoice = "";
                        if (int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out number))
                        {
                            MyEnums.PaymentMethod selection = (MyEnums.PaymentMethod)number;


                            switch (selection)
                            {
                                case MyEnums.PaymentMethod.Klarna:
                                    paymentChoice = MyEnums.PaymentMethod.Klarna.ToString();
                                    Console.WriteLine("You choose Klarna!");
                                    break;

                                case MyEnums.PaymentMethod.Credit_Card:
                                    paymentChoice = MyEnums.PaymentMethod.Credit_Card.ToString();
                                    Console.WriteLine("You choose Credit Card!");
                                    break;

                                case MyEnums.PaymentMethod.PayPal:
                                    paymentChoice = MyEnums.PaymentMethod.PayPal.ToString();
                                    Console.WriteLine("You choose PayPal!");
                                    break;

                                case MyEnums.PaymentMethod.Swish:
                                    paymentChoice = MyEnums.PaymentMethod.Swish.ToString();
                                    Console.WriteLine("You choose Swish!");
                                    break;
                                default:
                                    Console.Clear();
                                    View(customerId);
                                    break;
                            }

                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }



                        var completedOrder = GetOrderId(customerId);



                        var stockStatus = (
                                    from h in myDb.Orders
                                    join hi in myDb.OrderDetails on h.Id equals hi.OrderId
                                    join his in myDb.Products on hi.ProductId equals his.Id
                                    where (h.Completed == false && h.CustomerId == customerId)
                                    select new
                                    {
                                        id = his.Id,
                                        stockStatus = his.StockStatus,
                                        quantity = hi.Quantity

                                    }).ToList();

                        foreach (var status in stockStatus)
                        {
                            var quantityUpdate = (from q in myDb.Products
                                                  where q.Id == status.id
                                                  select q).SingleOrDefault();
                            quantityUpdate.StockStatus -= status.quantity;

                            myDb.Update(quantityUpdate);
                            myDb.SaveChanges();

                        }

                        completedOrder.Completed = true;
                        completedOrder.Delivery = deliveryChoice;
                        completedOrder.Payment = paymentChoice;

                        //myDb.Update(stockStatus);
                        myDb.Update(completedOrder);
                        myDb.SaveChanges();

                        Console.WriteLine("Thank you for your order!");
                        Console.ReadLine();
                        Console.Clear();
                        View(customerId);
                    }
                }
            }
        }

        public static void ViewProductsCase(int customerId)
        {
            using (var myDb = new MyDbContext())
            {
                bool quit = false;
                while (quit == false)
                {
                   
                    ViewAllProducts();
                    var inputId = int.Parse(Console.ReadLine());
                    if (inputId == 0)
                    {
                        Console.Clear();
                        View(customerId);
                    }
                    ViewProduct(inputId);
                    ProductView(customerId,inputId,quit);
                    
                }
            }
        }

        public static double CartPrice(int customerId)
        {
            double total = 0;

            using (var myDb = new MyDbContext())
            {
                var orderId = GetOrderId(customerId);

                if(orderId != null)
                {
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
                    return total;
                }
                else
                {
                    return 0; 
                }
               

            }


                
        }

        public static List<string> CartView(int customerId)
        {
            List<string> cart = new List<string>() { };

            using (var myDb = new MyDbContext())
            {
                var orderId = GetOrderId(customerId);

                var orderDetails = (from o in myDb.OrderDetails
                                    where o.OrderId == orderId.Id
                                    select o);

                if (orderId == null)
                {

                    cart.Add("empty");
                    return cart;
                }
                else if (orderDetails.Count() <= 0)
                {
                  cart.Add("empty");
                  return cart;
                }
                
                


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
                    cart.Add(productInfo.Id+ ". " + productInfo.Name + ", antal: " + quantityInCart + ", " + productInfo.Price + " SEK per product. Totalt: " + (quantityInCart * productInfo.Price) + " SEK");
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
                Console.WriteLine(productChoosen.Name + ", *" + productChoosen.Info + "* " + productChoosen.Price + " SEK, " + productChoosen.Size);
            }
        }
        public static void ViewAllProducts()
        {
            using (var myDb = new MyDbContext())
            {
                DrawHomeButton();
                Console.SetCursorPosition(0, 0);
                foreach (var p in myDb.Products)
                {
                    Console.WriteLine(p.Id + ". " + p.Name + " " + p.Price + " SEK");

                }
                Console.WriteLine();

                Console.WriteLine("View product's info");
                Console.WriteLine("Enter product's Id: ");

                


            }
        }

        public static void DrawHomeButton(int left = 50)
        {
            List<string> list = new List<string>() { "Press 0!" };
            Window homeBox = new Window("Home", left, 1, list);
            homeBox.Draw();

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
