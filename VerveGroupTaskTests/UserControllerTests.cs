using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VerveGroupTask.Models;
using VerveGroupTask.Web.Controllers;
using VerveGroupTask.Web.Services;
using Xunit;

namespace VerveGroupTask.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IGithubService> _mockRepo;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockRepo = new Mock<IGithubService>();
            _controller = new UserController(_mockRepo.Object);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = _controller.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void GetUser_ActionExecutes_ReturnsViewForGetUser()
        {
            User user = new User { Name="robconery" };
            var result = _controller.GetUser(user);
            Assert.IsType<Task<IActionResult>>(result);
        }

        [Fact]
        public async Task GetUser_GetCorrectUserDataAsync()
        {
            Mock mock = new Mock<HttpClient>();
            UserController controller = new UserController(new GithubService((HttpClient) mock.Object));
            var user = new UserDTO
            {
                Name = "Rob Conery",
                Location = "Honolulu, HI",
                Avatar_Url = "https://avatars0.githubusercontent.com/u/78586?v=4"
            };
            var repos = new List<RepoDTO>
            {
                new RepoDTO{Name="moebius", Full_Name="robconery/moebius", Description="A functional query tool for Elixir", Stargazers_Count=523, SVN_Url="https://github.com/robconery/moebius", Owner=user, Stargazers = new List<StargazerDTO>{
                    new StargazerDTO{Login="jbavari"},
                    new StargazerDTO{Login="ckampfe"}
                    }
                },
                new RepoDTO{Name="dox", Full_Name="robconery/dox", Description="A Document Database API extension for Postgres", Stargazers_Count=161, SVN_Url="https://github.com/robconery/dox", Owner=user, Stargazers = new List<StargazerDTO>{
                    new StargazerDTO{Login="christiansakai"},
                    new StargazerDTO{Login="abeusher"}
                    }
                },
                new RepoDTO{Name="congo", Full_Name="robconery/congo", Description="A MongoDB Explorer written in Backbone using Twitter Bootstrap. Part of Tekpub's Backbone.series", Stargazers_Count=139, SVN_Url="https://github.com/robconery/congo", Owner=user, Stargazers = new List<StargazerDTO>{
                    new StargazerDTO{Login="andris-silis"},
                    new StargazerDTO{Login="comster"}
                    }
                },
                new RepoDTO{Name="meteor-shop", Full_Name="robconery/meteor-shop", Description="A demo eCommerce site using Meteor.js - the code for this site can also be seen at Pluralsight.", Stargazers_Count=119, SVN_Url="https://github.com/robconery/meteor-shop", Owner=user, Stargazers = new List<StargazerDTO>{
                    new StargazerDTO{Login="LaureateTS"},
                    new StargazerDTO{Login="angusshire"}
                    }
                },
                new RepoDTO{Name="mvc3", Full_Name="robconery/mvc3", Description="Code and Resources for Real-World ASP.NET MVC3", Stargazers_Count=108, SVN_Url="https://github.com/robconery/mvc3", Owner=user, Stargazers = new List<StargazerDTO>{
                    new StargazerDTO{Login="eiu165"},
                    new StargazerDTO{Login="joemcbride"}
                    }
                }
            };

            var result = await controller.GetUser(new User{ Name = "robconery" });
            Assert.NotNull(result);
            var objResult = result as OkObjectResult;
            Assert.NotNull(objResult);
            var repoObj = result as IEnumerable<RepoDTO>;
            Assert.NotNull(repoObj);
            var repoResultList = repoObj.ToList();
            Assert.Equal(repoResultList.Count(), repos.Count());
            for (int i=0; i<repoResultList.Count(); i++)
            {
                Assert.Equal(repoResultList[i].Name, repos[i].Name);
                Assert.Equal(repoResultList[i].Full_Name, repos[i].Full_Name);
                Assert.Equal(repoResultList[i].Description, repos[i].Description);
                Assert.Equal(repoResultList[i].Stargazers_Count, repos[i].Stargazers_Count);
                Assert.Equal(repoResultList[i].SVN_Url, repos[i].SVN_Url);
                Assert.Equal(repoResultList[i].Owner.Name, repos[i].Owner.Name);
                Assert.Equal(repoResultList[i].Owner.Location, repos[i].Owner.Location);
                Assert.Equal(repoResultList[i].Owner.Avatar_Url, repos[i].Owner.Avatar_Url);
                Assert.Equal(repoResultList[i].Stargazers.First().Login, repos[i].Stargazers.First().Login);
                Assert.Equal(repoResultList[i].Stargazers.Last().Login, repos[i].Stargazers.Last().Login);
            }
        }
    }
}
