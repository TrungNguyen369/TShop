namespace TShop.ViewModels
{
    public class CartItem
    {
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string image { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice => Price * Quantity;
    }
}
