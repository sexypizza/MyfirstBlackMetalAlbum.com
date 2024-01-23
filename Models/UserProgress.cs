using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyfirstBlackMetalAlbum.com.Models
{
    public class UserProgress
    {
        [Key]
        public int UserProgressId { get; set; } // Primary key for UserProgress table

        [Required]
        [ForeignKey("User")]
        [Column("UserIntId")]
        public int UserIntId { get; set; } // Foreign key pointing to Users table

        [Required]
        [ForeignKey("Lesson")]
        [Column("LessonId")]
        public int LessonId { get; set; } // Foreign key pointing to Lesson table

        [Required]
        [Column("IsCompleted")]

        public bool IsCompleted { get; set; } // Flag indicating if the lesson is completed

        public Users User { get; set; } // Navigation property to User model

        public Lesson Lesson { get; set; } // Navigation property to Lesson model
    }
}
