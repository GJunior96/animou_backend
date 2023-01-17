namespace Animou.API.DTOs
{
    public class LikeDislikeDTO
    {
        public Guid Id { get; set; }
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
    }
}
