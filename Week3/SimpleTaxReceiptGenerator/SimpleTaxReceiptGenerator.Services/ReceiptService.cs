using System.Text;
using SimpleTaxReceiptGenerator.Services.Models;

namespace SimpleTaxReceiptGenerator.Services;


public class ReceiptService
{
    public string GenerateReceiptTextWithString(Receipt receipt)
    {
        string text = "---- Receipt ----\n";

        foreach (var item in receipt.Items)
        {
            decimal lineTotal = item.Price * item.Quantity;
            decimal taxTotal = item.SalesTax * item.Quantity;
            text += $"{item.Quantity}x {item.Name} @ {item.Price:C} = {lineTotal:C} (Tax: {taxTotal:C})\n";
        }

        text += "\n";
        text += $"Subtotal: {receipt.Subtotal:C}\n";
        text += $"Tax: {receipt.TotalTax:C}\n";
        text += $"Grand Total: {receipt.GrandTotal:C}\n";

        return text;
    }

    public string GenerateReceiptTextWithStringBuilder(Receipt receipt)
    {
        var builder = new StringBuilder();
        builder.AppendLine("---- Receipt ----");

        foreach (var item in receipt.Items)
        {
            decimal lineTotal = item.Price * item.Quantity;
            decimal taxTotal = item.SalesTax * item.Quantity;
            builder.AppendLine($"{item.Quantity}x {item.Name} @ {item.Price:C} = {lineTotal:C} (Tax: {taxTotal:C})");
        }

        builder.AppendLine();
        builder.AppendLine($"Subtotal: {receipt.Subtotal:C}");
        builder.AppendLine($"Tax: {receipt.TotalTax:C}");
        builder.AppendLine($"Grand Total: {receipt.GrandTotal:C}");

        return builder.ToString();
    }
}