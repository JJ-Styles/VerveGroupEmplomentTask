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
                             TempDB context)
        {
            var client = clientFactory.CreateClient("RetryAndBreak");
            client.BaseAddress = new Uri("https://api.github.com/");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _client = client;
            _context = context;
        }

        public  GithubService(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _client = client;
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        public async Task<IEnumerable<RepoDTO>> GetRepos(string Login)
        {
            var response = await GetClient().GetAsync("users/"+ Login.ToLower() + "/repos");
            if (!response.IsSuccessStatusCode)
            {
                var repoDb = await _context.Repos.Where(r => r.UserLogin == Login).ToListAsync();
                if (repoDb.Count != 0)
                {
                    var repo = new List<RepoDTO>();

                    foreach (Repos repoItem in repoDb)
                    {
                        repo.Add(new RepoDTO {Full_Name = repoItem.Full_Name, Description = repoItem.Description, Name = repoItem.Name, Stargazers_Count = repoItem.Stargazers_Count, SVN_Url = repoItem.Svn_Url, UserLogin = repoItem.UserLogin });
                    }
                    return repo; 
                }
                else
                {
                      return null;
                }
            }
            response.EnsureSuccessStatusCode();
            IEnumerable<RepoDTO> repoDTOs = await response.Content.ReadAsAsync<IEnumerable<RepoDTO>>();
            return repoDTOs;
        }

        public async Task<IEnumerable<StargazerDTO>> GetStargazers(string RepoFullName)
        {
            var response = await GetClient().GetAsync("repos/" + RepoFullName.ToLower() + "/stargazers");
            if (!response.IsSuccessStatusCode)
            {
                var stargazerDb = await _context.Stargazers.Where(r => r.Repo.Full_Name == RepoFullName).ToListAsync();
                if (stargazerDb.Count != 0)
                {
                    var stargazer = new List<StargazerDTO>();

                    foreach (Stargazers stargazerItem in stargazerDb)
                    {
                        stargazer.Add(new StargazerDTO { Login = stargazerItem.Login});
                    }
                    return stargazer;
                }
                else
                {
                      return null;
                }
            }
            response.EnsureSuccessStatusCode();
            IEnumerable<StargazerDTO> stargazerDTOs = await response.Content.ReadAsAsync<IEnumerable<StargazerDTO>>();
            return stargazerDTOs;
        }

        public async Task<UserDTO> GetUser(string user)
        {
            var response = await GetClient().GetAsync("users/" + user);
            if (!response.IsSuccessStatusCode)
            {
                var userDb = _context.Users.FirstOrDefault(r => r.Login == user);
                if (user != null)
                {
                    var userDto = new UserDTO
                    {
                        Name = userDb.Name,
                        Location = userDb.Location,
                        Avatar_Url = userDb.Avatar_Url
                    };

                    return userDto; 
                }
                else
                {
                      return null;
                }
            }
            response.EnsureSuccessStatusCode();
            UserDTO userDTO = await response.Content.ReadAsAsync<UserDTO>();
            return userDTO;
        }
    }
}
