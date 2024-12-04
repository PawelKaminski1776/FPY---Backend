using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using BowlingSys.Contracts.UserDtos;
using BowlingSys.Entities.UserDBEntities;
using System.Threading.Tasks;

namespace BowlingSys.Process.Controllers
{
    [ApiController]
    [Route("Api/UserDetails")]
    public class BowlingSysController : BaseController
    {
        public BowlingSysController(IMessageSession messageSession) : base(messageSession)
        {
        }

        [HttpGet("CheckLogin")]
        public async Task<IActionResult> CheckLogin(string? username, string? email, string? password)
        {

            LoginDto loginDto = new LoginDto
            {
                Email = email,
                Username = username,
                Password = password
            };
            try
            {
                var response = await _messageSession.Request<GetUserIDResult>(loginDto);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }


        [HttpGet("CheckExistingUser")]
        public async Task<IActionResult> CheckExistingUser(string? username, string? email)
        {

            UserExistsDto loginDto = new UserExistsDto
            {
                Email = email,
                Username = username,
            };
            try
            {
                var response = await _messageSession.Request<ErrorMessage>(loginDto);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }

        [HttpPost("AddNewAccount")]
        public async Task<IActionResult> AddAccount(UserCreationDto dto)
        {
            
            try
            {
                var response = await _messageSession.Request<ErrorMessage>(dto);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }
}
