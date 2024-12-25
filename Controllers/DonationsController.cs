using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DonationManagmentServer.Models;
using DonationManagmentServer.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DonationManagmentServer.Models.DTO;

namespace DonationManagmentServer.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DonationsController : ControllerBase
    {
        private readonly DonationService _donationService;
        private readonly UserService _userService;

        public DonationsController(DonationService donationService, UserService userService)
        {
            _donationService = donationService;
            _userService = userService;
        }

        // GET: api/Donations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonationDto>>> GetDonations()
        {
            try
            {
                var userId = await _userService.getUserIdByToken(User);
                var donations = await _donationService.GetDonations(userId);
                return Ok(donations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-donations-with-donors")]
        public async Task<ActionResult<IEnumerable<DonationDtoWithDonorName>>> GetDonationsWithDonors()
        {
            try
            {
                var userId = await _userService.getUserIdByToken(User);
                var donations = _donationService.GetDonationsWithDonorName(userId);
                return Ok(donations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET: api/Donations
        [HttpGet("get-donations-by-donor {donorId}")]
        public async Task<ActionResult<IEnumerable<DonationDto>>> GetDonationsByDonorId(int donorId)
        {
            var donations = await _donationService.GetDonationsByDonorIdAsync(donorId);
            return Ok(donations);
        }

        // GET: api/Donations/5
        [HttpGet("{donationId}")]
        public async Task<ActionResult<DonationDto>> GetDonation(int donationId)
        {
            var donation = await _donationService.GetDonationByIdAsync(donationId);

            if (donation == null)
            {
                return NotFound();
            }

            return donation;
        }

        // PUT: api/Donation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{donationId}")]
        public async Task<IActionResult> PutDonation(int donationId, [FromBody] DonationDto donationDto)
        {
            if (donationId != donationDto.DonationId)
            {
                return BadRequest();
            }

            await _donationService.UpdateDonationAsync(donationDto);

            return NoContent();
        }

        // POST: api/Donations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostDonation([FromBody] DonationDto donationDto)
        {
            try
            {
                await _donationService.AddDonationAsync(donationDto);

                return CreatedAtAction(nameof(PostDonation), new { id = donationDto.DonationId }, donationDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }


        [HttpPost("delete-donations")]
        public async Task<IActionResult> DeleteMultipleDonations([FromBody] List<int> donationIds)
        {
            if (donationIds == null || donationIds.Count == 0)
            {
                return BadRequest("No donations IDs provided.");
            }

            try
            {
                await _donationService.DeleteDonationsAsync(donationIds);

                return Ok(new { Message = $"{donationIds.Count} donations deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // DELETE: api/Donations/5
        [HttpDelete("{donationId}")]
        public async Task<IActionResult> DeleteDonation(int donationId)
        {
            var donation = await _donationService.GetDonationByIdAsync(donationId);
            if (donation == null)
            {
                return NotFound();
            }

            await _donationService.DeleteDonationAsync(donationId);

            return NoContent();
        }

    }
}
