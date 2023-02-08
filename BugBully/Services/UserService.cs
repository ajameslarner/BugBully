using BugBully.Data;
using System.Security.Cryptography;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BugBully.Models;
using System.Data.Entity;
using System.Linq;

namespace BugBully.Services
{
    public class UserService
    {
        private readonly IRepository _context;

        public UserService(IRepository repository)
        {
            _context = repository;
        }

        public bool Authenticate(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user == null) return false;

            return password == user.Password;
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public static bool VerifyPasswordHash(string password, string storedHash)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var passwordHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return passwordHash == storedHash;
            }
        }

        public List<Users> GetUsers()
        {
            return _context.Users.ToList();
        }
        public Users GetUserByUsername(string username)
        {
            return _context.Users.Where(u => u.Username == username).FirstOrDefault();
        }
        public Users GetUserById(int? id)
        {
            return _context.Users.Where(b => b.Id == id).FirstOrDefault();
        }
        public bool AddUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                _context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateUser(Users user)
        {
            try
            {
                _context.SaveEntry(user);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteUser(Users user)
        {
            try
            {
                _context.Users.Remove(user);
                _context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Async Methods
        public async Task<List<Users>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<Users> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<Users> GetUserByIdAsync(int? id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<bool> AddUserAsync(Users user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserAsync(Users user)
        {
            try
            {
                return await _context.SaveEntryAsync(user);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> DeleteUserAsync(Users user)
        {
            try
            {
                _context.Users.Remove(user);
                await _context.SaveAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}