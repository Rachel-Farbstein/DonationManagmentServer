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

namespace DonationManagmentServer.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<Donor>>> GetDonors()
        {
            var userId = await _userService.getUserIdByToken(User);
            var donors = await _donorService.GetDonors(userId);
            return Ok(donors);
        }


        // GET: api/Donors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donor>> GetDonor(int donorId)
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
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonor(int donorId, Donor donor)
        {
            if (donorId != donor.DonorId)
            {
                return BadRequest();
            }

            await _donorService.UpdateDonorAsync(donor);

            return NoContent();
        }

        // POST: api/Donors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donor>> PostDonor(DonorDto donorDto)
        {
            var userId = await _userService.getUserIdByToken(User);

            Donor donor = new Donor()
            {
                DonorId = donorDto.DonorId,
                UserId = userId,
                FullName = donorDto.FullName,
                Email = donorDto.Email,
                Address = donorDto.Address,
                Phone = donorDto.Phone,
            };

            await _donorService.AddDonorAsync(donor);

            return CreatedAtAction("GetDonor", new { id = donor.DonorId }, donor);
        }


        [HttpPost("delete-donors")]
        public async Task<IActionResult> DeleteMultipleDonors([FromBody] List<int> donorIds)
        {
            if (donorIds == null || donorIds.Count == 0)
            {
                return BadRequest("No donors IDs provided.");
            }

            return NoContent();
            //try
            //{
            //    var donorList = _context.Donor.Where(d => donorIds.Contains(d.Id)).ToList();

            //    if (donorList.Count == 0)
            //    {
            //        return NotFound("No matching products found.");
            //    }

            //    _context.Donor.RemoveRange(donorList);
            //    await _context.SaveChangesAsync();

            //    return Ok(new { Message = $"{donorList.Count} products deleted successfully." });
            //}
            //catch (Exception ex)
            //{
            //    // Log the error
            //    return StatusCode(500, $"Internal server error: {ex.Message}");
            //}
        }


        // DELETE: api/Donors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int donorId)
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
