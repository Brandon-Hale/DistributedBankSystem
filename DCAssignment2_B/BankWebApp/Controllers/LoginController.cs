using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BankWebApp.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        RestClient restClient = new RestClient("http://localhost:5019/api");
        [HttpGet]
        public IActionResult LoginView()
        {
            
            if (Request.Cookies.ContainsKey("Authenticated"))
            {
                var cookieValue = Request.Cookies["Authenticated"];
                if (cookieValue == "true")
                {
                    return PartialView("LoginAuthenticatedView");
                }
            }
            return PartialView("LoginView");
        }

        [HttpGet("authed")]
        public IActionResult LoginAuthenticatedView()
        {
            if (Request.Cookies.ContainsKey("Authenticated"))
            {
                var cookieValue = Request.Cookies["Authenticated"];
                if (cookieValue == "true")
                {
                    return PartialView("LoginAuthenticatedView");
                }

            }
            return PartialView("LoginErrorView");
        }

        [HttpGet("error")]
        public IActionResult LoginErrorView() 
        {
            if (Request.Cookies.ContainsKey("Authenticated"))
            {
                var cookieValue = Request.Cookies["Authenticated"];
                if (cookieValue == "true")
                {
                    return PartialView("LoginAuthenticatedView");
                }
            }
            return PartialView("LoginErrorView");
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User user)
        {

            if(user.Email.Equals("admin") &&  user.Password.Equals("admin"))
            {
                Response.Cookies.Append("Authenticated", "true");
                Response.Cookies.Append("User", user.Email.ToString());
                return Json(new { login = true });

            } else
            {
                RestRequest request = new RestRequest("user/getall");
                RestResponse restResponse = restClient.Get(request);
                List<User> users = JsonConvert.DeserializeObject<List<User>>(restResponse.Content);

                foreach (User val in users)
                {
                    if (user.Email.Equals(val.Email) && user.Password.Equals(val.Password))
                    {
                        Response.Cookies.Append("Authenticated", "true");
                        Response.Cookies.Append("User", user.Email.ToString());

                        if(val.UserType.Equals("admin"))
                        {
                            Response.Cookies.Append("UserType", "admin");
                        } else
                        {
                            Response.Cookies.Append("UserType", "user");
                        }

                        Console.WriteLine($"Received user: UserId - {user?.UserId}, Password - {user?.Password}");
                        return Json(new { login = true });
                    }
                }
            }
            return Json(new { login = false });
        }
    }
}
