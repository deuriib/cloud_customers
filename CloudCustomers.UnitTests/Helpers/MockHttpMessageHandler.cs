using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using CloudCustomers.Domain.Models;
using Moq.Protected;

namespace CloudCustomers.UnitTests.Helpers;

internal static class MockHttpMessageHandler<T>
{
    internal static Mock<HttpMessageHandler> SetupBasicResourceList(List<T> expectedResponse)
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(expectedResponse)),
        };
        
        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        return mockHttpMessageHandler;
    }

    internal static Mock<HttpMessageHandler> SetupReturn404()
    {
        var mockResponse = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(string.Empty),
        };
        
        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        return mockHttpMessageHandler;
    }
}