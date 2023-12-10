using Api.Database;
using Api.Dtos.Dependent;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly Query _query;
    public DependentsController(Query query)
    {
        _query = query;   
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _query.Dependent(id);

        if (dependent == null)
        {
            return NotFound();
        }
        
        var result = new ApiResponse<GetDependentDto>
        {
            Data = dependent,
            Success = true
        };

        return result;
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _query.AllDependents();
        
        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }
}
