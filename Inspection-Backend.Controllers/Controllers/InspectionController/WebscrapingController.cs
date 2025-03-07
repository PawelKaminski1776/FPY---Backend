using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using InspectionBackend.Contracts.UserDtos;
using System.Threading.Tasks;
using InspectionBackend.UserDetails.Controllers.DtoFactory;
using InspectionBackend.Contracts.InspectionDtos;

namespace InspectionBackend.Controllers.InspectionControllers
{
    [ApiController]
    [Route("Api/Training")]
    public class WebsraperController : BaseController
    {
        public WebsraperController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpPost("Webscrape")]
        public async Task<IActionResult> ImageRequest([FromBody] InspectionImageRequest dto)
        {
            var InspectionImagesDto = (InspectionImageRequest)_dtoFactory.UseDto("inspectionimagesdto", dto);

            try
            {
                var response = await _messageSession.Request<InspectionImageResponse>(InspectionImagesDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
