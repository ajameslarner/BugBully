using System.ComponentModel.DataAnnotations;

namespace BugBully.Models
{
    public class Statuses
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}