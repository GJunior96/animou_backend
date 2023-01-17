using System.ComponentModel.DataAnnotations;

namespace Animou.API.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? EmailAddress { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }

        public IEnumerable<CommentDTO>? Comments { get; set; }
    }
}
