using System.Text.RegularExpressions;

namespace Animou.Business.Models
{
    public class User : Entity
    {
        public string? Name { get; set; }
        public Email? Email { get; set; }
        public string? Image { get; set; }
        public IEnumerable<Media>? Medias { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
        public IEnumerable<Like>? Likes { get; set; }
        public IEnumerable<Dislike>? Dislikes { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        public User() { }

        public User(string name, string email)
        {
            Name = name;
            Email = new Email(email);
            IsDeleted = false;
        }

        public User(Guid id, string? name, string? email)
        {
            Name = name;
            Email = new Email(email);
            IsDeleted = true;
            Image = null;
        }
    }

    public class Email
    {
        public string? Address { get; set; }

        protected Email() { }

        public Email(string? address)
        {
            if (!Validate(address)) throw new Exception("ERROU");
            Address = address;
        }

        public static bool Validate(string email)
        {
            var regexEmail = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            return email != null
                ? regexEmail.IsMatch(email)
                : true;
        }
    }

    public class Media : Entity
    {
        public Guid UserId { get; set; }
        public int MediaId { get; set; }
        public Status Status { get; set; }
        public decimal Score { get; set; }
        public User? User { get; set; }
    }

    public enum Status
    {
        WHATCHING, PLANNING, COMPLETED, DROPPED
    }

    public class Comment : Entity
    {
        public Guid? UserId { get; set; }
        public Guid? ParentUserId { get; set; }
        public Guid? ParentId { get; set; }
        public int MediaId { get; set; }
        public string? Text { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual IEnumerable<Like>? Likes { get; set; }
        public virtual IEnumerable<Dislike>? Dislikes { get; set; }
        public virtual Comment? Parent { get; set; }
        public virtual IEnumerable<Comment>? Replies { get; set; }
        public virtual User? User { get; set; }
        //public virtual User? ParentUser { get; set; }

        public Comment() => Replies = new HashSet<Comment>(); 

        public Comment(Guid id, string? text, int mediaId, Guid? userId, 
                        Guid? parentUserID, Guid? parentId)
        {
            Id = id;
            Text = text;
            MediaId = mediaId;
            IsDeleted = false;
            UserId = userId;
            ParentUserId = parentUserID;
            ParentId = parentId;
            Replies = new HashSet<Comment>();
        }

        public Comment(Guid id, string? text)
        {
            Id = id; 
            Text = text;
            IsDeleted = true;
        }
    }

    public class Like : Entity
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public Comment? Comment { get; set; }
        public User? User { get; set; }
    }

    public class Dislike : Entity
    {
        public Guid CommentId { get; set; }
        public Guid UserId { get; set; }
        public Comment? Comment { get; set; }
        public User? User { get; set; }
    }
}
