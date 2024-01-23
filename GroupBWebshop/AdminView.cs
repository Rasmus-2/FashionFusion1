using GroupBWebshop.Models;

namespace GroupBWebshop
{
    internal class AdminView
    {
        public static void View()
        {
            Console.WriteLine("1. Add product");
            Console.WriteLine("2. Edit product");
            Console.WriteLine("3. Delete product");
            Console.WriteLine("4. Add category");
            Console.WriteLine("5. Delete category");
            Console.WriteLine("6. View Statistics");
            Console.WriteLine("7. Edit customer account");
            Console.WriteLine("8. Delete customer account");
            Console.WriteLine("9. Log out");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    AddProduct();
                    break;
                case '2':
                    Console.Clear();
                    EditProduct();
                    break;
                case '3':
                    Console.Clear();
                    DeleteProduct();
                    break;
                case '4':
                    Console.Clear();
                    AddCategory();
                    break;
                case '5':
                    Console.Clear();
                    DeleteCategory();
                    break;
                case '6':
                    Console.Clear();
                    CheckStats();
                    break;
                case '7':
                    Console.Clear();
                    EditCustomer();
                    break;
                case '8':
                    Console.Clear();
                    DeleteCustomer();
                    break;
                case '9':
                    Console.Clear();
                    Console.WriteLine("Thank you, see you soon!");
                    Thread.Sleep(3000);
                    Console.Clear();
                    CustomerView.LoginOrAdmin();

                    break;
            }
        }

