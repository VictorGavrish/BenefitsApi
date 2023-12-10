namespace Api.Dtos.Employee;

public class GetPaycheckDto
{
    public int EmployeeId { get; set; }
    public decimal Salary { get; set; }
    public decimal BenefitsDeductions { get; set; }
}