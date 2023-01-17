namespace Animou.API.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? ParentUserId { get; set; }
        public Guid? ParentId { get; set; }
        public int MediaId { get; set; }
        public string? Text { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
