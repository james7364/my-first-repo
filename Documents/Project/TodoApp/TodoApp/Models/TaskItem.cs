using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public required string Title { get; set; }
        
        public string? Description { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        // Priority levels: 1 = Low, 2 = Medium, 3 = High
        public int Priority { get; set; } = 1; 
        
        // Hex code for color-coding priority cards
        public string ColorHex { get; set; } = "#ffffff";
        
        // Connects the task to the authenticated user's ID
        [Required]
        public required string UserId { get; set; }

        // Add this property
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
    }
}