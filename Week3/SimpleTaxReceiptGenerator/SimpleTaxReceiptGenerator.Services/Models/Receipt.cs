namespace SimpleTaxReceiptGenerator.Services.Models;

public class Receipt
{
    public List<Item> Items { get; set; } = new List<Item>();

    public decimal Subtotal => Items.Sum(item => item.Price * item.Quantity);
    public decimal TotalTax => Items.Sum(item => item.SalesTax * item.Quantity);
    public decimal GrandTotal => Subtotal + TotalTax;
}