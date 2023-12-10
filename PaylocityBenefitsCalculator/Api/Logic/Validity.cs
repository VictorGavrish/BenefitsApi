using Api.Database;
using Api.Dtos.Dependent;
using Api.Models;

namespace Api.Logic;

public class Validity
{
    private readonly DatabaseConnection _connection;

    public Validity(DatabaseConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Tests whether this dependent can be currently added.
    /// </summary>
    /// <param name="dependentDto">The dependent to add</param>
    /// <returns>Validation errors. Empty if no errors were found.</returns>
    public async Task<List<string>> ValidateNewDependent(AddDependentDto dependentDto)
    {
        var validationMessages = new List<string>();
        var employee = await _connection.GetEmployee(dependentDto.EmployeeId);
        if (employee == null)
        {
            validationMessages.Add($"No employee with employeeId {dependentDto.EmployeeId} found");
            return validationMessages;
        }

        if (!employee.Dependents.Any())
        {
            // No existing dependents, can proceed
            return validationMessages;
        }

        if (dependentDto.Relationship == Relationship.Child)
        {
            // Can have as many children as possible; can proceed
            return validationMessages;
        }

        if (dependentDto.Relationship is Relationship.Spouse or Relationship.DomesticPartner
            && employee.Dependents.Any(d => d.Relationship is Relationship.Spouse or Relationship.DomesticPartner))
        {
            validationMessages.Add("Only one spouse or domestic partner is allowed");
        }

        return validationMessages;
    }
}