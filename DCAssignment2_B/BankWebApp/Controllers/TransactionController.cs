using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Reflection;

namespace BankWebApp.Controllers
{
    [Route("/api/[controller]")]
    public class TransactionController : Controller
    {
        RestClient restClient = new RestClient("http://localhost:5019/api");

        [HttpGet]
        public IActionResult TransactionHistoryView()
        {
            DashboardModel model = new DashboardModel();
            List<Transaction> userTransactions = new List<Transaction>();
            if (HttpContext.Request.Cookies.TryGetValue("User", out string userEmail))
            {
                RestRequest Userrequest = new RestRequest($"user/getbyemail/{userEmail}");
                RestResponse Userresponse = restClient.Get(Userrequest);
                model.User = JsonConvert.DeserializeObject<User>(Userresponse.Content);

                RestRequest Bankrequest = new RestRequest($"bankaccount/getall");
                RestResponse Bankresponse = restClient.Get(Bankrequest);
                List<BankAccount> allBankAccounts = JsonConvert.DeserializeObject<List<BankAccount>>(Bankresponse.Content);

                RestRequest Transactionrequest = new RestRequest($"transaction/getall");
                RestResponse Transactionresponse = restClient.Get(Transactionrequest);
                List<Transaction> allTransactions = JsonConvert.DeserializeObject<List<Transaction>>(Transactionresponse.Content);

                model.BankAccounts = allBankAccounts.Where(account => account.UserId == model.User.UserId).ToList();

                if (Request.Cookies.ContainsKey("UserType") && Request.Cookies["UserType"] == "admin")
                {
                    userTransactions = allTransactions;
                } else 
                { 
                    foreach (var transaction in allTransactions)
                    {
                        foreach (var bankAcc in model.BankAccounts)
                        {
                            if (transaction.ToAccount == bankAcc.AccNo || transaction.FromAccount == bankAcc.AccNo)
                            {
                                userTransactions.Add(transaction);
                            }
                        }
                    }
                }
            }
            return PartialView("TransactionHistoryView", userTransactions);
        }

        [HttpGet("usertransactions/{userId}")]
        public IActionResult TransactionHistoryByIdView(uint userId)
        {
            DashboardModel model = new DashboardModel();
            List<Transaction> userTransactions = new List<Transaction>();

            RestRequest req = new RestRequest($"user/getbyid/{userId}");
            RestResponse res = restClient.Get(req);

            model.User = JsonConvert.DeserializeObject<User>(res.Content);

            RestRequest Bankrequest = new RestRequest($"bankaccount/getall");
            RestResponse Bankresponse = restClient.Get(Bankrequest);
            List<BankAccount> allBankAccounts = JsonConvert.DeserializeObject<List<BankAccount>>(Bankresponse.Content);

            RestRequest Transactionrequest = new RestRequest($"transaction/getall");
            RestResponse Transactionresponse = restClient.Get(Transactionrequest);
            List<Transaction> allTransactions = JsonConvert.DeserializeObject<List<Transaction>>(Transactionresponse.Content);

            model.BankAccounts = allBankAccounts.Where(account => account.UserId == model.User.UserId).ToList();

            foreach (var transaction in allTransactions)
            {
                foreach (var bankAcc in model.BankAccounts)
                {
                    if (transaction.ToAccount == bankAcc.AccNo || transaction.FromAccount == bankAcc.AccNo)
                    {
                        userTransactions.Add(transaction);
                    }
                }
            }

            return PartialView("TransactionHistoryView", userTransactions);
        }

        [HttpGet("new")]
        public IActionResult TransactionNewView()
        {
            if (Request.Cookies.ContainsKey("Authenticated"))
            {
                var cookieValue = Request.Cookies["Authenticated"];
                if (cookieValue == "true")
                {
                    Console.WriteLine("Opening Transaction View");
                    return PartialView("TransactionNewView");
                }
            }
            Console.WriteLine("Opening Failed Transaction View");
            return PartialView("TransactionNewView");
        }

        [HttpPost("posttrans")]
        public IActionResult PostTransaction([FromBody] Transaction transaction)
        {
            DashboardModel model = new DashboardModel();
            List<Transaction> userTransactions = new List<Transaction>();
            List<BankAccount> allBankAccounts = new List<BankAccount>();

            if (HttpContext.Request.Cookies.TryGetValue("User", out string userEmail))
            {
                RestRequest Userrequest = new RestRequest($"user/getbyemail/{userEmail}");
                RestResponse Userresponse = restClient.Get(Userrequest);
                model.User = JsonConvert.DeserializeObject<User>(Userresponse.Content);

                RestRequest Bankrequest = new RestRequest($"bankaccount/getall");
                RestResponse Bankresponse = restClient.Get(Bankrequest);
                allBankAccounts = JsonConvert.DeserializeObject<List<BankAccount>>(Bankresponse.Content);

                //getting bank accounts of user
                model.BankAccounts = allBankAccounts.Where(account => account.UserId == model.User.UserId).ToList();

                foreach (var bankAcc in model.BankAccounts)
                {
                    foreach (var bankAc in allBankAccounts)
                    {
                        if (transaction.FromAccount.Equals(bankAcc.AccNo) && transaction.ToAccount.Equals(bankAc.AccNo))
                        {
                            Response.Cookies.Append("Authenticated", "true");
                            Response.Cookies.Append("Transaction", transaction.TransactionId.ToString());
                            Console.WriteLine($"Received Transaction - {transaction?.ToAccount}, {transaction.FromAccount}, {transaction.Amount}");
                            RestRequest restRequest = new RestRequest("transaction/insert").AddJsonBody(transaction);
                            RestResponse restResponse = restClient.Post(restRequest);
                            return Json(new { transaction = true });
                        }
                    }
                }
            }
            Console.WriteLine($"Failed Transaction - {transaction?.ToAccount}, {transaction.FromAccount}, {transaction.Amount}");
            return Json(new { login = false });
        }
    }
}
