using Dapper;
using GroupBWebshop.Models;
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
        static string connString = "data source=.\\SQLEXPRESS05; initial catalog = FashionFusion; persist security info = True; Integrated Security = True;";
        public static void BestSelling()
        {
            string sql = @"SELECT Top 5 p.Name, SUM(od.Quantity) as [Top Seller] FROM OrderDetails od JOIN Products p on od.ProductId = p.Id
             GROUP BY p.Name ORDER BY [Top Seller] DESC";

            List <string> bestSelling = new List <string>();
            using(var connection = new SqlConnection(connString))
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

    }

   
}
