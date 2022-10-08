using Bootstrap.Application.Customers;
using Bootstrap.BuildingBlocks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers.V2;

[ApiController]
[Route("v{v:apiVersion}/customers")]
[ApiVersion("2.0")]
public class CustomerController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;

    public CustomerController(IQueryDispatcher queryDispatcher) => _queryDispatcher = queryDispatcher;

    [HttpGet]
    public async Task<IEnumerable<CustomerListItemDto>> ListCustomers()
        => await _queryDispatcher.Dispatch(new ListCustomersQuery());
}
