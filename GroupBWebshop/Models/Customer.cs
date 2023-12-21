using System.ComponentModel.DataAnnotations;

namespace GroupBWebshop.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        [MaxLength(100)]
        public string StreetName { get; set; }
        [MaxLength(100)]
        public string PostalCode { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}
