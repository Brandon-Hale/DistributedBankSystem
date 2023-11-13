using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BankDataServer.Models;
using BankDataServer.Data;

namespace BankDataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("getall")]
        public IEnumerable<User> GetUsers()
        {
            List<User> users = UserManager.GetAll();
            return users;
        }

        [HttpGet]
        [Route("getbyid/{userId}")]
        public IActionResult GetUserByID(uint userId)
        {
            User user = UserManager.GetById(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("getbyemail/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            User user = UserManager.GetByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("insert")]
        public IActionResult PostUser([FromBody] User user)
        {
            if (UserManager.Insert(user))
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }

        [HttpDelete]
        [Route("delete/{userId}")]
        public IActionResult DeleteUser(uint userId)
        {
            if (UserManager.Delete(userId))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }

        [HttpPut]
        [Route("update")]
        public IActionResult PutUser(User user)
        {
            if (UserManager.Update(user))
            {
                return Ok("Successfully Updated");
            }
            return BadRequest("Could not Update");
        }
    }
}
