using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System.Threading.Tasks;
using InspectionBackend.UserDetails.Controllers.DtoFactory;

namespace InspectionBackend.Controllers.UserControllers
{
    [ApiController]
    [Route("Api/Accounts")]
    public class LoginController : BaseController
    {
        public LoginController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpPost("Login")]
        public async Task<IActionResult> AddAccount([FromBody] LoginRequest dto)
        {
            var loginDto = (LoginRequest)_dtoFactory.UseDto("logindto", dto);

            try
            {
                var response = await _messageSession.Request<LoginResponse>(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
