using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
