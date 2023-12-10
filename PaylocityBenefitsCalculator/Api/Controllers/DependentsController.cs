using Api.Database;
using Api.Dtos.Dependent;
using Api.Dtos.Errors;
using Api.Logic;
using Api.Models;
using Api.Models.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DependentsController : ControllerBase
{
    private readonly Query _query;
    private readonly Command _command;

    public DependentsController(Query query, Command command)
    {
        _query = query;
        _command = command;
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

    [SwaggerOperation(Summary = "Add new dependent")]
    [HttpPost("")]
    public async Task<ActionResult<ApiResponse<GetDependentDto>>> Add(AddDependentDto dependentDto)
    {
        try
        {
            var created = await _command.AddDependent(dependentDto);

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
                Data = ex.Data,
                Message = ex.Message,
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
        var dependents = await _query.AllDependents();

        var result = new ApiResponse<List<GetDependentDto>>
        {
            Data = dependents,
            Success = true
        };

        return result;
    }
}