using Animou.API.DTOs;
using Animou.Business.Interfaces;
using Animou.Business.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Animou.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : MainController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, 
                                IMapper mapper, IUserService userService,
                                IStringLocalizer<SharedResource> localizer, INotifier notifier) 
            : base(notifier, localizer)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers() => 
            CustomResponse(_mapper.Map<IEnumerable<UserDTO>>(await _userRepository.GetAll()));
        

        [HttpGet("{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var user = _mapper.Map<UserDTO>(await _userRepository.GetById(id));
            if (user == null)
            {
                NotifyError("userNotFound");
                return CustomResponse();
            }
            return CustomResponse(user);
        }

        [HttpPost("add")]
        public async Task<ActionResult> AddUser(UserDTO dto)
        {
            await _userService.Add(_mapper.Map<User>(dto));

            return CustomResponse(dto);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateUser(UserDTO dto)
        {
            if (GetUserById(dto.Id) == null) return NotFound();

            await _userService.Update(_mapper.Map<User>(dto));

            return CustomResponse(dto);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            if (GetUserById(id).Result == null) return NotFound();

            await _userService.Delete(id);

            return CustomResponse();
        }

        private async Task<UserDTO?> GetUserById(Guid id) =>
            _mapper.Map<UserDTO>(await _userRepository.GetById(id));

    }
}
