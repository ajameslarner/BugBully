using System;
using System.ComponentModel.DataAnnotations;

namespace BugBully.Models
{
    public class Bugs
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateReported { get; set; }
        [Required]
        public int StatusId { get; set; }
        public virtual Statuses Status { get; set; }
        [Required]
        public int UserId { get; set; }
        public virtual Users User { get; set; }
    }
}