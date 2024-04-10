using TShop.Models;
using TShop.ViewComponents;
using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IUserService
    {
        Customer UserRegister(UserVM userVM, bool gender);
        UserVM UserLogin(LoginVM loginVM);
    }
}
