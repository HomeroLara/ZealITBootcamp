using SimpleTaxReceiptGenerator.Services.Models;

namespace SimpleTaxReceiptGenerator.Services;


public static class SimulatorService
{
    private static readonly Random _random = new();

    private static readonly string[] SampleItemNames = new[]
    {
        "T-shirt", "Hat", "Mug", "Sticker", "Notebook", "Pen", "Keychain", "Water Bottle"
    };

    public static List<Receipt> GenerateReceipts(int numberOfReceipts, int itemsPerReceipt)
    {
        var receipts = new List<Receipt>();

        for (int i = 0; i < numberOfReceipts; i++)
        {
            var receipt = new Receipt();
            for (int j = 0; j < itemsPerReceipt; j++)
            {
                var item = new Item(
                    Id: j + 1,
                    Name: SampleItemNames[_random.Next(SampleItemNames.Length)],
                    Price: Math.Round((decimal)(_random.NextDouble() * 50 + 5), 2), // Random price $5 - $55
                    SalesTax: Math.Round((decimal)(_random.NextDouble() * 5), 2),  // Random tax $0 - $5
                    Quantity: _random.Next(1, 5) // Quantity between 1-4
                );

                receipt.Items.Add(item);
            }
            receipts.Add(receipt);
        }

        return receipts;
    }

    public static decimal CalculateTotalSales(List<Receipt> receipts)
    {
        return receipts.Sum(r => r.GrandTotal);
    }

    public static decimal CalculateTotalTaxes(List<Receipt> receipts)
    {
        return receipts.Sum(r => r.TotalTax);
    }
}