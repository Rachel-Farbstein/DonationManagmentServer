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
    public class DonationsController : ControllerBase
    {
        private readonly DonationContext _context;

        public DonationsController(DonationContext context)
        {
            _context = context;
        }

        // GET: api/Donations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Donation>>> Getdonataions()
        {
            return await _context.Donataions.ToListAsync();
        }

        // GET: api/Donations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id)
        {
            var donation = await _context.Donataions.FindAsync(id);

            if (donation == null)
            {
                return NotFound();
            }

            return donation;
        }

        // PUT: api/Donations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonation(int id, Donation donation)
        {
            if (id != donation.Id)
            {
                return BadRequest();
            }

            _context.Entry(donation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
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

        // POST: api/Donations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(Donation donation)
        {
            _context.Donataions.Add(donation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDonation", new { id = donation.Id }, donation);
        }

        // DELETE: api/Donations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            var donation = await _context.Donataions.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            _context.Donataions.Remove(donation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonationExists(int id)
        {
            return _context.Donataions.Any(e => e.Id == id);
        }
    }
}
