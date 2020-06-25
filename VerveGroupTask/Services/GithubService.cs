using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace VerveGroupTask.Web.Services
{
    public class GithubService : IGithubService
    {
        private HttpClient _client;
        private readonly TempDB _context

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

        public  GithubService(HttpClient client
                              TempDB context)
        {
            client.BaseAddress = new Uri("https://api.github.com/");
            client.Timeout = TimeSpan.FromSeconds(5);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _client = client;
            _context = context;
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
                var repo = await _context.Repos.Where(r => r.UserId == Login).ToListAsync();
                if (repo != null)
                {
                     //Turn into repoDTO IEnumerable
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
                var stargazers = await _context.Stargazer.Where(r => r.RepoId == RepoFullName).ToListAsync();
                if (stargazers != null)
                {
                     //Turn into stargazerDTO IEnumerable
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
                var user = _context.Users.FirstOrDefault(r => r.Id == user);
                if (user != null)
                {
                     //Turn into userDTO
                     return user; 
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
