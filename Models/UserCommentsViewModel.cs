using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyfirstBlackMetalAlbum.com.Models
{
    public class UserCommentsViewModel
    {
        public UserCommentsViewModel()
        {
            Comments = new List<CommentViewModel>(); // Initialize the Comments list in the constructor
        }

        public List<CommentViewModel> Comments { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public int LessonId { get; set; } // Lesson ID to specify which lesson the comment is for

        public string UserName { get; set; } // Username of the commenter
        public UserProgress UserProgress { get; set; } // UserProgress property to represent the user's progress
        public Users Users { get; set; } // UserProgress property to represent the user's progress
        public byte[] ProfilePicture { get; set; } // Add this property for the thumbnail image
        public DateTime CreatedAt { get; set; } // Set the creation date to the current date and time

    }

    public class CommentViewModel
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public byte[] ProfilePicture { get; set; } // Add this property for the thumbnail image
        public DateTime CreatedAt { get; set; }
    }
}
