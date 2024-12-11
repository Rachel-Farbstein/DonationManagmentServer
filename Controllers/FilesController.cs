using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using DonationManagmentServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace DonationManagmentServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly S3Service _s3Service;
        private readonly IAmazonS3 _s3Client;
        private readonly string? _bucketName;
        public FilesController(S3Service s3service)
        {
            _s3Service = s3service;
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

            //var fileUrl = await _s3Service.UploadFileAsync(file);
            var uniqueKey = await _s3Service.UploadFileAsync(file);

            return Ok(new { Key = uniqueKey, Message = "File uploaded successfully!" });

        }

        //public async Task SaveFileMetadata(string fileName, string fileUrl, int userId = 0)
        //{
        //    // Your logic to save metadata in SQL Server
        //}
    }
}
