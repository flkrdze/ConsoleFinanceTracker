using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleFinanceTracker
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime Date {  get; set; }

        public Transaction(decimal amount, string descrption)
        {
            Amount = amount;
            Description = descrption;
        }

        public override string ToString()
        {
            return $"{Date} | {Amount} | {Description}";
        }
    }
}
