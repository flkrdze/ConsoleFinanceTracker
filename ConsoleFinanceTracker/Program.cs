
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConsoleFinanceTracker
{
    internal class Program
    {

        static List<Transaction> transactions = new List<Transaction>();

        static string fileName = "transactions.json"; 
        static void Main()
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            LoadTransactions();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Finance Tracker ===");
                Console.WriteLine("1. Add transaction");
                Console.WriteLine("2. Display all transactions");
                Console.WriteLine("3. Display statistic");
                Console.WriteLine("4. Find transaction");
                Console.WriteLine("5. Save and exit");
                Console.WriteLine("0. No-save exit");
                Console.Write("\nDo your next move: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        AddNewTransaction();
                        break;
                    case "2":
                        ShowAllTransactions();
                        break;
                    case "3":
                        ShowStatistics();
                        break;
                    case "4":
                        SearchTransactions();
                        break;
                    case "5":
                        SaveTransactions();
                        Console.WriteLine("Data has been saved. Goodbye!");
                        return;
                    case "0":
                        Console.WriteLine("No-save exit. Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Not correct answer!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }

        }

        static void LoadTransactions()
        {
            try
            {
                if (File.Exists(fileName))
                {
                    string json = File.ReadAllText(fileName);

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true 
                    };

                    transactions = JsonSerializer.Deserialize<List<Transaction>>(json, options)
                                 ?? new List<Transaction>();

                    Console.WriteLine($"Loaded {transactions.Count} transactions");

                    foreach (var t in transactions)
                    {
                        Console.WriteLine($"  - Amount: {t.Amount}, Desc: {t.Description}, Date: {t.Date}, Cat: {t.Category}");
                    }
                }
                else
                {
                    Console.WriteLine("File not found.");
                    transactions = new List<Transaction>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                transactions = new List<Transaction>();
            }
        }

        static void SaveTransactions()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(transactions, options);
                File.WriteAllText(fileName, json);
                Console.WriteLine($"Saved {transactions.Count} transactions");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error by saving: {ex.Message}");
            }
        }

        static void AddNewTransaction()
        {
            Console.WriteLine("\n--- New transaction ---");

            decimal amount;
            while (true)
            {
                Console.Write("Type sum: ");
                if (decimal.TryParse(Console.ReadLine(), out amount))
                    break;
                Console.WriteLine("Error! Type correct value (like: 150.50)");
            }
            Console.Write("Введите описание: ");
            string description = Console.ReadLine()?.Trim() ?? "No description";

            Console.WriteLine("\nCategories:");
            Console.WriteLine("1. Food");
            Console.WriteLine("2. Transport");
            Console.WriteLine("3. Entertainment");
            Console.WriteLine("4. Shopping");
            Console.WriteLine("5. Other");
            Console.Write("Change category (1-5): ");

            TransactionCategory category = TransactionCategory.Other;
            string categoryChoice = Console.ReadLine();
            category = categoryChoice switch
            {
                "1" => TransactionCategory.Food,
                "2" => TransactionCategory.Transport,
                "3" => TransactionCategory.Entertainment,
                "4" => TransactionCategory.Shopping,
                _ => TransactionCategory.Other
            };

            var transaction = new Transaction(amount, description, category);
            transactions.Add(transaction);

            SaveTransactions();

            Console.WriteLine($"\n✓ Transaction added: {transaction}");
        }
        static void ShowAllTransactions()
        {
            Console.WriteLine($"\n--- All trasaction ({transactions.Count}) ---");

            if (transactions.Count == 0)
            {
                Console.WriteLine("There is no transactions.");
                return;
            }

            var sorted = transactions.OrderByDescending(t => t.Date).ToList();

            Console.WriteLine("Date       |     Sum   | Category    | Description");
            Console.WriteLine("-----------|-----------|-------------|------------");

            foreach (var transaction in sorted)
            {
                Console.WriteLine(transaction);
            }

            decimal total = transactions.Sum(t => t.Amount);
            Console.WriteLine($"\nTota: {total:C2}");
        }

        static void ShowStatistics()
        {
            Console.WriteLine("\n--- Statistic ---");

            if (transactions.Count == 0)
            {
                Console.WriteLine("No data for analyze.");
                return;
            }

            decimal total = transactions.Sum(t => t.Amount);
            decimal average = transactions.Average(t => t.Amount);
            decimal max = transactions.Max(t => t.Amount);
            decimal min = transactions.Min(t => t.Amount);

            Console.WriteLine($"Count of transations: {transactions.Count}");
            Console.WriteLine($"Total sum: {total:C2}");
            Console.WriteLine($"Average sum: {average:C2}");
            Console.WriteLine($"Max: {max:C2}");
            Console.WriteLine($"Min: {min:C2}");

            Console.WriteLine("\nBy category:");
            var byCategory = transactions
                .GroupBy(t => t.Category)
                .Select(g => new { Category = g.Key, Total = g.Sum(t => t.Amount), Count = g.Count() })
                .OrderByDescending(x => x.Total);

            foreach (var group in byCategory)
            {
                Console.WriteLine($"  {group.Category,-12}: {group.Total,8:C2} ({group.Count} шт.)");
            }
        }

        static void SearchTransactions()
        {
            Console.WriteLine("\n--- Find transaction ---");
            Console.Write("Type text for search (Description): ");
            string searchText = Console.ReadLine()?.ToLower() ?? "";

            var results = transactions
                .Where(t => t.Description.ToLower().Contains(searchText))
                .OrderByDescending(t => t.Date)
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No data.");
                return;
            }

            Console.WriteLine($"\n {results.Count} transations found:");
            foreach (var transaction in results)
            {
                Console.WriteLine(transaction);
            }
        }
    }
}
