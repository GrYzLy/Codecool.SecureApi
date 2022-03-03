using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codecool.SecureApi.Controllers
{

    [ApiController]
    [Route("[controller]/[action]")]
    public class SecureController : Controller
    {
        
        private IServiceProvider Services { get; set; }
        private Microsoft.AspNetCore.Antiforgery.IAntiforgery Csrf { get { return Services.GetRequiredService<Microsoft.AspNetCore.Antiforgery.IAntiforgery>(); } }

        private HttpContext Context { get { return Services.GetRequiredService<IHttpContextAccessor>().HttpContext; } }
        public SecureController(IServiceProvider services)
        {
            this.Services = services;
        }


        [HttpPost]
        public IActionResult Login(LoginData loginData)
        {
            if (loginData.login == "juzek" && loginData.password == "maslo")
            {
                var tokens = Csrf.GetAndStoreTokens(HttpContext);
                Response.Cookies.Append("X-XSRF-TOKEN", tokens.RequestToken, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    HttpOnly = false
                });

            }
            else
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }
            
            return Ok(loginData.login);
        }
    }
}