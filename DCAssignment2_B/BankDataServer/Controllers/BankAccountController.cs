using BankDataServer.Data;
using BankDataServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankDataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankAccountController : ControllerBase
    {
        [HttpGet]
        [Route("getall")]
        public IEnumerable<BankAccount> GetBankAccounts()
        {
            List<BankAccount> bankAccounts = BankAccountManager.GetAll();
            return bankAccounts;
        }

        [HttpGet]
        [Route("get/{accNo}")]
        public IActionResult GetBankAccount(uint accNo)
        {
            BankAccount bankAccount = BankAccountManager.GetByAccNo(accNo);
            if (bankAccount == null)
            {
                return NotFound();
            }
            return Ok(bankAccount);
        }

        [HttpPost]
        [Route("insert")]
        public IActionResult PostBankAccount([FromBody] BankAccount bankAccount)
        {
            if (BankAccountManager.Insert(bankAccount))
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }

        [HttpDelete]
        [Route("delete/{accNo}")]
        public IActionResult DeleteBankAccount(uint accNo)
        {

            if (BankAccountManager.Delete(accNo))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateBankAccount(BankAccount bankAccount)
        {
            if (BankAccountManager.Update(bankAccount))
            {
                return Ok("Successfully Updated");
            }
            return BadRequest("Could not Update");
        }

    }
}
