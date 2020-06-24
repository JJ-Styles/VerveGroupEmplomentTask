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

        public GithubService(IHttpClientFactory clientFactory)
        {
            var client = clientFactory.CreateClient("RetryAndBreak");
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
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            IEnumerable<RepoDTO> repoDTOs = await response.Content.ReadAsAsync<IEnumerable<RepoDTO>>();
            return repoDTOs;
        }

        public async Task<IEnumerable<StargazerDTo>> GetStargazers(string RepoFullName)
        {
            var response = await GetClient().GetAsync("repos/" + RepoFullName.ToLower() + "/stargazers");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            IEnumerable<StargazerDTo> stargazerDTos = await response.Content.ReadAsAsync<IEnumerable<StargazerDTo>>();
            return stargazerDTos;
        }

        public async Task<UserDTO> GetUser(string user)
        {
            var response = await GetClient().GetAsync("users/" + user);
            if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Forbidden)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            UserDTO userDTO = await response.Content.ReadAsAsync<UserDTO>();
            return userDTO;
        }
    }
}
