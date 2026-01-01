using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ConsoleFinanceTracker
{
    public enum TransactionCategory
    {
        Food, Transport, Shopping, Entertainment, Other
    }
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TransactionCategory Category { get; set; }

        public Transaction()
        {
            Date = DateTime.Now;
        }

        public Transaction(decimal amount, string description, TransactionCategory category) : this()
        {
            Amount = amount;
            Description = description;
            Category = category;
        }

        public Transaction(decimal amount, string description, TransactionCategory category, DateTime date)
        {
            Amount = amount;
            Description = description;
            Category = category;
            Date = date;
        }

        public override string ToString()
        {
            return $"{Date:dd.MM.yyyy} | {Amount,9:C2} | {Category,-12} | {Description}";
        }
    }
}
