using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts
{
    public class MoneyTransaction
    {
        public MoneyTransaction(Account fromAaccount, Account toAccount, decimal value)
        {
            Date = DateTime.Now;
            FromNumber = fromAaccount.Number;
            ToNumber = toAccount.Number;
            FromUserId= fromAaccount.UserId;
            ToUserId = toAccount.UserId;
            Amount = value;
        }

        public DateTime Date { get; set; }
        public long FromNumber { get; set; }
        public long ToNumber { get; set; }
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }
        public decimal Amount { get; set; }
        //public char Sign { get; set; }
    }
}
