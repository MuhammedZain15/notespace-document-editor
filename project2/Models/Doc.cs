using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project2.Models
{
    public class Doc
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }

    }
}
