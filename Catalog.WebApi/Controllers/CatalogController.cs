using Catalog.Application.UseCases.Categories;
using Catalog.Application.UseCases.Category.Create;
using Catalog.Application.UseCases.Category.Update;
using Catalog.Application.UseCases.Priorities;
using Catalog.Application.UseCases.PriorityUpdate;
using Catalog.Application.UseCases.SpecificCategory;
using Catalog.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    
    [Authorize]
    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories([FromQuery] CategoriesRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response.Categories);
    }
    
    [Authorize]
    [HttpGet("category/{id}")]
    public async Task<IActionResult> SpecificCategory(Guid id)
    {
        var response = await _mediator.Send(new SpecificCategoryRequest(id));
        return Ok(response.SpecificCategory);
    }
    
    [Authorize]
    [HttpGet("categories/list")]
    public async Task<IActionResult> GetCategoriesForDropDownList([FromQuery] CategoriesRequest request)
    {
        var response = await _mediator.Send(request);
        return Ok(response.Categories);
    }
    
    [Authorize]
    [HttpPatch("categoryUpdate")]
    public async Task<IActionResult> CategoryUpdate([FromBody] CategoryUpdateRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [Authorize]
    [HttpPost("categoryCreate")]
    public async Task<IActionResult> CategoryCreate([FromBody] CategoryCreateRequest request)
    {
        var response = await _mediator.Send(request);
        if (response is null)
            return BadRequest();
        return Ok();
    }
    
    [Authorize]
    [HttpGet("priorities")]
    public async Task<IActionResult> GetPriorities()
    {
        var response = await _mediator.Send(new PrioritiesRequest());
        return Ok(response.Priorities);
    }
    
    [Authorize]
    [HttpPatch("priorityUpdate")]
    public async Task<IActionResult> PriorityUpdate([FromBody] List<PriorityUpdateDto> request)
    {
        var exchangeRequest = new PriorityUpdateRequest(request);
        var response = await _mediator.Send(exchangeRequest);
        if (response is null)
            return BadRequest();
        return Ok();
    }
}