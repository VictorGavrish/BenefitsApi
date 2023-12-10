using Dapper;
using Microsoft.Data.Sqlite;

namespace Api.Data;

public class DatabaseInit
{
    private readonly DatabaseConfig _config;

    public DatabaseInit(DatabaseConfig config)
    {
        _config = config;
    }

    public async Task Init()
    {
        using var connection = new SqliteConnection(_config.Name);

        var table = await connection.QueryAsync<string>(
            "SELECT name FROM sqlite_master WHERE type='table' AND name = 'Employee';");
        var tableName = table.FirstOrDefault();
        if (!string.IsNullOrEmpty(tableName) && tableName == "Employee")
        {
            return;
        }

        await connection.ExecuteAsync(InitSql);
    }

    private const string InitSql = @"
CREATE TABLE Employee (
    Id INTEGER PRIMARY KEY,
    FirstName TEXT NULL,
    LastName TEXT NULL,
    Salary DECIMAL(10,2) NOT NULL,
    DateOfBirth DATETIME NOT NULL
);

CREATE TABLE Dependent (
    Id INTEGER PRIMARY KEY,
    FirstName TEXT NULL,
    LastName TEXT NULL,
    DateOfBirth DATETIME NOT NULL,
    Relationship INTEGER NOT NULL, -- 0: None, 1: Spouse, 2: DomesticPartner, 3: Child
    EmployeeId INTEGER NOT NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employee(Id)
);

INSERT INTO Employee (Id, FirstName, LastName, Salary, DateOfBirth)
VALUES
    (1, 'LeBron', 'James', 75420.99, '1984-12-30'),
    (2, 'Ja', 'Morant', 92365.22, '1999-08-10'),
    (3, 'Michael', 'Jordan', 143211.12, '1963-2-17');

INSERT INTO Dependent (Id, FirstName, LastName, DateOfBirth, Relationship, EmployeeId)
VALUES
    (1, 'Spouse', 'Morant', '1998-03-03', 1, 2),
    (2, 'Child1', 'Morant', '2020-06-23', 3, 2),
    (3, 'Child2', 'Morant', '2021-05-18', 3, 2),
    (4, 'DP', 'Jordan', '1974-01-02', 2, 3);
";
}