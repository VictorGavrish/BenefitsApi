using System;
using System.Collections.Generic;
using Api.Dtos.Dependent;
using Api.Dtos.Employee;
using Api.Logic;
using FluentAssertions;
using Xunit;

namespace ApiTests.UnitTests;

public class PaycheckCalculatorTests
{
    [Fact]
    public void WhenCalculatingPaycheck_ShouldGiveCorrectSalary()
    {
        var calc = new PaycheckCalculator();
        var dob = new DateTime(2000, 01, 01);
        var now = new DateTime(2024, 01, 01);
        var expectedSalary = 5000m;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            DateOfBirth = dob,
            FirstName = "TestName",
            LastName = "TestLastName",
            Salary = expectedSalary * 26
        };

        var paycheck = calc.GetPaycheck(employee, now);

        paycheck.Salary.Should().Be(expectedSalary);
    }

    [Fact]
    public void WhenCalculatingPaycheck_ShouldGive1000WithoutAdditionalThings()
    {
        var calc = new PaycheckCalculator();
        var dob = new DateTime(2000, 01, 01);
        var now = new DateTime(2024, 01, 01);
        // no dependents
        // salary less than $80_000
        // 1000 per month expected
        var expectedBenefits = 1000m * 12 / 26;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            DateOfBirth = dob,
            FirstName = "TestName",
            LastName = "TestLastName",
            Salary = 70_000m
        };

        var paycheck = calc.GetPaycheck(employee, now);

        paycheck.BenefitsDeductions.Should().Be(expectedBenefits);
    }

    [Fact]
    public void WhenCalculatingPaycheck_ShouldGive600AdditionalBenefitsPerDependent()
    {
        var calc = new PaycheckCalculator();
        var dob = new DateTime(2000, 01, 01);
        var now = new DateTime(2024, 01, 01);
        // 1 dependent
        // salary less than $80_000
        // 1000 + 600 per month expected
        var expectedBenefits = (1000m + 600m) * 12 / 26;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            DateOfBirth = dob,
            FirstName = "TestName",
            LastName = "TestLastName",
            Salary = 70_000m,
            Dependents = new List<GetDependentDto>
            {
                new()
                {
                    Id = 0,
                    FirstName = "Dep1",
                    LastName = "Dep1",
                    DateOfBirth = dob,
                    Relationship = Relationship.Spouse
                }
            }
        };

        var paycheck = calc.GetPaycheck(employee, now);

        paycheck.BenefitsDeductions.Should().Be(expectedBenefits);
    }

    [Fact]
    public void WhenCalculatingPaycheck_ShouldGive2PercentAdditionalBenefitsOver80000Salary()
    {
        var calc = new PaycheckCalculator();
        var dob = new DateTime(2000, 01, 01);
        var now = new DateTime(2024, 01, 01);
        // 0 dependents
        // salary over $80_000
        // 1000 per month + 2% of 90_000 per year expected
        var expectedBenefits = (1000m * 12 + 0.02m * 90_000m) / 26;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            DateOfBirth = dob,
            FirstName = "TestName",
            LastName = "TestLastName",
            Salary = 90_000m
        };

        var paycheck = calc.GetPaycheck(employee, now);

        paycheck.BenefitsDeductions.Should().Be(expectedBenefits);
    }
    
    

    [Fact]
    public void WhenCalculatingPaycheck_ShouldGive200AdditionalBenefitsPerDependentOver50()
    {
        var calc = new PaycheckCalculator();
        var dob = new DateTime(1974, 01, 01);
        var now = new DateTime(2024, 01, 01); // exactly 50 years old
        // 1 dependent over 50 years old
        // salary under $80_000
        // 1000 + 600 + 200 per month expected
        var expectedBenefits = (1000m + 600m + 200m) * 12 / 26;
        var employee = new GetEmployeeDto
        {
            Id = 1,
            DateOfBirth = dob,
            FirstName = "TestName",
            LastName = "TestLastName",
            Salary = 70_000m,
            Dependents = new List<GetDependentDto>
            {
                new()
                {
                    Id = 0,
                    FirstName = "Dep1",
                    LastName = "Dep1",
                    DateOfBirth = dob,
                    Relationship = Relationship.Spouse
                }
            }
        };

        var paycheck = calc.GetPaycheck(employee, now);

        paycheck.BenefitsDeductions.Should().Be(expectedBenefits);
    }
}