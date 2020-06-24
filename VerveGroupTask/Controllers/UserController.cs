using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerveGroupTask.Web.Services;

namespace VerveGroupTask.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IGithubService _githubService;

        public UserController(IGithubService githubService)
        {
            _githubService = githubService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetUser(string user)
        {
            if (user == null || user.Equals(""))
            {
                return NotFound();
            }

            var userAccount = _githubService.GetUser(user);

            if (userAccount == null)
            {
                return NotFound();
            }

            return View(await userAccount);
        }
    }
}