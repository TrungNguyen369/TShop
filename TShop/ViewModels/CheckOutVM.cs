namespace TShop.ViewModels
{
    public class CheckOutVM
    {
        public int IdUser { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
        public float GrandTotal => (float)cartItems.Sum(x => x.TotalPrice);
    }
}
