using Animou.API.DTOs;
using Animou.Business.Interfaces;
using Animou.Business.Models;
using Animou.Data.Context;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Animou.API.Controllers
{
    [Route("api/[controller]")]
    public class CommentsController : MainController
    {
        private readonly IMapper _mapper;
        private readonly ICommentRepository _commentRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IDislikeRepository _dislikeRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentService _commentService;

        public CommentsController(IMapper mapper, ICommentRepository commentRepository,
                                    INotifier notifier, IStringLocalizer<SharedResource> localizer,
                                    ICommentService commentService, ILikeRepository likeRepository,
                                    IDislikeRepository dislikeRepository, IUserRepository userRepository) 
            : base(notifier, localizer)
        {
            _mapper = mapper;
            _commentRepository = commentRepository;
            _commentService = commentService;
            _likeRepository = likeRepository;
            _dislikeRepository = dislikeRepository;
            _userRepository = userRepository;
        }

        [HttpGet("/comment/all")]
        public async Task<ActionResult> GetAll() =>
            CustomResponse(_mapper.Map<IEnumerable<CommentDTO>>(await _commentRepository.GetAll()));

        [HttpGet("/comment/media/{id:int}")]
        public async Task<ActionResult> GetByMediaId(int id) =>
            CustomResponse(_mapper.Map<IEnumerable<CommentDTO>>(await _commentRepository.GetCommentsByMediaId(id)));

        [HttpPost("/comment/add")]
        public async Task<ActionResult> Add(CommentDTO dto)
        {
            if (GetUserById(dto.UserId).Result == null) return NotFound();

            await _commentService.AddComment(_mapper.Map<Comment>(dto));
            return CustomResponse(dto);
        }

        [HttpPut("/comment/update")]
        public async Task<ActionResult> Update(CommentDTO dto)
        {
            if (GetCommentById(dto.Id).Result == null) return NotFound();

            await _commentService.Update(_mapper.Map<Comment>(dto));
            return CustomResponse(dto);
        }

        [HttpDelete("/comment/delete")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (GetCommentById(id).Result == null) return NotFound();

            await _commentService.DeleteComment(id);
            return CustomResponse();
        }

        [HttpGet("/like/all")]
        public async Task<ActionResult> GetAllLikes() =>
            CustomResponse(_mapper.Map<IEnumerable<LikeDislikeDTO>>(await _likeRepository.GetAll()));

        [HttpPost("/like/add")]
        public async Task<ActionResult> AddLike(LikeDislikeDTO dto)
        {
            if (GetUserById(dto.UserId).Result == null && GetCommentById(dto.CommentId).Result == null)
                return NotFound();

            await _commentService.AddLike(_mapper.Map<Like>(dto));
            return CustomResponse(dto);
        }

        [HttpDelete("/like/delete")]
        public async Task<ActionResult> DeleteLike(Guid id)
        {
            if (GetLikeById(id).Result == null) return NotFound(id);

            await _commentService.DeleteLike(id);
            return CustomResponse();
        }

        [HttpGet("/dislike/all")]
        public async Task<ActionResult> GetAllDislikes() =>
            CustomResponse(_mapper.Map<IEnumerable<LikeDislikeDTO>>(await _dislikeRepository.GetAll()));

        [HttpPost("/dislike/add")]
        public async Task<ActionResult> AddDislike(LikeDislikeDTO dto)
        {
            if (GetUserById(dto.UserId).Result == null && GetCommentById(dto.CommentId).Result == null)
                return NotFound();

            await _commentService.AddDislike(_mapper.Map<Dislike>(dto));
            return CustomResponse(dto);
        }

        [HttpDelete("/dislike/delete")]
        public async Task<ActionResult> DeleteDislike(Guid id)
        {
            if (GetDislikeById(id).Result == null) return NotFound();

            await _commentService.DeleteDislike(id);
            return CustomResponse();
        }

        private async Task<UserDTO?> GetUserById(Guid id) =>
            _mapper.Map<UserDTO>(await _userRepository.GetById(id));

        private async Task<CommentDTO?> GetCommentById(Guid id) =>
            _mapper.Map<CommentDTO>(await _commentRepository.GetById(id));

        private async Task<LikeDislikeDTO?> GetLikeById(Guid id) =>
            _mapper.Map<LikeDislikeDTO>(await _likeRepository.GetById(id));

        private async Task<LikeDislikeDTO?> GetDislikeById(Guid id) =>
            _mapper.Map<LikeDislikeDTO>(await _dislikeRepository.GetById(id));
    }
}
