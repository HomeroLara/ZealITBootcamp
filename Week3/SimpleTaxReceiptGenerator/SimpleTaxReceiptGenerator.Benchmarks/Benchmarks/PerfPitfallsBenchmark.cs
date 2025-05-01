using System.Text;
using BenchmarkDotNet.Attributes;
using SimpleTaxReceiptGenerator.Services.Models;

namespace SimpleTaxReceiptGenerator.Benchmarks.Benchmarks;

[MemoryDiagnoser]
public class PerfPitfallsBenchmark
{
    private List<Item> _items;
    private Dictionary<int, Item> _itemDict;
    private List<Receipt> _receipts;
    private readonly StringBuilder _sharedBuilder = new();
    private readonly int _targetId = 500;
    private decimal _value = 1234.56m;

    [GlobalSetup]
    public void Setup()
    {
        _items = new List<Item>(1000);
        _itemDict = new Dictionary<int, Item>(1000);
        _receipts = new List<Receipt>();

        for (int i = 1; i <= 1000; i++)
        {
            var item = new Item(i, "Item " + i, i * 1.5m, 0.1m, i % 5 + 1);
            _items.Add(item);
            _itemDict[i] = item;
        }

        for (int i = 0; i < 100; i++)
        {
            var receipt = new Receipt { Items = new List<Item>(_items) };
            _receipts.Add(receipt);
        }
    }

    [Benchmark]
    public decimal LinqSum() =>
        _items.Where(i => i.Quantity > 2).Sum(i => i.Price * i.Quantity);

    [Benchmark]
    public decimal LoopSum()
    {
        decimal total = 0;
        foreach (var item in _items)
        {
            if (item.Quantity > 2)
                total += item.Price * item.Quantity;
        }
        return total;
    }

    [Benchmark]
    public Item? FindInList() =>
        _items.FirstOrDefault(i => i.Id == _targetId);

    [Benchmark]
    public Item? FindInDictionary() =>
        _itemDict.TryGetValue(_targetId, out var item) ? item : null;

    [Benchmark]
    public string AllocateNewStringBuilder()
    {
        var sb = new StringBuilder();
        foreach (var item in _items)
            sb.Append(item.Name);
        return sb.ToString();
    }

    [Benchmark]
    public string ReuseStringBuilder()
    {
        _sharedBuilder.Clear();
        foreach (var item in _items)
            _sharedBuilder.Append(item.Name);
        return _sharedBuilder.ToString();
    }

    [Benchmark]
    public string FormatWithToString() =>
        _value.ToString("C");

    [Benchmark]
    public string FormatWithSpan()
    {
        Span<char> buffer = stackalloc char[32];
        _value.TryFormat(buffer, out int written, "C");
        return new string(buffer[..written]);
    }

    [Benchmark]
    public decimal TotalWithLoop() =>
        _receipts.Sum(r => r.GrandTotal);

    [Benchmark]
    public decimal TotalWithPLINQ() =>
        _receipts.AsParallel().Sum(r => r.GrandTotal);

    [Benchmark]
    public decimal WithToList() =>
        _items.Where(x => x.Price > 10).ToList().Sum(x => x.Price);

    [Benchmark]
    public decimal WithoutToList() =>
        _items.Where(x => x.Price > 10).Sum(x => x.Price);

    [Benchmark]
    public string SerializeWithSystemTextJson() =>
        System.Text.Json.JsonSerializer.Serialize(_receipts[0]);

    [Benchmark]
    public string SerializeWithNewtonsoftJson() =>
        Newtonsoft.Json.JsonConvert.SerializeObject(_receipts[0]);
}