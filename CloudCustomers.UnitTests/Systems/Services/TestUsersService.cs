using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CloudCustomers.Domain.Config;
using CloudCustomers.Domain.Models;
using CloudCustomers.Domain.Services;
using CloudCustomers.UnitTests.Fixtures;
using CloudCustomers.UnitTests.Helpers;
using Microsoft.Extensions.Options;
using Moq.Protected;

namespace CloudCustomers.UnitTests.Systems.Services;

public class TestUsersService
{
    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesHttpGetRequest()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "http://example.com/users";
        var mockHttpMessageHandler = MockHttpMessageHandler<User>
            .SetupBasicResourceList(expectedResponse);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        // Act
        await sut.GetAllUsers();

        // Assert
        mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async Task GetAllUsers_WhenHits404_ReturnsEmptyListOfUsers()
    {
        // Arrange
        var endpoint = "http://example.com/users";
        var mockHttpMessageHandler = MockHttpMessageHandler<User>.SetupReturn404();

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        result.Count
            .Should()
            .Be(0);
    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_ReturnsListOfUsersOfExpectedSize()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "http://example.com/users";

        var mockHttpMessageHandler = MockHttpMessageHandler<User>
            .SetupBasicResourceList(expectedResponse);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        // Act
        var result = await sut.GetAllUsers();

        // Assert
        result
            .Count
            .Should()
            .Be(expectedResponse.Count);
    }

    [Fact]
    public async Task GetAllUsers_WhenCalled_InvokesConfiguredExternalUrl()
    {
        // Arrange
        var expectedResponse = UsersFixture.GetTestUsers();
        var endpoint = "http://example.com/users";

        var mockHttpMessageHandler = MockHttpMessageHandler<User>
            .SetupBasicResourceList(expectedResponse);

        var httpClient = new HttpClient(mockHttpMessageHandler.Object);

        var config = Options.Create(new UsersApiOptions
        {
            Endpoint = endpoint
        });

        var sut = new UsersService(httpClient, config);

        // Act
        var result = await sut.GetAllUsers();

        var uri = new Uri(endpoint);

        // Assert
        mockHttpMessageHandler
            .Protected()
            .Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Get && request.RequestUri == uri),
                ItExpr.IsAny<CancellationToken>());
    }
}