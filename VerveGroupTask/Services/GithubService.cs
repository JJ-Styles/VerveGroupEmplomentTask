using Microsoft.EntityFrameworkCore;
using StaffApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VerveGroupTask.Models;

namespace VerveGroupTask.Web.Services
{
    public class GithubService : IGithubService
    {
        private HttpClient _client;
        private readonly TempDB _context;

        public GithubService(IHttpClientFactory clientFactory,
                             TempDB context) //Sets up a HttpClient with a retry and circuit break patterns attached. Also sets up DbContext
        {
            var client = clientFactory.CreateClient("RetryAndBreak");
            client.BaseAddress = new Uri("https://api.github.com/");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");  //Github APi blocks any calls that dont contain a UserAgent
            _client = client;
            _context = context;
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        public async Task<IEnumerable<RepoDTO>> GetRepos(string Login)
        {
            var response = await GetClient().GetAsync("users/"+ Login.ToLower() + "/repos"); //Queries the repo section of the github api
            if (!response.IsSuccessStatusCode) //checks whether query was unsuccessful
            {
                var repoDb = await _context.Repos.Where(r => r.UserLogin == Login).ToListAsync(); //Pull backup data in Db if unsuccessful 
                if (repoDb.Count != 0)
                {
                    var repo = new List<RepoDTO>();

                    foreach (Repos repoItem in repoDb) //Controller expects a RepoDTO so convert to RepoDTO
                    {
                        repo.Add(new RepoDTO {Full_Name = repoItem.Full_Name, Description = repoItem.Description, Name = repoItem.Name, Stargazers_Count = repoItem.Stargazers_Count, SVN_Url = repoItem.Svn_Url, UserLogin = repoItem.UserLogin });
                    }
                    return repo;
                }
                else //If repos are not in Db
                {
                      return null;
                }
            }
            response.EnsureSuccessStatusCode();
            IEnumerable<RepoDTO> repoDTOs = await response.Content.ReadAsAsync<IEnumerable<RepoDTO>>();   //Controller expects a RepoDTO so convert to RepoDTO
            return repoDTOs;
        }

        public async Task<IEnumerable<StargazerDTO>> GetStargazers(string RepoFullName)
        {
            var response = await GetClient().GetAsync("repos/" + RepoFullName.ToLower() + "/stargazers");  //Queries the stargazers section of the github api
            if (!response.IsSuccessStatusCode)   //checks whether query was unsuccessful
            {
                var stargazerDb = await _context.Stargazers.Where(r => r.Repo.Full_Name == RepoFullName).ToListAsync();  //Pull backup data in Db if unsuccessful
                if (stargazerDb.Count != 0)
                {
                    var stargazer = new List<StargazerDTO>();

                    foreach (Stargazers stargazerItem in stargazerDb) //Controller expects a StargazerDTO so convert to StargazerDTO
                    {
                        stargazer.Add(new StargazerDTO { Login = stargazerItem.Login});
                    }
                    return stargazer;
                }
                else //If Stargazers are not in Db
                {
                      return null;
                }
            }
            response.EnsureSuccessStatusCode();
            IEnumerable<StargazerDTO> stargazerDTOs = await response.Content.ReadAsAsync<IEnumerable<StargazerDTO>>();   //Controller expects a StargazerDTO so convert to StargazerDTO
            return stargazerDTOs;
        }

        public async Task<UserDTO> GetUser(string user)
        {
            var response = await GetClient().GetAsync("users/" + user); //Queries the Users section of the github api
            if (!response.IsSuccessStatusCode) //checks whether query was unsuccessful
            {
                var userDb = _context.Users.FirstOrDefault(r => r.Login == user);
                if (user != null)
                {
                    var userDto = new UserDTO //Controller expects a UserDTO so convert to UserDTO
                    {
                        Name = userDb.Name,
                        Location = userDb.Location,
                        Avatar_Url = userDb.Avatar_Url
                    };

                    return userDto; 
                }
                else //If User is not in Db
                {
                      return null;
                }
            }
            response.EnsureSuccessStatusCode();
            UserDTO userDTO = await response.Content.ReadAsAsync<UserDTO>();  //Controller expects a UserDTO so convert to UserDTO
            return userDTO;
        }
    }
}
