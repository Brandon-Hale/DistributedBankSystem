using BankDataServer.Data;
using System.Data.Entity.Core.Mapping;

namespace BankDataServer.Models.Generator
{
    public class Generate
    {
        public static List<User> users = new List<User>();
        public static List<BankAccount> bankAccounts = new List<BankAccount>();
        public static List<Transaction> transactions = new List<Transaction>();
        public static void GenerateData() //generates data for tables and creates tables
        {
            int numUsers = 5;
            int numAccountsPerUser = 2;
            int numTransactions = 20;

            UserManager.CreateTables();
            BankAccountManager.CreateTables();
            TransactionManager.CreateTables();

            for (int i = 0; i < numUsers; i++)
            {
                User user = GenerateEntries.GenerateRandomUser();
                users.Add(user);
            }
            foreach (User user in users)
            {
                for (int i = 0; i < numAccountsPerUser; i++)
                {
                    BankAccount bankAccount = GenerateEntries.GenerateRandomBankAccount(user.UserId);
                    bankAccounts.Add(bankAccount);
                }
            }

            for (int i = 0; i < numTransactions; i++)
            {
                Transaction transaction = GenerateEntries.GenerateRandomTransaction(bankAccounts);
                transactions.Add(transaction);
            }

            User admin = GenerateEntries.GenerateAdmin();
            users.Add(admin);

            foreach (User user in users)
            {
                UserManager.Insert(user);
            }
            foreach (BankAccount bankAccount in bankAccounts)
            {
                BankAccountManager.Insert(bankAccount);
            }
            foreach (Transaction transaction in transactions)
            {
                TransactionManager.Insert(transaction);
            }
        }

        public static void RemoveData() //removes previous data 
        {
            BankAccountManager.DropTable();
            UserManager.DropTable();
            TransactionManager.DropTable();
        }
    }
}
