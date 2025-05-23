using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _controller = new UserController();
            // Clear static list before each test
            UserController.userlist.Clear();
        }

        [Fact]
        public void Index_ReturnsViewResult_WithAllUsers()
        {
            // Arrange
            UserController.userlist.Add(new User { Id = 1, Name = "Test User", Email = "test@example.com" });

            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var result = _controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsViewResult_WithUser()
        {
            // Arrange
            var testUser = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(testUser);

            // Act
            var result = _controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(testUser, viewResult.Model);
        }

        [Fact]
        public void Create_POST_AddsUser_WhenModelValid()
        {
            // Arrange
            var newUser = new User { Name = "New User", Email = "new@example.com" };

            // Act
            var result = _controller.Create(newUser);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            Assert.Single(UserController.userlist);
            Assert.Equal("New User", UserController.userlist.First().Name);
        }

        [Fact]
        public void Create_POST_ReturnsView_WhenModelInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");
            var invalidUser = new User();

            // Act
            var result = _controller.Create(invalidUser);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Edit_POST_UpdatesUser_WhenValid()
        {
            // Arrange
            var originalUser = new User { Id = 1, Name = "Original", Email = "original@example.com" };
            UserController.userlist.Add(originalUser);
            var updatedUser = new User { Name = "Updated", Email = "updated@example.com" };

            // Act
            var result = _controller.Edit(1, updatedUser);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Updated", UserController.userlist.First().Name);
        }

        [Fact]
        public void Delete_POST_RemovesUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test User", Email = "test@example.com" };
            UserController.userlist.Add(user);

            // Act
            var result = _controller.Delete(1, null);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Empty(UserController.userlist);
        }

        [Fact]
        public void Delete_POST_ReturnsNotFound_WhenMissing()
        {
            // Act
            var result = _controller.Delete(99, null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
