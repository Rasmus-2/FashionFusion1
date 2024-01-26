using Dapper;
using GroupBWebshop.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupBWebshop
{
    internal class DatabaseDapper
    {
        static string connString = "data source=.\\SQLEXPRESS; initial catalog = FashionFusion; persist security info = True; Integrated Security = True;";
        public static void BestSelling()
        {
            string sql = @"SELECT Top 5 p.Name, SUM(od.Quantity) as [Top Seller] FROM OrderDetails od JOIN Products p on od.ProductId = p.Id
             GROUP BY p.Name ORDER BY [Top Seller] DESC";

            List<string> bestSelling = new List<string>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                bestSelling = connection.Query<string>(sql).ToList();
                Console.WriteLine("Best selling products: ");
                foreach (var product in bestSelling)
                {
                    Console.WriteLine(product + " ");
                }
            }
        }

        public static void MostPopularCategory()
        {
            string sql = @"SELECT c.Name,
                            SUM(cp.CategoriesId) AS ""PopularCategory""
                            FROM Categories c
                            JOIN CategoryProduct cp
                            ON c.Id = cp.CategoriesId
                            JOIN Products p 
                            ON p.Id = cp.ProductsId
                            GROUP BY c.Name
                            ORDER BY SUM(cp.CategoriesId) DESC";

            List<PopularCategoryList> popularCategory = new List<PopularCategoryList>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                popularCategory = connection.Query<PopularCategoryList>(sql).ToList();
                Console.WriteLine("Most popular category: ");
                foreach (var product in popularCategory)
                {
                    Console.WriteLine(product.Name + "\t" + product.PopularCategory + " products sold");
                }
            }

          
        }

        public static void MostPopularInWomen()
        {
            string sql = @"SELECT p.Name,
                            Sum(cp.ProductsId) AS ""PopularCategory""
                            FROM Categories c
                            JOIN CategoryProduct cp
                            ON c.Id = cp.CategoriesId
                            JOIN Products p 
                            ON p.Id = cp.ProductsId
                            WHERE c.Name = 'Women'
                            GROUP BY p.Name
                            ORDER BY Sum(cp.ProductsId) DESC";

            List<PopularCategoryList> popularCategory = new List<PopularCategoryList>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                popularCategory = connection.Query<PopularCategoryList>(sql).ToList();
                Console.WriteLine("Most popular product in Women category: ");
                foreach (var product in popularCategory)
                {
                    Console.WriteLine(product.Name + "\t" + product.PopularCategory + " product(s) sold");
                }
            }
        }

        public static void MostPopularForAbove40()
        {
            string sql = @"SELECT p.Name,
                            SUM(od.Quantity) AS ""Total Order"",
                            (2024 -Year(c.BirthDate)) AS Age,
                            Count(2024 -Year(c.BirthDate)) AS ""Amount of person""
                            FROM OrderDetails od 
                            JOIN Products p 
                            ON od.ProductId = p.Id
                            JOIN Orders o
                            ON o.Id = od.OrderId
                            JOIN Customers c
                            ON c.Id = o.CustomerId
                            GROUP BY (2024 -Year(c.BirthDate)), p.Name
                            HAVING (2024 -Year(c.BirthDate)) > 40
                            ORDER BY SUM(od.Quantity) DESC ";

            List<string> bestSelling = new List<string>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                bestSelling = connection.Query<string>(sql).ToList();
                Console.WriteLine("Best selling products for customer above 40: ");
                foreach (var product in bestSelling)
                {
                    Console.WriteLine(product + " ");
                }
            }
        }

        public static void MostOrderPerCity()
        {
            string sql = @"SELECT
                            c.City AS City,
                            COUNT(o.Id) AS ""Amount of Order""
                            FROM OrderDetails od
                            JOIN Orders o
                            ON o.Id = od.OrderId
                            JOIN Customers c
                            ON c.Id = o.CustomerId
                            WHERE o.Completed = 1
                            GROUP BY c.City
                            ORDER BY c.City ";

            List<string> bestSellingPerCity = new List<string>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                bestSellingPerCity = connection.Query<string>(sql).ToList();
                Console.WriteLine("Most order per city: ");
                foreach (var product in bestSellingPerCity)
                {
                    Console.WriteLine(product + " ");
                }
            }
        }

        public static void TopSellerBySupplier()
        {
            string sql = @"SELECT 
                            s.Name AS Supplier,
                            (COUNT(o.Id)) AS Sold,
                            p.Name AS Product
                            FROM Suppliers s
                            JOIN Products p
                            ON p.SupplierId = s.Id
                            JOIN OrderDetails od
                            ON od.ProductId = p.Id
                            JOIN Orders o
                            ON o.Id = od.OrderId
                            WHERE o.Completed = 1 
                            GROUP BY s.Name, p.Name
                            HAVING COUNT(o.Id) > 1
                            ORDER BY COUNT(o.Id)";

            List<string> bestSupplier = new List<string>();
            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                bestSupplier = connection.Query<string>(sql).ToList();
                Console.WriteLine("Supplier that sold most product: ");
                foreach (var product in bestSupplier)
                {
                    Console.WriteLine(product + " ");
                }
            }
        }
    }

   
}
