using Animou.Business.Models;

namespace Animou.Business.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByName(string name);
        Task<User?> GetUserRelationsById(Guid id);
    }

    public interface ICommentRepository : IRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByMediaId(int id);
    }

    public interface ILikeRepository : IRepository<Like>
    {
        Task<Like?> GetByCommentId(Guid id);
    }

    public interface IDislikeRepository : IRepository<Dislike>
    {
        Task<Dislike?> GetByCommentId(Guid id);
    }
}
