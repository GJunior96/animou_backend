using Animou.Business.Models;

namespace Animou.Business.Interfaces
{
    public interface IUserService
    {
        Task Add(User user);
        Task Update(User user);
        Task Delete(Guid id);
    }

    public interface ICommentService
    {
        Task AddComment(Comment comment);
        Task Update(Comment comment);
        Task DeleteComment(Guid id);
        Task AddLike(Like comment);
        Task DeleteLike(Guid id);
        Task AddDislike(Dislike comment);
        Task DeleteDislike(Guid id);
    }

    public interface IRelationsService
    {
        Task DeleteUserRelations(Guid id);
        Task DeleteCommentRelations(Guid id);
        Task DeleteCommentChildren(Guid id);
    }
}
