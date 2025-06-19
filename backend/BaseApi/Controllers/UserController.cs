using AutoMapper;
using Domain.DTOs;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercadoFacilAPI.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;        

        public UserController(
            IUserService userService,  
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpGet(Name = "GetAllUsers")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsers();
            if (users == null)
                return NotFound();
            return Ok(users);
        }
        [AllowAnonymous]
        [HttpPost(Name = "AddUser")]
        public async Task<IActionResult> Post([FromBody] CreateUserDTO userDto)
        {              
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UserConversionResultDTO userConversionResultDTO = ConvertCreateUserDTOToUser(userDto);

            if(userConversionResultDTO.user != null)
            {
                await _userService.AddUser(userConversionResultDTO.user);   
                var toReturn = Ok(ConvertUserToCreateUserDTO(userConversionResultDTO.user));
                return toReturn;
            }
            return BadRequest("Um problema ocorreu ao salvar o usuário.");
        }

        [HttpPut(Name = "UpdateUser")]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            var existingUser = await _userService.GetUserById(user.Id);
            if (existingUser == null)
                return NotFound();

            await _userService.UpdateUser(user);
            return Ok(user);
        }

        [HttpDelete(Name = "DeleteUser")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            await _userService.DeleteUser(user);
            
            return Ok(user);
        }

        #region métodos auxiliares
        private UserConversionResultDTO ConvertCreateUserDTOToUser(CreateUserDTO createUserDTO)
        {
            UserConversionResultDTO userConversionResultDTO = new UserConversionResultDTO();
            User user = new User();
            
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.Email = createUserDTO.Email;
            user.Name = createUserDTO.Name;
            user.Password = createUserDTO.Password;
            user.Role = createUserDTO.Role;
            
            userConversionResultDTO.user = user;           
            return userConversionResultDTO;
        }

        private CreateUserDTO ConvertUserToCreateUserDTO(User user)
        {
            CreateUserDTO userDto = new CreateUserDTO();
            userDto.Name = user.Name;
            userDto.Email = user.Email;
            userDto.Password = user.Password;
            userDto.Role = user.Role;
            
            return userDto;
        }     
        #endregion
    }
}