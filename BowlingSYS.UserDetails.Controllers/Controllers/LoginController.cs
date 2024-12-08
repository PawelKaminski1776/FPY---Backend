using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using BowlingSys.Contracts.UserDtos;
using BowlingSys.Entities.UserDBEntities;
using System.Threading.Tasks;
using BowlingSYS.UserDetails.Controllers.DtoFactory;

namespace BowlingSys.Process.Controllers
{
    [ApiController]
    [Route("Api/UserDetails")]
    public class LoginController : BaseController
    {
        public LoginController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpGet("CheckLogin")]
        public async Task<IActionResult> CheckLogin(string? username, string? email, string password)
        {
            var loginDto = (LoginDto)_dtoFactory.CreateDto("logindto", username, email, password);

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
    }

}
