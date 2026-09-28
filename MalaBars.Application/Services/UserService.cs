using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MalaBars.Application.Services
{
    public class UserService :IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(
            IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<UserDto> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("Email Already Exists!!");
            }
            else
            {
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Role = "User",
                    IsBlocked = false
                };
                user.PasswordHash = _passwordHasher.HashPassword(user,request.Password);

                var createdUser = await _userRepository.AddAsync(user);

                return new UserDto
                {

                    UserId = createdUser.UserId,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Role = createdUser.Role,
                    IsBlocked = createdUser.IsBlocked
                };

            }
        }

        public async Task<string?> LoginAsync(LoginDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                return null;
            }
            else
            {
                var result = _passwordHasher.VerifyHashedPassword(
                    user,
                    user.PasswordHash,
                    request.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    return null;
                }
                if(user.IsBlocked)
                {
                    return null;
                }
            }
                return _tokenService.GenerateToken(user);
        }
        

    }
}
