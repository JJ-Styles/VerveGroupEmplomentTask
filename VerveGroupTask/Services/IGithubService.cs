using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VerveGroupTask.Web.Services
{
    public interface IGithubService
    {
        Task<UserDTO> GetUser(string user);
        Task<IEnumerable<RepoDTO>> GetRepos(string Login);
        Task<IEnumerable<StargazerDTO>> GetStargazers(string RepoFullName);
    }
}
