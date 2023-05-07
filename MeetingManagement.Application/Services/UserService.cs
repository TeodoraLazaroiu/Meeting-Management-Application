﻿using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Exceptions;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace MeetingManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        public UserService(IUserRepository userRepository, ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
        }

        public async Task<List<UserEntity>> GetUserList()
        {
            return (await _userRepository.GetAllAsync()).ToList();
        }

        public async Task<UserEntity> GetUserEntity(string id)
        {
            try
            {
                var user = await _userRepository.GetAsync(id);

                return user ?? throw new UserNotFoundException();
            }
            catch (Exception)
            {
                throw new UserNotFoundException();
            }
        }

        public async Task<string> RegisterUser(RegisterUserDTO registerUser)
        {
            var existingUser = await _userRepository.GetUserByEmail(registerUser.Email);

            if (existingUser != null)
            {
                throw new UserAlreadyExistsException();
            }

            var user = new UserEntity
            {
                Email = registerUser.Email,
                FirstName = registerUser.FirstName,
                LastName = registerUser.LastName,
                RoleTitle = registerUser.RoleTitle
            };

            if (registerUser.TeamId != null)
            {
                try
                {
                    await _teamRepository.GetAsync(registerUser.TeamId);
                }
                catch
                {
                    throw new TeamNotFoundException();
                }

                user.TeamId = new Guid(registerUser.TeamId);
            }

            var hashResult = HashPassword(registerUser.Password);
            user.PasswordHash = hashResult.Item1;
            user.PasswordSalt = hashResult.Item2;

            user.Id = Guid.NewGuid();
            user.CreatedDate = DateTime.UtcNow;
            user.LastModified = DateTime.UtcNow;

            await _userRepository.CreateAsync(user);

            return user.Id.ToString();
        }

        public async Task UpdateUser(string id, UpdateUserDTO updateUser)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;
            user.RoleTitle = updateUser.RoleTitle;

            user.LastModified = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userRepository.GetAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            await _userRepository.DeleteAsync(id);
        }

        private static (string, string) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(saltBytes);
            }

            string hashedPass = Convert.ToBase64String(KeyDerivation
                .Pbkdf2(password, saltBytes, KeyDerivationPrf.HMACSHA256, 100000, 32));
            var salt = Convert.ToBase64String(saltBytes);

            return (hashedPass, salt);
        }
    }
}
