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
    public class CheckUserController : BaseController
    {
        public CheckUserController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpGet("CheckExistingUser")]
        public async Task<IActionResult> CheckExistingUser(string username, string email)
        {
            var userExistsDto = (UserExistsDto)_dtoFactory.CreateDto("userexistsdto", username, email);

            try
            {
                var response = await _messageSession.Request<ErrorMessage>(userExistsDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}

