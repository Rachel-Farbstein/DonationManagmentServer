using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DonationManagmentServer.Models;

namespace DonationManagmentServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorsController : ControllerBase
    {
        private readonly DonationContext _context;

        public DonorsController(DonationContext context)
        {
            _context = context;
        }

        // GET: api/Donors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donor>>> GetDonor()
        {
            return await _context.Donor.ToListAsync();
        }

        // GET: api/Donors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donor>> GetDonor(int id)
        {
            var donor = await _context.Donor.FindAsync(id);

            if (donor == null)
            {
                return NotFound();
            }

            return donor;
        }

        // PUT: api/Donors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonor(int id, Donor donor)
        {
            if (id != donor.Id)
            {
                return BadRequest();
            }

            _context.Entry(donor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Donors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donor>> PostDonor(Donor donor)
        {
            _context.Donor.Add(donor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonor", new { id = donor.Id }, donor);
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
                var donorList = _context.Donor.Where(d => donorIds.Contains(d.Id)).ToList();

                if (donorList.Count == 0)
                {
                    return NotFound("No matching products found.");
                }

                _context.Donor.RemoveRange(donorList);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"{donorList.Count} products deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }





        // DELETE: api/Donors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            var donor = await _context.Donor.FindAsync(id);
            if (donor == null)
            {
                return NotFound();
            }

            _context.Donor.Remove(donor);
            _context.Donor.Remove(donor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonorExists(int id)
        {
            return _context.Donor.Any(e => e.Id == id);
        }
    }
}
