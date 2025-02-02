using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using DonationManagmentServer.Models;
using DonationManagmentServer.Models.DTO;
using DonationManagmentServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DonationManagmentServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly S3Service _s3Service;
        private readonly IAmazonS3 _s3Client;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        //private readonly string? _bucketName;
        public EmailController(S3Service s3service, UserService userService, EmailService emailService)
        {
            _s3Service = s3service;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromForm] string toEmail, [FromForm] string subject, [FromForm] string body, [FromForm] DonationDto donationDto)
        {
            try
            {
                if (donationDto.File == null)
                {

                }
                else
                {
                    await _emailService.SendEmailWithAttachmentAsync(toEmail, subject, body, donationDto.File);
                }
                return Ok(new { message = "Email sent successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
