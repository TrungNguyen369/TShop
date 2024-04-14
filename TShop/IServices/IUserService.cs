using TShop.Models;
using TShop.ViewComponents;
using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IUserService
    {
        /// <summary>
        /// Handle registration of user
        /// </summary>
        /// <param name="userVM"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        Customer UserRegister(UserVM userVM, bool gender);

        /// <summary>
        /// Hanle login user
        /// </summary>
        /// <param name="loginVM"></param>
        /// <returns></returns>
        Task<UserVM> UserLoginAsync(LoginVM loginVM);

        /// <summary>
        /// Hanle passwor change
        /// </summary>
        /// <param name="passwordVM"></param>
        /// <param name="emailUser"></param>
        /// <returns></returns>
        UserVM UserPasswordChange(PasswordVM passwordVM, string emailUser);

        /// <summary>
        /// Get information user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Customer GetUserByEmail(string email);
    }
}
