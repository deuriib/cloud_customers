using CloudCustomers.Domain.Models;

namespace CloudCustomers.UnitTests.Fixtures;

internal static class UsersFixture
{
    internal static List<User> GetTestUsers()
    {
        return AutoFaker.Generate<List<User>>();
    }
}