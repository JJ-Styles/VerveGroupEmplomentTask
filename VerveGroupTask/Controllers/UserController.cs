using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VerveGroupTask.Models;
using VerveGroupTask.Web.Services;

namespace VerveGroupTask.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IGithubService _githubService;
        private readonly TempDB _context

        public UserController(IGithubService githubService
                              TempDB context)
        {
            _githubService = githubService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetUser(User user)
        {
            if (user == null)
            {
                return NotFound();
            }

            var userAccount = await _githubService.GetUser(user.Name);
            var repos = await _githubService.GetRepos(user.Name);

            if (userAccount == null || repos == null)
            {
                return NotFound();
            }

            repos.ToList();
            foreach (RepoDTO repo in repos)
            {
                repo.Owner = userAccount;
            }
            repos = repos.OrderByDescending(x => x.Stargazers_Count);
            repos = repos.Take(5);

            foreach (RepoDTO repo in repos)
            {
                repo.Stargazers = await _githubService.GetStargazers(repo.Full_Name);
            }

            try
                {
//values need to be changed to data classes to be saved
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    _context.Add(repo);
                    await _context.SaveChangesAsync();
                    _context.Add(stargazers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                     Console.Log(e);
                }

            return View(repos);
        }
    }
}
