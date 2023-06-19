using CloudCustomers.API.Controllers;
using CloudCustomers.Domain.Models;
using CloudCustomers.Domain.Services;
using CloudCustomers.UnitTests.Fixtures;
using Microsoft.AspNetCore.Mvc;

namespace CloudCustomers.UnitTests.Systems.Controllers;

public class TestUsersController
{
    [Fact]
    public async Task Get_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers);
        
        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = (OkObjectResult) await sut.Get();
        
        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task Get_OnSuccess_InvokesUsersServiceExactlyOnce()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        
        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = await sut.Get();
        
        // Assert
        mockUsersService.Verify(
            service => service.GetAllUsers(),
            Times.Once);
    }
    
    [Fact]
    public async Task Get_OnSuccess_ReturnsListOfUser()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(UsersFixture.GetTestUsers);
        
        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = await sut.Get();
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var objectResult = (OkObjectResult) result;
        objectResult.Value.Should().BeOfType<List<User>>();
    }
    
    [Fact]
    public async Task Get_OnNoUsersFound_Returns404()
    {
        // Arrange
        var mockUsersService = new Mock<IUsersService>();
        mockUsersService
            .Setup(service => service.GetAllUsers())
            .ReturnsAsync(new List<User>());
        
        var sut = new UsersController(mockUsersService.Object);

        // Act
        var result = await sut.Get();
        
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}