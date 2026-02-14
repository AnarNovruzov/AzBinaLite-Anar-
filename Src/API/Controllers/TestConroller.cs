
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestConroller : ControllerBase
    {
        [HttpPost("my/api")]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }
            //do what you want with the file @ filePath
            return Ok();
        }
    }
}
