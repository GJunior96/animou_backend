using Animou.Business.Interfaces;
using Animou.Business.Models;
using Animou.Business.Validations;

namespace Animou.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRelationsService _relationsService;

        public UserService(IUserRepository userRepository, INotifier notifier, 
                            IRelationsService relationsService) 
            : base(notifier)
        {
            _userRepository = userRepository;
            _relationsService = relationsService;
        }

        public async Task Add(User user)
        {
            if (!RunValidation(new UserValidation(), user)) return;

            var usedEmail = await _userRepository.GetUserByEmail(user.Email!.Address!);
            var usedName = await _userRepository.GetUserByName(user.Name!);

            if (usedEmail != null)
            {
                Notify("emailTaken");
                return;
            }

            if (usedName != null)
            {
                Notify("nameTaken");
                return;
            }

            _userRepository.Add(user);
            await SaveData(_userRepository.UnitOfWork);
        }

        public async Task Delete(Guid id)
        {
            await _relationsService.DeleteUserRelations(id);
            _userRepository.Delete(id);
            await SaveData(_userRepository.UnitOfWork);
        }

        public async Task Update(User user)
        {
            if (_userRepository.GetCustomized(u => u.Email!.Address == user.Email!.Address && u.Id != user.Id).Result.Any())
            {
                Notify("emailTaken");
                return;
            }

            if (_userRepository.GetCustomized(u => u.Name == user.Name && u.Id != user.Id).Result.Any())
            {
                Notify("nameTaken");
                return;
            }

            _userRepository.Update(user);
            await SaveData(_userRepository.UnitOfWork);
        }

        public void Dispose() => _userRepository?.Dispose();
    }

    public class CommentService : BaseService, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IDislikeRepository _dislikeRepository;
        private readonly IRelationsService _relationsService;

        public CommentService(ICommentRepository commentRepository, INotifier notifier, 
                                IUserRepository userRepository, ILikeRepository repository,
                                IDislikeRepository dislikeRepository, 
                                IRelationsService relationsService)
            : base(notifier)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _likeRepository = repository;
            _dislikeRepository = dislikeRepository;
            _relationsService = relationsService;
        }

        #region Comment
        public async Task AddComment(Comment comment)
        {
            if (!RunValidation(new CommentValidation(), comment)) return;

            _commentRepository.Add(comment);
            await SaveData(_commentRepository.UnitOfWork);
        }
        
        public async Task Update(Comment comment)
        {
            if (!RunValidation(new CommentValidation(), comment)) return;

            _commentRepository.Update(comment);
            await SaveData(_commentRepository.UnitOfWork);
        }

        public async Task DeleteComment(Guid id)
        {
            await _relationsService.DeleteCommentChildren(id);
            await _relationsService.DeleteCommentRelations(id);
            _commentRepository.Delete(id);
            await SaveData(_commentRepository.UnitOfWork);
        }

        #endregion Comment

        #region Like
        public async Task AddLike(Like like)
        {
            if (!RunValidation(new LikeValidation(), like)) return;

            var dislike = await _dislikeRepository.GetByCommentId(like.CommentId);
            if (dislike != null) _dislikeRepository.Delete(dislike.Id);

            _likeRepository.Add(like);
            await SaveData(_likeRepository.UnitOfWork);
        }

        public async Task DeleteLike(Guid id)
        {
            _likeRepository.Delete(id);
            await SaveData(_likeRepository.UnitOfWork);
        }

        #endregion Like

        #region Dislike
        public async Task AddDislike(Dislike dislike)
        {
            if (!RunValidation(new DislikeValidation(), dislike)) return;

            var like = await _likeRepository.GetByCommentId(dislike.CommentId);
            if (like != null) _likeRepository.Delete(like.Id);

            _dislikeRepository.Add(dislike);
            await SaveData(_dislikeRepository.UnitOfWork);
        }

        public async Task DeleteDislike(Guid id)
        {
            _dislikeRepository.Delete(id);
            await SaveData(_dislikeRepository.UnitOfWork);
        }

        #endregion Dislike

        public void Dispose()
        {
            _userRepository?.Dispose();
            _commentRepository?.Dispose();
            _likeRepository?.Dispose();
            _dislikeRepository?.Dispose();
        }
    }

    public class RelationsService : BaseService, IRelationsService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IDislikeRepository _dislikeRepository;
        private readonly ILikeRepository _likeRepository;

        public RelationsService(ICommentRepository commentRepository, 
                                IDislikeRepository dislikeRepository, 
                                ILikeRepository likeRepository, INotifier notifier) : base(notifier)
        {
            _commentRepository = commentRepository;
            _dislikeRepository = dislikeRepository;
            _likeRepository = likeRepository;
        }

        public async Task DeleteCommentChildren(Guid id)
        {
            var children = await _commentRepository.GetCustomized(_ => _.ParentId == id);

            foreach (var child in children)
            {
                await DeleteCommentRelations(child.Id);
                await DeleteCommentChildren(child.Id);
                _commentRepository.Delete(child.Id);
                await SaveData(_commentRepository.UnitOfWork);
            }
        }

        public async Task DeleteCommentRelations(Guid id)
        {
            await _likeRepository.DeleteFromQuery(_ => _.CommentId == id);
            await _dislikeRepository.DeleteFromQuery(_ => _.CommentId == id);
        }

        public async Task DeleteUserRelations(Guid id)
        {
            await _likeRepository.DeleteFromQuery(_ => _.UserId == id);
            await _dislikeRepository.DeleteFromQuery(_ => _.UserId == id);

            var comments = await _commentRepository.GetCustomized(_ => _.UserId == id);

            foreach (var comment in comments)
            {
                await DeleteCommentChildren(comment.Id);
                await DeleteCommentRelations(comment.Id);
                _commentRepository.Delete(comment.Id);
                await SaveData(_commentRepository.UnitOfWork);
            }
        }

        public void Dispose()
        {
            _commentRepository?.Dispose();
            _likeRepository?.Dispose();
            _dislikeRepository?.Dispose();
        }
    }
}
