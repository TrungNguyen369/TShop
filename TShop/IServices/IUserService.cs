﻿using TShop.Models;
using TShop.ViewComponents;
using TShop.ViewModels;

namespace TShop.IServices
{
    public interface IUserService
    {
        Customer UserRegister(UserVM userVM, bool gender);
        Task<UserVM> UserLoginAsync(LoginVM loginVM);
        UserVM UserPasswordChange(PasswordVM passwordVM, string emailUser);
        Customer GetUserByEmail(string email);
    }
}
