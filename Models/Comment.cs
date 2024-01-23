namespace MyfirstBlackMetalAlbum.com.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }

        // Navigation property to associate comments with users
        public Users User { get; set; }
    }

}
