using DonationManagmentServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DonationManagmentServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonorsControllerOld : ControllerBase
    {

        private static List<Donor> donorsList = new List<Donor>
        {
            new Donor{Id = 1, FullName = "שלוש",Email="rachely7331@gmail.com", Address = "הרב רובין", Phone = "0548447331"},
            new Donor{Id = 2, FullName = "יוסוש",Email="rachely7331@gmail.com", Address = "הרב רובין", Phone = "0548423162"}
    
        };


        [HttpGet]
        public List<Donor> Get()
        {
            return donorsList;
        }

        [HttpGet]
        [Route("getNextId")]
        public int GetNextId()
        {
            int id;
            id = donorsList.Max(x => x.Id);
            id ++;
            return id;
        }

        //[HttpPost]
        //public List<Donor> Post([FromBody] Donor donor)
        //{
        //    donorsList.Add(donor);
        //    return donorsList;
        //}

        [HttpPost]
        public void Post(Donor donor)
        {
            donorsList.Add(donor);
        }

    }
}
