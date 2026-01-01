
namespace ConsoleFinanceTracker
{
    internal class Program
    {
        static void Main()
        {
            string fileName = "TransactionList.txt";

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "");    
                Console.WriteLine($"New transaction file created: {fileName}");
            }
            else
            {
                Console.WriteLine($"The existing file is used: {fileName}");
            }

            while (true)
            {
                Console.WriteLine("0 - Exit \n" +
                "1 - Type new transaction information \n" +
                "2 - Display all transactions \n");
                
                string? choice;
                choice = Console.ReadLine();

                switch (choice)
                {
                    case ("1"):
                        {
                            Console.WriteLine("Enter transaction information Example: <100> <Burger> ");
                            string[] newTransaction = Console.ReadLine().Split(' ');
                            for (int i = 0; i < newTransaction.Length; i++)
                            {
                                File.AppendAllText(fileName, newTransaction[i] + " ");
                                if (i%2 != 0)
                                    File.AppendAllText(fileName, "\n");
                            }
                            break;
                        }
                    case ("2"):
                        {
                            string content = File.ReadAllText(fileName);
                            Console.WriteLine(content);
                            break;
                        }
                    case ("0"):
                        {
                            return;
                        }
                }
            }

        }
    }
}
