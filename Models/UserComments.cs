using MyfirstBlackMetalAlbum.com.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// UserComments model with a UserName property
public class UserComments
{
    [Key]
    public int CommentId { get; set; } // Primary key

    [Required]
    public string Text { get; set; }

    [Required]
    public int LessonId { get; set; } // Lesson ID to specify which lesson the comment is for

    [ForeignKey("User")]
    public int UserIntId { get; set; } // This should correspond to the primary key of the Users table

    //[ForeignKey("User")]
    public string UserName { get; set; } // Username of the commenter

    public DateTime CreatedAt { get; set; } // Timestamp for comment creation


    // Navigation property to access the User model
    public Users User { get; set; }
}
