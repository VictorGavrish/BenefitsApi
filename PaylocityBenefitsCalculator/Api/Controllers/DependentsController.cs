using Api.Dtos.Dependent;
using Api.Models;
using Api.Models.Exceptions;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly DependentService _dependentService;

    public DependentsController(DependentService dependentService)
    {
        _dependentService = dependentService;
    }

    [SwaggerOperation(Summary = "Get dependent by id")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Get(int id)
    {
        var dependent = await _dependentService.GetDependent(id);

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

    [SwaggerOperation(Summary = "Add new dependent")]
    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Add(AddDependentDto dependentDto)
    {
        try
        {
            var created = await _dependentService.AddDependent(dependentDto);

            var result = new ApiResponse<GetDependentDto>
            {
                Data = created,
                Message = "dependent successfully created",
                Success = true
            };
            return result;
        }
        catch (ApiException ex)
        {
            var result = new ApiResponse<object>
            {
                Data = ex.ErrorData,
                Message = "An error occurred",
                Error = ex.Message,
                Success = false
            };
            return BadRequest(result);
        }
    }

    [SwaggerOperation(Summary = "Get all dependents")]
    [HttpGet("")]
    public async Task<ActionResult<ApiResponse<List<GetDependentDto>>>> GetAll()
    {
        var dependents = await _dependentService.GetAllDependents();

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }
}