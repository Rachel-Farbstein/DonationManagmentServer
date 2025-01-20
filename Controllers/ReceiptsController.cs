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
using DonationManagmentServer.Repisotories;
using System.Text.Json;

namespace DonationManagmentServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly ReceiptService _receiptService;
        private readonly UserService _userService;

        public ReceiptsController(ReceiptService receiptService, UserService userService)
        {
            _receiptService = receiptService;
            _userService = userService;
        }

        // GET: api/Receipts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptDto>>> GetReceipts()
        {
            var userId = await _userService.GetUserIdByToken(User);
            var receipts = await _receiptService.GetReceipts(userId);
            return Ok(receipts);
        }

        // POST: api/Receipts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostReceipt([FromForm] ReceiptDto receiptDto)
        {
            Console.WriteLine($"ReceiptId: {receiptDto.ReceiptId}, ReceiptProductionDate: {receiptDto.ReceiptProductionDate}, DonationId: {receiptDto.DonationId}");
            var file = receiptDto.File;
            if (file == null || receiptDto == null) //string.IsNullOrEmpty(receiptData)
                return BadRequest("File or receipt data is missing.");

            //var receiptDto = JsonSerializer.Deserialize<ReceiptDto>(receiptData);
            //if (receiptDto == null)
            //{
            //    return BadRequest("File or receipt data is missing.");
            //}
            receiptDto.File = file;
            try
            {
                var userId = await _userService.GetUserIdByToken(User);
                var cognitoUserId = _userService.GetCognitoUserIdByToken(User);
                await _receiptService.AddReceiptAsync(receiptDto,userId, cognitoUserId);
                return CreatedAtAction(nameof(PostReceipt), new { id = receiptDto.ReceiptId }, receiptDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }


        }

        //[HttpPost("delete-donors")]
        //public async Task<IActionResult> DeleteMultipleDonors([FromBody] List<int> donorIds)
        //{
        //    if (donorIds == null || donorIds.Count == 0)
        //    {
        //        return BadRequest("No donors IDs provided.");
        //    }

        //    try
        //    {
        //        await _donorService.DeleteDonorsAsync(donorIds);

        //        return Ok(new { Message = $"{donorIds.Count} products deleted successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the error
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}



        //// DELETE: api/Donors/5
        //[HttpDelete("{donorId}")]
        //public async Task<IActionResult> DeleteDonor([FromRoute] int donorId)
        //{
        //    var donor = await _donorService.GetDonorByIdAsync(donorId);
        //    if (donor == null)
        //    {
        //        return NotFound();
        //    }

        //    await _donorService.DeleteDonorAsync(donorId);

        //    return NoContent();
        //}

    }
}
