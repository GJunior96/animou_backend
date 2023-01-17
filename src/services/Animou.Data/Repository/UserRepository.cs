using Animou.Business.Interfaces;
using Animou.Business.Models;
using Animou.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Animou.Data.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AnimouContext context) : base(context)
        { }
        
        public async Task<User?> GetUserByEmail(string email) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(_ => _.Email!.Address == email);

        public async Task<User?> GetUserByName(string name) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(_ => _.Name == name);

    }

    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(AnimouContext context) : base(context)
        { }

        public async Task<IEnumerable<Comment>> GetCommentsByMediaId(int id) =>
            await _context.Comments.AsNoTracking().Where(_ => _.MediaId == id).ToListAsync();
    }

    public class LikeRepository : Repository<Like>, ILikeRepository
    {
        public LikeRepository(AnimouContext context) : base(context)
        { }

        public async Task<Like?> GetByCommentId(Guid id) =>
            await _context.Likes.AsNoTracking().FirstOrDefaultAsync(_ => _.CommentId.Equals(id));
    }

    public class DislikeRepository : Repository<Dislike>, IDislikeRepository
    {
        public DislikeRepository(AnimouContext context) : base(context)
        { }

        public async Task<Dislike?> GetByCommentId(Guid id) =>
            await _context.Dislikes.AsNoTracking().FirstOrDefaultAsync(_ => _.CommentId.Equals(id));
    }
}
