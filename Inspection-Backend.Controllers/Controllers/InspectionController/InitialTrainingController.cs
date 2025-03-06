using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System.Threading.Tasks;
using InspectionBackend.UserDetails.Controllers.DtoFactory;
using InspectionBackend.Contracts.InspectionDtos;

namespace InspectionBackend.Controllers.UserControllers
{
    [ApiController]
    [Route("Api/Training")]
    public class TrainingController : BaseController
    {
        public TrainingController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpPost("AddInspectionPattern")]
        public async Task<IActionResult> AddAccount([FromBody] InspectionRequest dto)
        {
            var userCreationDto = (InspectionRequest)_dtoFactory.UseDto("inspectiondto", dto);

            try
            {
                var response = await _messageSession.Request<InspectionResponse>(userCreationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
