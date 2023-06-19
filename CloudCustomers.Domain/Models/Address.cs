namespace CloudCustomers.Domain.Models;

public record Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
}