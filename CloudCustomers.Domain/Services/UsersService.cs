using System.Net;
using System.Net.Http.Json;
using CloudCustomers.Domain.Config;
using CloudCustomers.Domain.Models;
using Microsoft.Extensions.Options;

namespace CloudCustomers.Domain.Services;

public class UsersService : IUsersService
{
    private readonly HttpClient _httpClient;
    private readonly UsersApiOptions _config;

    public UsersService(HttpClient httpClient, IOptions<UsersApiOptions> config)
    {
        _httpClient = httpClient;
        _config = config.Value;
    }

    public async Task<List<User>> GetAllUsers()
    {
        var userResponse = await _httpClient.GetAsync(_config.Endpoint);
        
        if (userResponse.StatusCode == HttpStatusCode.NotFound)
            return new List<User>();

        var responseContent = userResponse.Content;
        var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();

        return allUsers ?? new List<User>();
    }
}