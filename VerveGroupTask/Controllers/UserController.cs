using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StaffApp.Data;
using VerveGroupTask.Models;
using VerveGroupTask.Web.Services;

namespace VerveGroupTask.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IGithubService _githubService;
        private readonly TempDB _context;

        public UserController(IGithubService githubService,
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
                var userDb = new User
                {
                    Login = user.Name,
                    Name = userAccount.Name,
                    Location = userAccount.Location,
                    Avatar_Url = userAccount.Avatar_Url
                };

                foreach (RepoDTO repo in repos)
                {
                    if (!RepoExists(repo.Full_Name))
                    {
                        _context.Add(new Repos { Full_Name = repo.Full_Name, Description = repo.Description, Name = repo.Name, Stargazers_Count = repo.Stargazers_Count, Svn_Url = repo.SVN_Url, UserLogin = user.Name });
                        await _context.SaveChangesAsync();

                        var last = await _context.Repos.LastAsync();

                        foreach (StargazerDTO stargazer in repo.Stargazers)
                        {
                            _context.Add(new Stargazers { Login = stargazer.Login, RepoID = last.ID });
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                if (!UserExists(userDb.Login))
                {
                    _context.Add(userDb);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                    Console.Out.WriteLine(e);
            }

            return View(repos);
        }

        private bool UserExists(string login)
        {
            return _context.Users.Any(e => e.Login == login);
        }

        private bool RepoExists(string fullname)
        {
            return _context.Repos.Any(e => e.Full_Name == fullname);
        }

        private bool StargazerExists(string login)
        {
            return _context.Stargazers.Any(e => e.Login == login);
        }
    }
}
