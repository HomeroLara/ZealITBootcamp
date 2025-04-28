using SimpleTaxReceiptGenerator.Services.Models;

namespace SimpleTaxReceiptGenerator.Services;


public static class SimulatorServiceImproved
{
    // Provides a thread-safe Random instance that may be used concurrently from any thread.
    // https://learn.microsoft.com/en-us/dotnet/api/system.random.shared?view=net-9.0
    private static readonly Random _random = Random.Shared; 
    private static readonly string[] SampleItemNames = new[]
    {
        "T-shirt", "Hat", "Mug", "Sticker", "Notebook", "Pen", "Keychain", "Water Bottle"
    };

    public static List<Receipt> GenerateReceipts(int numberOfReceipts, int itemsPerReceipt)
    {
        // init the list with the capacity to avoid resizing
        // this is a performance optimization - small but helps
        var receipts = new List<Receipt>(numberOfReceipts);

        for (int i = 0; i < numberOfReceipts; i++)
        {
            var receipt = new Receipt();
            
            // init the list with the capacity to avoid resizing
            // this is a performance optimization - small but helps
            receipt.Items = new List<Item>(itemsPerReceipt);

            for (int j = 0; j < itemsPerReceipt; j++)
            {
                // Create Name inline without _random.Next() twice
                var name = SampleItemNames[_random.Next(SampleItemNames.Length)];
                var price = Math.Round((decimal)(_random.NextDouble() * 50 + 5), 2); // Random price $5 - $55
                var tax = Math.Round((decimal)(_random.NextDouble() * 5), 2); // decimal. Random tax $0 - $5
                int quantity = _random.Next(1, 5); // Quantity between 1-4

                var item = new Item(
                    Id: j + 1,
                    Name: name,
                    Price: price,
                    SalesTax: tax,
                    Quantity: quantity
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