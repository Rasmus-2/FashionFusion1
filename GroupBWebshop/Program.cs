using GroupBWebshop.Models;
using System;
using System.Security.Cryptography;

namespace GroupBWebshop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Admin or customer? a/c");
            while (true)
            {
                var input = Console.ReadKey();

                switch (input.KeyChar)
                {
                    case 'a':

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
    }
}