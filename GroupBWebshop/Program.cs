using GroupBWebshop.Models;
using System;
using System.Security.Cryptography;

namespace GroupBWebshop
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomerView.LoginOrAdmin();
        }
    }
}