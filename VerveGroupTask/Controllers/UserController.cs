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
            return View(); //Default View With Search Box
        }

        public async Task<IActionResult> GetUser(User user)
        {
            if (user == null) //Stop searching a null user
            {
                return NotFound();
            }

            var userAccount = await _githubService.GetUser(user.Name); //gets the User from the APi
            var repos = await _githubService.GetRepos(user.Name); //gets the Repo from the Api

            if (userAccount == null || repos == null) //Checks that neither of the Api calls gave null values
            {
                return NotFound();
            }

            repos.ToList(); //Turn the repos into a list to be iterated through
            foreach (RepoDTO repo in repos)
            {
                repo.Owner = userAccount; //sets the user for each repo
            }
            repos = repos.OrderByDescending(x => x.Stargazers_Count); //Orders the Repos so the first repo has the highest Stargazers count
            repos = repos.Take(5); //Makes the List only contain the highest 5 stargazers count

            foreach (RepoDTO repo in repos)
            {
                repo.Stargazers = await _githubService.GetStargazers(repo.Full_Name); //Gets the stargazers for each of the repos
            }

            try
            {
                var userDb = new User  //creates a user object from the data from api and user
                {
                    Login = user.Name,
                    Name = userAccount.Name,
                    Location = userAccount.Location,
                    Avatar_Url = userAccount.Avatar_Url
                };

                foreach (RepoDTO repo in repos)
                {
                    //Checks repo does not exist as there is no need for a temp save of the data if it already exists.
                    if (!RepoExists(repo.Full_Name))
                    {
                        //adds each repo to the Db in case the app cannot make a successful connection
                        _context.Add(new Repos { Full_Name = repo.Full_Name, Description = repo.Description, Name = repo.Name, Stargazers_Count = repo.Stargazers_Count, Svn_Url = repo.SVN_Url, UserLogin = user.Name });
                        await _context.SaveChangesAsync();

                        var last = await _context.Repos.LastAsync(); //gets the last id from the repos

                        foreach (StargazerDTO stargazer in repo.Stargazers) // Saves the stargazers to the Db
                        {
                            _context.Add(new Stargazers { Login = stargazer.Login, RepoID = last.ID });
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                if (!UserExists(userDb.Login)) // checks if the user exists in the Db already
                {
                    _context.Add(userDb); //Adds the user to the Db incase of an unsuccessful api connection
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                    Console.Out.WriteLine(e);
            }

            return View(repos); //sends a RepoDTO to the view which has a link to both user and stargazer
        }

        //checks whether a user exists
        private bool UserExists(string login)
        {
            return _context.Users.Any(e => e.Login == login);
        }

        //checks whether a Repo exists
        private bool RepoExists(string fullname)
        {
            return _context.Repos.Any(e => e.Full_Name == fullname);
        }
    }
}
