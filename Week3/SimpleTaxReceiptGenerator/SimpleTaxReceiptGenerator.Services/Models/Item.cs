namespace SimpleTaxReceiptGenerator.Services.Models;

public record Item
(
    int Id,
    string Name,
    decimal Price,
    decimal SalesTax,
    int Quantity
);