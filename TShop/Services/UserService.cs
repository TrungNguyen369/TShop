using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TShop.Contants;
using TShop.Helpers;
using TShop.IServices;
using TShop.Models;
using TShop.ViewComponents;
using TShop.ViewModels;

namespace TShop.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly TshopContext _context;

        public UserService(IMapper mapper, TshopContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public Customer GetUserByEmail(string email)
        {
            Customer user = null;

            user = _context.Customers.FirstOrDefault(x => x.Email == email);

            return user;
        }

        public async Task<UserVM> UserLoginAsync(LoginVM loginVM)
        {
            UserVM userVM = null;
            try
            {
                var user = _context.Customers.FirstOrDefault(x => x.Email == loginVM.UserName);

                if (user == null)
                {
                    Console.WriteLine("Not found UserName or Email");
                    return userVM;
                }

                if (!user.Effect)
                {
                    Console.WriteLine("This account has been locked");
                    return userVM;
                }

                if (user.PassWord != loginVM.Password.ToMd5Hash(user.RandomKey))
                {
                    Console.WriteLine("Wrong login information");
                    return userVM;
                }

                userVM = _mapper.Map<UserVM>(user);
                userVM.Name = user.FullName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return userVM;
        }

        public UserVM UserPasswordChange(PasswordVM passwordVM, string emailUser)
        {
            UserVM userVM = null;

            try
            {
                var user = _context.Customers.FirstOrDefault(x => x.Email == emailUser);

                if (user == null)
                {
                    Console.WriteLine("Not found UserName or Email");

                    return userVM;
                }

                if (user.PassWord == passwordVM.CurrentPassword.ToMd5Hash(user.RandomKey))
                {
                    user.PassWord = passwordVM.NewPassword.ToMd5Hash(user.RandomKey);

                    userVM = _mapper.Map<UserVM>(user);
                    userVM.Name = user.FullName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return userVM;
        }

        public Customer UserRegister(UserVM userVM, bool gender)
        {
            var user = _mapper.Map<Customer>(userVM);
            try
            {
                user.FullName = userVM.Name;
                user.Sex = gender;
                user.RandomKey = Utils.GenerateRamdomKey();
                user.PassWord = userVM.Password.ToMd5Hash(user.RandomKey);
                user.Effect = true;

                _context.Add(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return user;
        }
    }
}
