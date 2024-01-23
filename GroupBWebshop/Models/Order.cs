namespace GroupBWebshop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool Completed { get; set; }
        public string? Payment { get; set; }

        public string? Delivery { get; set; }

    }
}
