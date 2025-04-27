using BenchmarkDotNet.Attributes;
using SimpleTaxReceiptGenerator.Services.Models;
using SimpleTaxReceiptGenerator.Services;

namespace SimpleTaxReceiptGenerator.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class ReceiptGeneratorBenchmark
{
    private List<Receipt> _receipts;
    private ReceiptService _receiptService;

    // Number of receipts for different scales - can be adjusted
    [Params(100)] 
    public int NumberOfReceipts;

    // Items per receipt - can be adjusted
    [Params(20)] 
    public int ItemsPerReceipt;

    [GlobalSetup]
    public void Setup()
    {
        _receiptService = new ReceiptService();
        _receipts = SimulatorService.GenerateReceipts(NumberOfReceipts, ItemsPerReceipt);
    }

    [Benchmark]
    public void GenerateReceiptsWithString()
    {
        foreach (var receipt in _receipts)
        {
            var result = _receiptService.GenerateReceiptTextWithString(receipt);
            // Optionally: Do something with the result (don't optimize it away)
            Consume(result);
        }
    }

    [Benchmark]
    public void GenerateReceiptsWithStringBuilder()
    {
        foreach (var receipt in _receipts)
        {
            var result = _receiptService.GenerateReceiptTextWithStringBuilder(receipt);
            Consume(result);
        }
    }

    // Helper method to prevent compiler optimizations removing the code
    private void Consume(string text)
    {
        if (string.IsNullOrEmpty(text))
            throw new System.Exception("Invalid receipt text");
    }
}