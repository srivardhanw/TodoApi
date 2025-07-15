using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItem
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string? Value { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
