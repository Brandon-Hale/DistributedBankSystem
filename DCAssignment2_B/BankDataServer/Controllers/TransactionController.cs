using BankDataServer.Data;
using BankDataServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankDataServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        [HttpGet]
        [Route("getall")]
        public IEnumerable<Transaction> GetTransactions()
        {
            List<Transaction> transactions = TransactionManager.GetAll();
            return transactions;
        }

        [HttpGet]
        [Route("get/{transactionId}")]
        public IActionResult GetTransaction(uint transactionId)
        {
            Transaction transaction = TransactionManager.GetById(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPost]
        [Route("insert")]
        public IActionResult PostTransaction([FromBody] Transaction transaction)
        {
            if (TransactionManager.Insert(transaction))
            {
                var fromAccount = BankAccountManager.GetByAccNo(transaction.FromAccount);
                var toAccount = BankAccountManager.GetByAccNo(transaction.ToAccount);

                fromAccount.Balance -= transaction.Amount;
                toAccount.Balance += transaction.Amount;

                BankAccountManager.Update(fromAccount);
                BankAccountManager.Update(toAccount);
                return Ok("Successfully Inserted");
            }
            return BadRequest("Could not Insert");
        }

        [HttpDelete]
        [Route("delete/{transactionId}")]
        public IActionResult DeleteTransaction(uint transactionId)
        {
            if (TransactionManager.Delete(transactionId))
            {
                return Ok("Successfully Deleted Transaction");
            }
            return BadRequest("Could not delete transaction");
        }

        [HttpPut]
        [Route("update")]
        public IActionResult UpdateTransaction(Transaction transaction)
        {
            Transaction beforeUpdate = TransactionManager.GetById(transaction.TransactionId);

            if (beforeUpdate != null)
            {
                double dif = transaction.Amount - beforeUpdate.Amount;

                if (TransactionManager.Update(transaction))
                {
                    var fromAccount = BankAccountManager.GetByAccNo(transaction.FromAccount);
                    var toAccount = BankAccountManager.GetByAccNo(transaction.ToAccount);

                    fromAccount.Balance -= dif;
                    toAccount.Balance += dif;

                    BankAccountManager.Update(fromAccount);
                    BankAccountManager.Update(toAccount);
                    return Ok("Successfully Updated");
                }
            }
            return BadRequest("Could not Update");
        }

    }
}
