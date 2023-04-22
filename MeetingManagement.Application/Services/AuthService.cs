﻿using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Exceptions;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MeetingManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task SignInUser(SignInUserDTO userCredentials)
        {
            var user = await _userRepository.GetUserByEmail(userCredentials.Email);
            var isPasswordCorrect = CheckPasswordHash(userCredentials.Password, user.PasswordSalt, user.PasswordHash);

            if (!isPasswordCorrect || user == null)
            {
                throw new UserInvalidCredentialsException();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.UserData, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                // add claim for team administrator
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow
            };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task SignOutUser()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private static bool CheckPasswordHash(string inputPassword, string salt, string hashedPassword)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);

            string inputHash = Convert.ToBase64String(KeyDerivation
                .Pbkdf2(inputPassword, saltBytes, KeyDerivationPrf.HMACSHA256, 100000, 32));

            return inputHash.Equals(hashedPassword);
        }
    }
}
