using Bootstrap.Domain.Customers;
using Bootstrap.Tests.Unit.Builders;
using FluentAssertions;
using Xunit;

namespace Bootstrap.Tests.Unit.Customers;

public class CustomerTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Renaming_a_customer_requires_a_non_empty_first_name(string emptyFirstName)
    {
        Customer customer = A.Customer;

        var action = () => customer.Rename(emptyFirstName, "some last name");

        action
            .Should()
            .Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Renaming_a_customer_requires_a_non_empty_last_name(string emptyLastName)
    {
        Customer customer = A.Customer;

        var action = () => customer.Rename("some first name", emptyLastName);

        action
            .Should()
            .Throw<ArgumentException>();
    }
}
