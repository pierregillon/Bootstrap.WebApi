using System.ComponentModel.DataAnnotations;
using Bootstrap.Application;
using Bootstrap.BuildingBlocks;
using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.WebApi.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomerController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CustomerController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    [HttpPost]
    public async Task Post([Required] RegisterNewCustomerBody body) 
        => await _commandDispatcher.Dispatch(new RegisterNewCustomer(body.FirstName, body.LastName));

    [HttpGet]
    public async Task<IEnumerable<CustomerListItemDto>> Get() 
        => await _queryDispatcher.Dispatch(new ListCustomers());
}