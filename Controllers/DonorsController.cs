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
    public class DonorsController : ControllerBase
    {
        private readonly DonorService _donorService;
        private readonly UserService _userService;

        public DonorsController(DonorService donorService, UserService userService)
        {
            _donorService = donorService;
            _userService = userService;
        }

        // GET: api/Donors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonorDto>>> GetDonors()
        {
            var userId = await _userService.getUserIdByToken(User);
            var donors = await _donorService.GetDonors(userId);
            return Ok(donors);
        }


        // GET: api/Donors/5
        [HttpGet("{donorId}")]
        public async Task<ActionResult<DonorDto>> GetDonor(int donorId)
        {
            var donor = await _donorService.GetDonorByIdAsync(donorId);

            if (donor == null)
            {
                return NotFound();
            }

            return donor;
        }

        // PUT: api/Donors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{donorId}")]
        public async Task<IActionResult> PutDonor(int donorId, DonorDto donorDto)
        {
            if (donorId != donorDto.DonorId)
            {
                return BadRequest();
            }

            try
            {
                var userId = await _userService.getUserIdByToken(User);
                await _donorService.UpdateDonorAsync(donorDto, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }



        }

        // POST: api/Donors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostDonor([FromBody] DonorDto donorDto)
        {
            try
            {
                var userId = await _userService.getUserIdByToken(User);
                await _donorService.AddDonorAsync(donorDto, userId);
                return CreatedAtAction(nameof(PostDonor), new { id = donorDto.DonorId }, donorDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        [HttpPost("delete-donors")]
        public async Task<IActionResult> DeleteMultipleDonors([FromBody] List<int> donorIds)
        {
            if (donorIds == null || donorIds.Count == 0)
            {
                return BadRequest("No donors IDs provided.");
            }

            try
            {
                await _donorService.DeleteDonorsAsync(donorIds);

                return Ok(new { Message = $"{donorIds.Count} products deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        // DELETE: api/Donors/5
        [HttpDelete("{donorId}")]
        public async Task<IActionResult> DeleteDonor([FromRoute] int donorId)
        {
            var donor = await _donorService.GetDonorByIdAsync(donorId);
            if (donor == null)
            {
                return NotFound();
            }

            await _donorService.DeleteDonorAsync(donorId);

            return NoContent();
        }

    }
}
