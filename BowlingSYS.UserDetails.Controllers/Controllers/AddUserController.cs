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
    public class AddUserController : BaseController
    {
        public AddUserController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpPost("AddNewAccount")]
        public async Task<IActionResult> AddAccount([FromBody] UserCreationDto dto)
        {
            var userCreationDto = (UserCreationDto)_dtoFactory.UseDto("usercreationdto", dto);

            try
            {
                var response = await _messageSession.Request<ErrorMessage>(userCreationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
