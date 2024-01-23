using System;
using System.ComponentModel.DataAnnotations;

namespace MyfirstBlackMetalAlbum.com.Models
{
    public class Lesson
    {

        [Key]
        public int LessonId { get; set; }
        public string Content { get; set; }
        public int Duration { get; set; }
        [Required]
        public string Title { get; set; }
        public string DifficultyLevel { get; set; }

        // Include other properties as needed for your application

        // You can also include navigation properties if necessary
        // public ICollection<UserComment> Comments { get; set; }
    }
}
