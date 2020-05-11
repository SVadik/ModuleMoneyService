using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Accounts
{
    public interface IAccountService
    {
        Account GetUserAccount(int userId, long number);
        void Fill(long number, decimal value);
        List<Account> GetUserAccounts(int userId);
        long Create(Account account);
        Account Get(long number);
        void Update(Account account);
        void Delete(long number);
    }

    public class AccountService : IAccountService
    {
        string connectionString = null;
        public AccountService(string conn)
        {

            connectionString = conn;
        }

        public Account GetUserAccount(int userId, long number)
        {
            using (IDbConnection db = Connection)
            {
                return db.QuerySingleOrDefault<Account>("SELECT user_id, balance, number FROM public.accounts WHERE number = @number AND user_Id = @userId", new { number, userId }); 
            }
        }

        internal IDbConnection Connection
        {
            get
            {
                return new NpgsqlConnection(connectionString);
            }
        }

        public void Fill(long number, decimal value)
        {
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "UPDATE public.accounts SET balance = balance + @Balance WHERE number = @Number";
                db.Execute(sqlQuery, new { number, value });
            }
        }

        public List<Account> GetUserAccounts(int userId)
        {
            using (IDbConnection db = Connection)
            {
                return db.Query<Account>("SELECT user_id, balance, number FROM public.accounts WHERE user_id = @userId", new { userId }).ToList();
            }
        }

        public long Create(Account account)
        {
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "INSERT INTO public.accounts (user_id, balance) VALUES(@UserId, @Balance); SELECT currval('accounts_number_seq') as bigint;";
                return db.Query<long>(sqlQuery, account).FirstOrDefault();

                //var sqlQuery = "INSERT INTO Users ( Username, Salt, PasswordHash) VALUES(@Username, @Salt, @PasswordHash); SELECT CAST(SCOPE_IDENTITY() as int)";
                //int? userId = db.Query<int>(sqlQuery, user).FirstOrDefault();
                //user.Id = userId.Value;
            }
        }

        public Account Get(long number)
        {
            using (IDbConnection db = Connection)
            {
                return db.QuerySingleOrDefault<Account>("SELECT user_id, balance, number FROM public.accounts WHERE number = @number", new { number });
            }
        }


        public void Update(Account account)
        {
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "UPDATE public.accounts SET balance = @Balance WHERE number = @Number";
                db.Execute(sqlQuery, account);
            }
        }

        public void Delete(long number)
        {
            using (IDbConnection db = Connection)
            {
                var sqlQuery = "DELETE FROM public.accounts WHERE number = @number";
                db.Execute(sqlQuery, new { number });
            }
        }
    }
}
