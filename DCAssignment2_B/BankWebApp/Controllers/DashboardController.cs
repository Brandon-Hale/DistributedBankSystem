using Microsoft.AspNetCore.Mvc;
using RestSharp;
using BankWebApp.Models;
using Newtonsoft.Json;
using System.Reflection;

namespace BankWebApp.Controllers
{
    [Route("/api/[controller]")]
    public class DashboardController : Controller
    {
        RestClient restClient = new RestClient("http://localhost:5019/api");
        [HttpGet]
        public IActionResult DashboardView()
        {
            DashboardModel model = new DashboardModel();
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
                List<Transaction> allTransactions = JsonConvert.DeserializeObject<List<Transaction>>(Bankresponse.Content);

                if (Request.Cookies.ContainsKey("UserType") && Request.Cookies["UserType"] == "admin")
                {
                    model.BankAccounts = allBankAccounts;
                } else
                {
                    model.BankAccounts = allBankAccounts.Where(account => account.UserId == model.User.UserId).ToList();
                } 
            }
               
            return PartialView("DashboardView", model);

        }

        [HttpGet("users")]
        public IActionResult DashboardUsersView()
        {
            UsersModel model = new UsersModel();

            if (Request.Cookies.ContainsKey("UserType") && Request.Cookies["UserType"] == "admin")
            {
                RestRequest Userrequest = new RestRequest($"user/getall");
                RestResponse Userresponse = restClient.Get(Userrequest);

                model.Users = JsonConvert.DeserializeObject<List<User>>(Userresponse.Content);
            }

            return PartialView("DashboardUsersView", model);
        }

        [HttpGet("edit")]
        public IActionResult DashboardEditView()
        {
            User user = new User();
            if (Request.Cookies.ContainsKey("Authenticated"))
            {
                var cookieValue = Request.Cookies["Authenticated"];
                if (cookieValue == "true")
                {
                    if (HttpContext.Request.Cookies.TryGetValue("User", out string userEmail))
                    {
                        RestRequest Userrequest = new RestRequest($"user/getbyemail/{userEmail}");
                        RestResponse Userresponse = restClient.Get(Userrequest);
                        user = JsonConvert.DeserializeObject<User>(Userresponse.Content);
                        Console.WriteLine("Opening Dashboard View");
                        return PartialView("DashboardEditView", user);
                    }
                }
            }
            Console.WriteLine("Opening Failed Dashboard View");
            return PartialView("DashboardEditView", user);
        }

        [HttpGet("edituser/{userId}")]
        public IActionResult AdminDashboardEditUserView(uint userId)
        {
            RestRequest req = new RestRequest($"user/getbyid/{userId}");
            RestResponse res = restClient.Get(req);
            User user = JsonConvert.DeserializeObject<User>(res.Content);

            return PartialView("DashboardEditView", user);
        }

        [HttpPut("updateuser")]
        public IActionResult DashboardUpdate([FromBody] User user)
        {
            if (HttpContext.Request.Cookies.TryGetValue("User", out string userEmail))
            {
                Response.Cookies.Append("Authenticated", "true");
                Console.WriteLine($"Received User Update - {user.UserId}, {user.Name}, {user.Email}, {user.PhoneNo}");
                RestRequest restRequest = new RestRequest("user/update").AddJsonBody(user);
                RestResponse restResponse = restClient.Put(restRequest);
                return Json(new { user = true });
            }
            Console.WriteLine($"Received User Update - {user.Name}, {user.Email}, {user.PhoneNo}");
            return Json(new { user = false });
        }

        [HttpPost("deleteuser/{userId}")]
        public void DashboardDeleteUser(uint userId)
        {
            RestRequest req = new RestRequest($"user/delete/{userId}");
            restClient.Delete(req);
        }
    }
}
