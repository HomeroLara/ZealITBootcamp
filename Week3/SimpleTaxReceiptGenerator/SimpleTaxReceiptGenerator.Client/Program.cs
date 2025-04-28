using SimpleTaxReceiptGenerator.Services.Models;
using SimpleTaxReceiptGenerator.Services;

// var receipts = SimulatorService.GenerateReceipts(100,25);
// var receiptService = new ReceiptService();
// foreach (var receipt in receipts)
// {
//     var result = receiptService.GenerateReceiptTextWithString(receipt);
//     Console.WriteLine(result);
// }

var receipts = SimulatorServiceImproved.GenerateReceipts(100, 25);
var receiptService = new ReceiptService();
foreach (var receipt in receipts)
{
    var result = receiptService.GenerateReceiptTextWithStringBuilder(receipt);
    Console.WriteLine(result);
}