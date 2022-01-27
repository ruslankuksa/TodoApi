using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Services;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController(UserService userService) =>
            this.userService = userService;

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Login(LoginModel model)
        {
            var registeredUser = await userService.Authenticate(model.Email, model.Password);

            if (registeredUser == null)
            {
                return NotFound();
            }

            var token = userService.generateJWToken(registeredUser.Email);
            registeredUser.setToken(token);

            return registeredUser;
        }

        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register(User newUser)
        {
            await userService.Create(newUser);
            var dtoUser = new UserDTO(newUser);
            var token = userService.generateJWToken(newUser.Email);
            dtoUser.setToken(token);

            return CreatedAtAction(nameof(Login), new { id = newUser.Id }, dtoUser);
        }
    }
}

