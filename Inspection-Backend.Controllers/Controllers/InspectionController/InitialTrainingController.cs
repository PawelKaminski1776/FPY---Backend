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
        public async Task<IActionResult> AddInspection([FromForm] InspectionRequest dto)
        {
            Console.WriteLine("Cocofile: " + dto.Cocofile?.FileName);
            Console.WriteLine("Photos count: " + dto.Photos?.Length);
            var userCreationDto = (InspectionRequest)_dtoFactory.UseDto("inspectiondto", dto);
            Console.WriteLine("Cocofile: " + dto.Cocofile?.FileName);
            Console.WriteLine("Photos count: " + dto.Photos?.Length);
            try
            {
                if(dto.Email == null || dto.Inspectionname == null)
                {
                    return BadRequest("Email or Inspection Name are Empty");
                }
                if (dto.Cocofile == null || dto.Photos == null || dto.Photos.Length == 0)
                {
                    return BadRequest("Cocofile and at least one photo are required.");
                }

                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory); 
                }

                var cocofilePath = Path.Combine("Uploads", dto.Cocofile.FileName);

                using (var stream = new FileStream(cocofilePath, FileMode.Create))
                {
                    await dto.Cocofile.CopyToAsync(stream);
                }

                foreach (var photo in dto.Photos)
                {
                    var photoPath = Path.Combine("Uploads", photo.FileName);
                    using (var stream = new FileStream(photoPath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                }

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
