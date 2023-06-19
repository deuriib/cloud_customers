using CloudCustomers.Domain.Models;

namespace CloudCustomers.Domain.Services;

public interface IUsersService
{
    Task<List<User>> GetAllUsers();
}