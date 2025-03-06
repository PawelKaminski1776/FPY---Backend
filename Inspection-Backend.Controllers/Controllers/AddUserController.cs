using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System.Threading.Tasks;
using InspectionBackend.UserDetails.Controllers.DtoFactory;

namespace InspectionBackend.Controllers
{
    [ApiController]
    [Route("Api/UserDetails")]
    public class AddUserController : BaseController
    {
        public AddUserController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpPost("AddNewAccount")]
        public async Task<IActionResult> AddAccount([FromBody] UserCreationRequest dto)
        {
            var userCreationDto = (UserCreationRequest)_dtoFactory.UseDto("usercreationdto", dto);

            try
            {
                //var response = await _messageSession.Request<>(userCreationDto);
                //return Ok(response);
                return Ok(userCreationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
