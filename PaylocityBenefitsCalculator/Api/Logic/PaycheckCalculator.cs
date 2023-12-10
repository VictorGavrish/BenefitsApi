using Api.Dtos.Employee;
using Api.Models;

namespace Api.Logic;

public class PaycheckCalculator
{
    private const int PaychecksPerYear = 26;
    
    // All figures converted to "per year"
    private const decimal BaseBenefits = 1000m * 12;
    private const decimal DependentBenefits = 600m * 12;
    private const decimal AdditionalBenefitsFloor = 80_000m;
    private const decimal AdditionalBenefitsForAge = 200m * 12;

    private const int AgeForAdditionalBenefitsForAge = 50;
    private const decimal AdditionalBenefitsFraction = 0.02m;
    
    public GetPaycheckDto GetPaycheck(GetEmployeeDto employee, DateTime time)
    {
        var salary = employee.Salary / PaychecksPerYear;
        var benefitDeductions = BaseBenefits / PaychecksPerYear;
        benefitDeductions += employee.Dependents.Count * DependentBenefits / PaychecksPerYear;
        if (employee.Salary > AdditionalBenefitsFloor)
        {
            benefitDeductions += employee.Salary * AdditionalBenefitsFraction / PaychecksPerYear;
        }

        foreach (var dependent in employee.Dependents)
        {
            var age = CalculateAge(dependent.DateOfBirth, time);
            if (age >= AgeForAdditionalBenefitsForAge)
            {
                benefitDeductions += AdditionalBenefitsForAge / PaychecksPerYear;
            }
        }

        var paycheck = new GetPaycheckDto
        {
            EmployeeId = employee.Id,
            Salary = salary,
            BenefitsDeductions = benefitDeductions
        };
        return paycheck;
    }

    private static int CalculateAge(DateTime dateOfBirth, DateTime now)
    {
        var age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month ||
            now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)
        {
            age--;
        }

        return age;
    }
}