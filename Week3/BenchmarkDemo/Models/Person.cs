namespace BenchmarkDemo.Models;

public record Person(
    int Id,
    string FirstName,
    string LastName,
    DateTime BirthDate,
    decimal Salary,
    bool IsActive,
    string Department,
    string Email,
    string PhoneNumber
);