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
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations()
        {
            var userId = await _userService.getUserIdByToken(User);
            var donations = await _donationService.GetDonations(userId);
            return Ok(donations);
        }

        // GET: api/Donations
        [HttpGet("get-donations-by-donor {id}")]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonationsByDonorId(int donorId)
        {
            var donations = await _donationService.GetDonationsByDonorIdAsync(donorId);
            return Ok(donations);
        }

        // GET: api/Donations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int donationId)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonation(int donationId, DonationDto donationDto)
        {
            if (donationId != donationDto.DonationId)
            {
                return BadRequest();
            }

            var donation = await this.setDonationFromDonationDto(donationDto);

            await _donationService.UpdateDonationAsync(donation);

            return NoContent();
        }

        // POST: api/Donations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(DonationDto donationDto)
        {

            var donation = await this.setDonationFromDonationDto(donationDto);
            await _donationService.AddDonationAsync(donation);

            return CreatedAtAction("GetDonation", new { id = donation.DonationId }, donation);
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
        [HttpDelete("{id}")]
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

        private async Task<Donation> setDonationFromDonationDto(DonationDto donationDto)
        {
            var userId = await _userService.getUserIdByToken(User);

            Donation donation = new Donation()
            {
                DonationId = donationDto.DonationId,
                DonorId = donationDto.DonorId,
                UserId = userId,
                Amount = donationDto.Amount,
                DonationDate = donationDto.DonationDate,
                PaymentType = donationDto.PaymentType,
            };

            return donation;
        }
    }
}
