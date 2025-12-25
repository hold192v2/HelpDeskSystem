using Catalog.Application.UseCases.Categories;
using Catalog.Application.UseCases.Category.Create;
using Catalog.Application.UseCases.Category.Update;
using Catalog.Application.UseCases.Priorities;
using Catalog.Application.UseCases.PriorityUpdate;
using Catalog.Application.UseCases.SpecificCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebApi.Controllers;

[ApiController]
[Route("catalog")]
public class CatalogController: ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("getCategories")]
    public async Task<IActionResult> GetCategories([FromQuery] CategoriesRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response.Categories);
    }
    
    [HttpGet("specificCategory")]
    public async Task<IActionResult> SpecificCategory([FromQuery] SpecificCategoryRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response.SpecificCategory);
    }

    [HttpPatch("categoryUpdate")]
    public async Task<IActionResult> CategoryUpdate([FromBody] CategoryUpdateRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }

    [HttpPost("categoryCreate")]
    public async Task<IActionResult> CategoryCreate([FromBody] CategoryCreateRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }

    [HttpGet("priorities")]
    public async Task<IActionResult> GetPriorities()
    {
        var response = await _mediator.Send(new PrioritiesRequest());
        return Ok(response.Priorities);
    }

    [HttpPatch("priorityUpdate")]
    public async Task<IActionResult> PriorityUpdate([FromBody] PriorityUpdateRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
}