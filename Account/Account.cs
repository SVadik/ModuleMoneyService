using System;

namespace Accounts
{
    public class Account
    {
        public Account( int user_id, decimal balance = 0, long number = 0)
        {
            Number = number;
            UserId = user_id;
            Balance = balance;
        }
        public long Number { get; set; }

        public int UserId { get; set; }

        public decimal Balance { get; set; }

    }
}
