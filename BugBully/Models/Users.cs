using BugBully.Services;
using System.ComponentModel.DataAnnotations;

namespace BugBully.Models
{
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Username { get; set; }
        private string _password;
        [Required]
        public string Password
        {
            get => _password;
            set => _password = UserService.HashPassword(value);
        }
    }
}