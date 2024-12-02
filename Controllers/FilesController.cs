using DonationManagmentServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DonationManagmentServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly S3Service _s3Service;

        public FilesController(S3Service s3Service)
        {
            _s3Service = s3Service;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var fileUrl = await _s3Service.UploadFileAsync(file);

            //this.SaveFileMetadata()
            return Ok(new { Url = fileUrl });
        }

        public async Task SaveFileMetadata(string fileName, string fileUrl, int userId)
        {
            // Your logic to save metadata in SQL Server
        }
    }
}
