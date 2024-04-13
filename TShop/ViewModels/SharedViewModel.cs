using TShop.ViewComponents;

namespace TShop.ViewModels
{
    public class SharedViewModel
    {
        public UserVM UserVM { get; set; } = new UserVM();
        public LoginVM LoginVM { get; set; } = new LoginVM();
        public PasswordVM PasswordVM { get; set; } = new PasswordVM();
    }
}
