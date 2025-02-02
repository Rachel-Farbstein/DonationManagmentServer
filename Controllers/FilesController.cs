using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationManagmentServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly S3Service _s3Service;
        private readonly IAmazonS3 _s3Client;
        private readonly UserService _userService;

        //private readonly string? _bucketName;
        public FilesController(S3Service s3service, UserService userService)
        {
            _s3Service = s3service;
            _userService = userService;
        }


        // GET: api/files
        [HttpGet]
        public async Task<ActionResult> GetUrl()
        {
            return Ok(new { Url = "fileUrl" });
        }

        // POST: api/Files
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var userId = await _userService.GetUserIdByToken(User);
            var uniqueKey = await _s3Service.UploadFileAsync("hh", file);

            return Ok(new { Key = uniqueKey, Message = "File uploaded successfully!" });

        }
        [HttpGet("get-file-from-s3")]
        //public async Task<ActionResult> GetFileFromS3([FromBody] FileDetails fileDetails)
        public async Task<ActionResult> GetFileFromS3([FromQuery] string bucketName, [FromQuery] string key)
        {
            var fileS3 = await _s3Service.GetFileAsync(bucketName, key);

            using var memoryStream = new MemoryStream();
            await fileS3.ResponseStream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            return File(fileBytes, fileS3.Headers["Content-Type"]);
        }

        //public async Task SaveFileMetadata(string fileName, string fileUrl, int userId = 0)
        //{
        //    // Your logic to save metadata in SQL Server
        //}
    }
}