        public static void CheckStats()
        {
            CustomerView.DrawHomeButton();

            //Best selling products
            DatabaseDapper.BestSelling();

            //Most popular category
            Console.WriteLine();
            DatabaseDapper.MostPopularCategory();

            //Most popular product mens/women
            Console.WriteLine();
            DatabaseDapper.MostPopularInWomen();

            //Most popular product between ages 40
            Console.WriteLine();
            DatabaseDapper.MostPopularForAbove40();

            //Most orders per city
            Console.WriteLine();
            DatabaseDapper.MostOrderPerCity();

            //Sales sorted by supplier
            Console.WriteLine();
            DatabaseDapper.TopSellerBySupplier();

            var input = Console.ReadKey();

            if (input.KeyChar == '0')
            {
                Console.Clear();
                View();
            }
            else
            {
                Console.Clear();
                CheckStats();
            }


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


                string materialChoice = EditMaterial();


                ShowSuppliers();
                //int supplierId = int.Parse(Console.ReadLine());
                int supplierId = 0;
                string inputSupplier = Console.ReadLine();
                if (TryParseInt(inputSupplier) == true && int.Parse(inputSupplier) <= myDb.Suppliers.LongCount())
                {
                    supplierId = int.Parse(inputSupplier);
                }
                else
                {
                    InvalidInputAddProduct();
                }


                Console.WriteLine("Price: ");

                //float price = float.Parse(Console.ReadLine());
                float price = 0;
                string inputPrice = Console.ReadLine();
                if (TryParseFloat(inputPrice) == true)
                {
                    price = float.Parse(inputPrice);
                }
                else
                {
                    InvalidInputAddProduct();
                }


                Console.WriteLine("Info text: ");
                string info = Console.ReadLine();

                Console.WriteLine("Stock status: ");
                //int stockStatus = int.Parse(Console.ReadLine());
                int stockStatus = 0;
                string inputStockStatus = Console.ReadLine();
                if (TryParseInt(inputStockStatus) == true)
                {
                    stockStatus = int.Parse(inputStockStatus);
                }
                else
                {
                    InvalidInputAddProduct();
                }


                ShowCategories();
                Console.WriteLine("How many categories? ");
                //int firstCat = int.Parse(Console.ReadLine());
                int firstCat = 0;
                string firstCategory = Console.ReadLine();
                if (TryParseInt(firstCategory) == true)
                {
                    firstCat = int.Parse(firstCategory);
                }
                else
                {
                    InvalidInputAddProduct();
                }


                List<Category> newCategory = CategoryInput(firstCat);
                foreach (Category category in newCategory)
                {
                    Console.WriteLine("[" + category.Id + "] " + category.Name);
                }

                var product = new Product
                {
                    Name = name,
                    DisplayProduct = inputDisplay,
                    Size = size,
                    Material = materialChoice,
                    SupplierId = supplierId,
                    Price = price,
                    Info = info,
                    StockStatus = stockStatus,
                };
                product.Categories = new List<Category>();

                for (int i = 0; i < firstCat; i++)
                {
                    product.Categories.Add(myDb.Categories.Where(x => x.Id == newCategory[i].Id).SingleOrDefault());
                }

                try
                {
                    myDb.Add(product); //possible try catch
                    myDb.SaveChanges();//in the same curly bracket}

                }
                catch (Exception error)
                {
                    Console.WriteLine("Cannot add product.");
                    Console.WriteLine(error.ToString());
                    Thread.Sleep(5000);
                }
                Console.Clear();
                View();
            }
        }

        public static string EditMaterial()
        {
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
            return materialChoice;
        }

        public static void EditProduct()
        {
            using (var myDb = new MyDbContext())
            {

                foreach (var p in myDb.Products)
                {
                    Console.WriteLine(p.Id + " " + p.Name + " " + p.Price);
                }
                Console.WriteLine();

                Console.WriteLine("Enter product Id: ");

                int prodId = 0;
                string inputString = Console.ReadLine();
                if (TryParseInt(inputString) == true)
                {
                    prodId = int.Parse(inputString);
                }
                else
                {
                    Console.WriteLine("Invalid input! Try write a number.");
                    Thread.Sleep(5000);
                    Console.Clear();
                    EditProduct();
                }



                var choosenEditProd = (from e in myDb.Products
                                       where e.Id == prodId
                                       select e).SingleOrDefault();

                Console.WriteLine("Press n to edit product's name.");
                Console.WriteLine("Press d to edit display setting.");
                Console.WriteLine("Press s to edit product's size.");
                Console.WriteLine("Press m to edit product's material.");
                Console.WriteLine("Press l to edit product's supplier Id.");
                Console.WriteLine("Press p to edit product's price.");
                Console.WriteLine("Press i to edit product's info.");
                Console.WriteLine("Press t to edit product's stock status.");
                Console.WriteLine("Press c to edit product's categories.");
                CustomerView.DrawHomeButton();


                var key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case 'n':
                        Console.Clear();
                        Console.WriteLine("Enter new name: ");
                        string newName = Console.ReadLine();

                        choosenEditProd.Name = newName;

                        myDb.Update(choosenEditProd);

                        myDb.SaveChanges();

                        EditProduct();
                        break;

                    case 'd':
                        Console.Clear();
                        Console.WriteLine("Is this product on display? " + choosenEditProd.DisplayProduct);

                        Console.WriteLine("Display product? y/n");
                        var key1 = Console.ReadKey();
                        if (key1.KeyChar == 'y')
                        {
                            choosenEditProd.DisplayProduct = true;

                        }
                        else if (key1.KeyChar == 'n')
                        {
                            choosenEditProd.DisplayProduct = false;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");

                        }

                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();

                        EditProduct();
                        break;

                    case 's':
                        Console.Clear();
                        Console.Write("Size: | ");
                        foreach (int i in Enum.GetValues(typeof(MyEnums.EnumSize)))
                        {
                            Console.Write(Enum.GetName(typeof(MyEnums.EnumSize), i) + " | ");
                        }
                        Console.WriteLine();

                        Console.WriteLine("Current product's size: " + choosenEditProd.Size);
                        Console.WriteLine("Enter new size: ");
                        string newSize = Console.ReadLine().ToUpper();
                        choosenEditProd.Size = newSize;
                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;

                    case 'm':
                        Console.Clear();
                        string newMaterial = EditMaterial();
                        choosenEditProd.Material = newMaterial;
                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;

                    case 'l':
                        Console.Clear();
                        ShowSuppliers();
                        Console.WriteLine("Current product's supplier Id: " + choosenEditProd.SupplierId);
                        Console.WriteLine("Enter new supplier Id: ");
                        int newSupplier = 0;
                        string newSupplierString = Console.ReadLine();
                        if (TryParseInt(newSupplierString) == true)
                        {
                            newSupplier = int.Parse(newSupplierString);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Try write a number. Wait to be redirected");
                            Thread.Sleep(5000);
                            Console.Clear();
                            EditProduct();
                        }
                        choosenEditProd.SupplierId = newSupplier;
                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;

                    case 'p':
                        Console.Clear();
                        Console.WriteLine("Current product's price: " + choosenEditProd.Price);
                        Console.WriteLine("Enter new price: ");
                        float newPrice = 0;
                        string newPriceString = Console.ReadLine();
                        if (TryParseFloat(newPriceString) == true)
                        {
                            newPrice = float.Parse(newPriceString);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Try write a number. Wait to be redirected");
                            Thread.Sleep(5000);
                            Console.Clear();
                            EditProduct();
                        }
                        choosenEditProd.Price = newPrice;
                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;

                    case 'i':
                        Console.Clear();
                        foreach (var p in myDb.Products)
                        {
                            Console.WriteLine(p.Name + " " + p.Info);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Current product's info: " + choosenEditProd.Info);
                        Console.WriteLine("Enter new info: ");
                        string newInfo = Console.ReadLine();
                        choosenEditProd.Info = newInfo;
                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;
                    case 't':
                        Console.Clear();
                        foreach (var p in myDb.Products)
                        {
                            Console.WriteLine(p.Name + " " + p.StockStatus);
                        }
                        Console.WriteLine();
                        Console.WriteLine("Current product's stock status: " + choosenEditProd.StockStatus);
                        Console.WriteLine("Enter new stock: ");
                        int newStock = 0;
                        string newStockString = Console.ReadLine();
                        if (TryParseInt(newStockString) == true)
                        {
                            newStock = int.Parse(newStockString);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Try write a number. Wait to be redirected");
                            Thread.Sleep(5000);
                            Console.Clear();
                            EditProduct();
                        }
                        choosenEditProd.StockStatus = newStock;
                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;

                    case 'c':
                        Console.Clear();
                        ShowCategories();

                        // Assuming choosenEditProd is a tracked entity
                        myDb.Entry(choosenEditProd).Collection(p => p.Categories).Load();

                        // Remove the associations between the product and categories
                        choosenEditProd.Categories.Clear();


                        //myDb.Categories.RemoveRange(choosenEditProd.Categories);

                        //var exCategory = myDb.Products.Where(x => x.Id == prodId && x.Categories != null);
                        //choosenEditProd.Categories.Remove(exCategory);

                        //choosenEditProd.Categories.Remove(myDb.Categories.Where(x => x.Id == choosenEditProd.Id).SingleOrDefault());


                        Console.WriteLine("How many categories? ");
                        int firstCat = 0;
                        string firstCatString = Console.ReadLine();
                        if (TryParseInt(firstCatString) == true)
                        {
                            firstCat = int.Parse(firstCatString);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Try write a number. Wait to be redirected");
                            Thread.Sleep(5000);
                            Console.Clear();
                            EditProduct();
                        }
                        List<Category> newCategory = CategoryInput(firstCat);

                        choosenEditProd.Categories = new List<Category>();

                        for (int i = 0; i < firstCat; i++)
                        {
                            choosenEditProd.Categories.Add(myDb.Categories.Where(x => x.Id == newCategory[i].Id).SingleOrDefault());
                        }


                        myDb.Update(choosenEditProd);
                        myDb.SaveChanges();
                        EditProduct();
                        break;

                    default:
                        Console.Clear();
                        View();
                        break;

                }



            }
        }

        public static void EditCustomer()
        {
            using (var myDb = new MyDbContext())
            {
                Console.Clear();
                foreach (var p in myDb.Customers)
                {
                    Console.WriteLine(p.Id + " " + p.Name + " " + p.Email + " " + p.Phone + " " + p.BirthDate + " " + p.StreetName + " " + p.PostalCode + " " + p.City + " " + p.CountryId);
                }
                Console.WriteLine();

                Console.WriteLine("Enter customer Id: ");

                int customerId = 0;
                string customerIdString = Console.ReadLine();
                if (TryParseInt(customerIdString) == true)
                {
                    if (int.Parse(customerIdString) <= myDb.Customers.LongCount())
                    {
                        customerId = int.Parse(customerIdString);
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input! Try picking a number from the options.");
                        Thread.Sleep(2000);
                        Console.Clear();
                        EditCustomer();
                    }

                }
                else
                {
                    Console.WriteLine("Invalid input! Try write a number. Wait to be redirected");
                    Thread.Sleep(2000);
                    Console.Clear();
                    EditCustomer();
                }

                var choosenEditCustomer = (from e in myDb.Customers
                                           where e.Id == customerId
                                           select e).SingleOrDefault();

                Console.WriteLine("Press n to edit customer's name.");
                Console.WriteLine("Press d to edit customer's email.");
                Console.WriteLine("Press s to edit customer's phone number.");
                Console.WriteLine("Press m to edit customer's birthdate.");
                Console.WriteLine("Press l to edit customer's streetname.");
                Console.WriteLine("Press p to edit customer's postal code.");
                Console.WriteLine("Press i to edit customer's city.");
                Console.WriteLine("Press t to edit customer's country.");
                Console.WriteLine("Press b to check customer's order history.");
                CustomerView.DrawHomeButton();

                var key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case 'n':
                        Console.Clear();
                        Console.WriteLine("Enter new name: ");
                        string newName = Console.ReadLine();

                        choosenEditCustomer.Name = newName;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;

                    case 'd':
                        Console.Clear();
                        Console.WriteLine("Enter new email: ");
                        string newEmail = Console.ReadLine();

                        choosenEditCustomer.Email = newEmail;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;

                    case 's':
                        Console.Clear();
                        Console.WriteLine("Enter new phone number: ");
                        Console.WriteLine("Example: +46123456910");
                        string newPhone = Console.ReadLine();

                        choosenEditCustomer.Phone = newPhone;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;
                    case 'm':
                        Console.Clear();
                        Console.WriteLine("Enter new birthdate: ");
                        Console.WriteLine("Example: yyyy-mm-dd");
                        DateTime newBirthdate;
                        bool success = DateTime.TryParse(Console.ReadLine(), out DateTime result);
                        if (success)
                        {
                            newBirthdate = result;
                        }
                        else
                        {
                            Console.WriteLine("Not a valid input! Try again.");
                            Thread.Sleep(5000);
                            Console.Clear();
                            EditCustomer();
                            newBirthdate = result;
                        }

                        choosenEditCustomer.BirthDate = newBirthdate;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;

                    case 'l':
                        Console.Clear();
                        Console.WriteLine("Enter new streetname: ");
                        Console.WriteLine("Example: Normalvägen 123 ");
                        string newStreetname = Console.ReadLine();

                        choosenEditCustomer.StreetName = newStreetname;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;

                    case 'p':
                        Console.Clear();
                        Console.WriteLine("Enter new Postal Code: ");
                        Console.WriteLine("Example: 633 60 ");
                        string newPostalcode = Console.ReadLine();

                        choosenEditCustomer.PostalCode = newPostalcode;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;

                    case 'i':
                        Console.Clear();
                        Console.WriteLine("Enter new City: ");
                        Console.WriteLine("Example: Eskilstuna ");
                        string newCity = Console.ReadLine();

                        choosenEditCustomer.City = newCity;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;
                    case 't':
                        Console.Clear();
                        foreach (var country in myDb.Countries)
                        {
                            Console.WriteLine(country.Id + " " + country.Name);
                        }
                        Console.WriteLine("Enter new country Id: ");

                        int newCountryId = 0;
                        string newCountryIdString = Console.ReadLine();
                        if (TryParseInt(newCountryIdString) == true && int.Parse(newCountryIdString) <= myDb.Customers.LongCount())
                        {
                            newCountryId = int.Parse(newCountryIdString);
                        }
                        else
                        {
                            Console.WriteLine("Not a valid input! Wait to be redirected.");
                            Thread.Sleep(5000);
                            Console.Clear();
                            EditCustomer();
                        }

                        choosenEditCustomer.CountryId = newCountryId;

                        myDb.Update(choosenEditCustomer);

                        myDb.SaveChanges();

                        EditCustomer();
                        break;

                    case 'b':
                        Console.Clear();
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
                        Console.WriteLine("Press Enter to Continue");
                        Console.ReadLine();

                        EditCustomer();

                        break;

                    default:
                        Console.Clear();
                        View();
                        break;

                }
            }
        }

        public static void DeleteCustomer()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var p in myDb.Customers)
                {
                    Console.WriteLine(p.Id + " " + p.Name + " " + p.Email + " " + p.Phone + " " + p.BirthDate + " " + p.StreetName + " " + p.PostalCode + " " + p.City + " " + p.CountryId);
                }
                Console.WriteLine();

                Console.WriteLine("Enter customer Id: ");

                int deleteCustomer = 0;
                string deleteCustomerString = Console.ReadLine();
                if (TryParseInt(deleteCustomerString) == true && int.Parse(deleteCustomerString) <= myDb.Customers.LongCount())
                {
                    deleteCustomer = int.Parse(deleteCustomerString);
                }
                else
                {
                    Console.WriteLine("Not a valid input! Wait to be redirected.");
                    Thread.Sleep(5000);
                    Console.Clear();
                    DeleteCustomer();
                }


                var choosenDeleteCustomer = (from e in myDb.Customers
                                             where e.Id == deleteCustomer
                                             select e).SingleOrDefault();

                myDb.Customers.Remove(choosenDeleteCustomer);
                myDb.SaveChanges();
                Console.Clear();
                View();

            }
        }
        public static void DeleteCategory()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var c in myDb.Categories)
                {
                    Console.WriteLine(c.Id + " " + c.Name);
                }
                Console.WriteLine("Enter category Id you wish to delete: ");

                //int deleteCat = int.Parse(Console.ReadLine());
                int deleteCat = 0;
                string deleteCatString = Console.ReadLine();
                if (TryParseInt(deleteCatString) == true && int.Parse(deleteCatString) <= myDb.Categories.LongCount())
                {
                    deleteCat = int.Parse(deleteCatString);
                }
                else
                {
                    Console.WriteLine("Not a valid input! Wait to be redirected.");
                    Thread.Sleep(5000);
                    Console.Clear();
                    DeleteCategory();
                }

                var choosenDeleteCat = (from e in myDb.Categories
                                        where e.Id == deleteCat
                                        select e).SingleOrDefault();

                myDb.Categories.Remove(choosenDeleteCat);
                myDb.SaveChanges();
                Console.Clear();
                View();

            }
        }
        public static void AddCategory()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var c in myDb.Categories)
                {
                    Console.WriteLine(c.Id + " " + c.Name);
                }
                Console.WriteLine("Enter category name: ");
                string catName = Console.ReadLine();
                Category category = new Category() { Name = catName };
                myDb.Categories.Add(category);
                myDb.SaveChanges();
                Console.Clear();
                View();
            }
        }
        public static void DeleteProduct()
        {
            using (var myDb = new MyDbContext())
            {

                foreach (var p in myDb.Products)
                {
                    Console.WriteLine(p.Id + " " + p.Name + " " + p.Price);
                }
                Console.WriteLine();

                Console.WriteLine("Enter product Id: ");

                //int prodId = int.Parse(Console.ReadLine());
                int prodId = 0;
                string prodIdString = Console.ReadLine();
                if (TryParseInt(prodIdString) == true && int.Parse(prodIdString) <= myDb.Products.LongCount())
                {
                    prodId = int.Parse(prodIdString);
                }
                else
                {
                    Console.WriteLine("Not a valid input! Wait to be redirected.");
                    Thread.Sleep(5000);
                    Console.Clear();
                    DeleteProduct();
                }

                var choosenDeleteProd = (from e in myDb.Products
                                         where e.Id == prodId
                                         select e).SingleOrDefault();

                myDb.Products.Remove(choosenDeleteProd);
                myDb.SaveChanges();
                Console.Clear();
                View();

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
                    Console.WriteLine("Enter category " + (i + 1) + " Id: ");

                    //int catText = int.Parse(Console.ReadLine());
                    int catText = 0;
                    string catTextString = Console.ReadLine();
                    if (TryParseInt(catTextString) == true && int.Parse(catTextString) <= myDb.Categories.LongCount())
                    {
                        catText = int.Parse(catTextString);
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input! Wait to be redirected.");
                        CategoryInput(numCategory);
                    }


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

                foreach (Supplier supplier in suppliers)
                {
                    Console.WriteLine("[" + supplier.Id + "] " + supplier.Name);
                }
            }
        }
        public static bool TryParseInt(string input)
        {
            bool success = int.TryParse(input, out int number);

            if (success)
            {
                return true;
            }

            return false;
        }

        public static bool TryParseFloat(string input)
        {
            bool success = float.TryParse(input, out float number);

            if (success)
            {
                return true;
            }

            return false;
        }

        public static void InvalidInputAddProduct()
        {
            Console.WriteLine("Not a valid input! Wait to be redirected.");
            Thread.Sleep(5000);
            Console.Clear();
            AddProduct();
        }
    }
}
