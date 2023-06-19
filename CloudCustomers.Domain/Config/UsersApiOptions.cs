using System.ComponentModel.DataAnnotations;

namespace CloudCustomers.Domain.Config;

public class UsersApiOptions
{
    [Required, DataType(DataType.Url)]
    public string Endpoint { get; init; } = String.Empty;
}