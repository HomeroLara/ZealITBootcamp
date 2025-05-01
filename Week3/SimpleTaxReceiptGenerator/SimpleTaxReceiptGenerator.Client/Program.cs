using System.Diagnostics;
using SimpleTaxReceiptGenerator.Services;
using SimpleTaxReceiptGenerator.Services.Models;
using Spectre.Console;

// Ask which Simulator Service to use
var sim = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Which [green]Simulator Service[/]?")
        .PageSize(10)
        .MoreChoicesText("[grey](Use ↑↓ arrows to move and Enter to select)[/]")
        .AddChoices("SimulatorService", "SimulatorServiceImproved"));

// Ask whether to use string or StringBuilder for receipt formatting
var format = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Which [green]Receipt Generator[/] method?")
        .AddChoices("String", "StringBuilder"));

AnsiConsole.MarkupLine($"\n[bold yellow]Simulator Selected:[/] [aqua]{sim}[/]");
AnsiConsole.MarkupLine($"[bold yellow]Formatter Selected:[/] [aqua]{format}[/]\n");

var stopwatchGenerateReceipts = Stopwatch.StartNew();

// Generate Receipts
List<Receipt> receipts = sim switch
{
    "SimulatorService" => SimulatorService.GenerateReceipts(10000, 250),
    "SimulatorServiceImproved" => SimulatorServiceImproved.GenerateReceipts(10000, 250),
    _ => throw new InvalidOperationException("Unknown simulator selected.")
};

stopwatchGenerateReceipts.Stop();

var stopwatchGenerateReceiptText = Stopwatch.StartNew();

// Generate Receipt Output
var receiptService = new ReceiptService();
foreach (var receipt in receipts)
{
    var output = format.ToLower() == "string"
        ? receiptService.GenerateReceiptTextWithString(receipt)
        : receiptService.GenerateReceiptTextWithStringBuilder(receipt);

    // Comment this out for large runs
    // AnsiConsole.WriteLine(output);
}

stopwatchGenerateReceiptText.Stop();

// -------------------------------------------
// Stopwatch Summary Table
// -------------------------------------------
AnsiConsole.MarkupLine($"\n[bold yellow]Performance Summary:[/]");
var timingTable = new Table()
    .Border(TableBorder.Minimal)
    .AddColumn("[green]Operation[/]")
    .AddColumn("[cyan]Duration (ms)[/]");

timingTable.AddRow("GenerateReceipts()", $"{stopwatchGenerateReceipts.ElapsedMilliseconds}");
timingTable.AddRow("GenerateReceiptText()", $"{stopwatchGenerateReceiptText.ElapsedMilliseconds}");

AnsiConsole.Write(timingTable);

// -------------------------------------------
// Receipt Summary Table (First 5)
// -------------------------------------------
AnsiConsole.MarkupLine("\n[bold yellow]Receipt Summary (First 5):[/]");

var summaryTable = new Table()
    .Border(TableBorder.Rounded)
    .AddColumn("Receipt #")
    .AddColumn("Subtotal")
    .AddColumn("Tax")
    .AddColumn("Grand Total");

for (int i = 0; i < Math.Min(5, receipts.Count); i++)
{
    var r = receipts[i];
    summaryTable.AddRow(
        $"[blue]{i + 1}[/]",
        $"[green]{r.Subtotal:C}[/]",
        $"[yellow]{r.TotalTax:C}[/]",
        $"[bold]{r.GrandTotal:C}[/]"
    );
}

AnsiConsole.Write(summaryTable);

// -------------------------------------------
// Grand Totals Across All Receipts
// -------------------------------------------
decimal totalSubtotal = receipts.Sum(r => r.Subtotal);
decimal totalTax = receipts.Sum(r => r.TotalTax);
decimal totalGrand = receipts.Sum(r => r.GrandTotal);

AnsiConsole.MarkupLine("\n[bold yellow]Total Across All Receipts:[/]");
var totalTable = new Table()
    .Border(TableBorder.Square)
    .AddColumn("Total Subtotal")
    .AddColumn("Total Tax")
    .AddColumn("Total Grand Total");

totalTable.AddRow(
    $"[green]{totalSubtotal:C}[/]",
    $"[yellow]{totalTax:C}[/]",
    $"[bold]{totalGrand:C}[/]"
);

AnsiConsole.Write(totalTable);
