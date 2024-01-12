namespace GroupBWebshop.Models
{
    internal class MyEnums
    {
        public enum EnumSize
        {
            XS = 1,
            S,
            M,
            L,
            XL
        }
        public enum Material
        {
            Cotton,
            Cashmere,
            Polyester,
            Leather,
            Wool
        }
        public enum PaymentMethod
        {
            Klarna = 1,
            Credit_Card,
            PayPal,
            Swish
        }

        public enum DeliveryMethod
        {
            DHL = 1,
            Postnord,
            Schenker
        }
    }
}
