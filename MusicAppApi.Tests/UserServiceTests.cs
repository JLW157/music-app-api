using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using MusicAppApi.Core.Services;
using MusicAppApi.Core;
using MusicAppApi.Models.DbModels;
using System.Threading;
using MusicAppApi.Core.interfaces.Services;
using Range = System.Range;

public class UserServiceTests
{

    [Fact]
    public async Task GetUserById_ValidId_ReturnsUser()
    {
        // Arrange
        var expectedUserId = Guid.NewGuid();
        var expectedUser = new User { Id = expectedUserId };

        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(us => us.GetUserById(expectedUserId)).Returns(Task.FromResult(expectedUser));
        var userService = mockUserService.Object;

        // Act
        var actual = await userService.GetUserById(expectedUserId);

        //var res = It.IsAny<User>();
        // Assert
        Assert.True(actual != null);
        Assert.IsType<User>(actual);
        Assert.Equal(expectedUser.Id, actual.Id);
        Assert.Equal(expectedUser, actual);
    }

    [Fact]
    public async Task GetUserById_InvalidId_ReturnsNull()
    {
        // Arrange
        var expectedUserId = Guid.NewGuid();
        var expectedUser = new User { Id = expectedUserId };

        var mockUserService = new Mock<IUserService>();
        mockUserService.Setup(us => us.GetUserById(expectedUserId)).Returns(Task.FromResult<User>(null));
        var userService = mockUserService.Object;

        // Act
        var actual = await userService.GetUserById(expectedUserId);

        //var res = It.IsAny<User>();
        // Assert
        Assert.True(actual == null);
        Assert.IsNotType<User>(actual);
    }

}
