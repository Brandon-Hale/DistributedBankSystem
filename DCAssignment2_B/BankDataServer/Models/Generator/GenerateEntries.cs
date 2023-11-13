using BankDataServer.Models;

namespace BankDataServer.Models.Generator
{
    public class GenerateEntries
    {
        private static readonly Random _random = new Random();

        public static User GenerateRandomUser()
        {
            return new User
            {
                UserId = GenerateRandomUserId(),
                Name = GenerateRandomName(),
                Email = GenerateRandomEmail(),
                Address = GenerateRandomAddress(),
                PhoneNo = GenerateRandomPhoneNumber(),
                Password = GenerateRandomPassword(),
                UserType = "user"
            };
        }

        public static User GenerateAdmin()
        {
            return new User
            {
                UserId = GenerateRandomUserId(),
                Name = "Admin",
                Email = "admin@admin.com",
                Address = GenerateRandomAddress(),
                PhoneNo = GenerateRandomPhoneNumber(),
                Password = "admin",
                UserType = "admin"
            };
        }

        public static BankAccount GenerateRandomBankAccount(uint userId)
        {
            return new BankAccount
            {
                AccNo = GenerateRandomAccountNumber(),
                UserId = userId,
                Balance = _random.Next(100, 10000) // Random initial balance between 100 and 10000
            };
        }

        public static Transaction GenerateRandomTransaction(List<BankAccount> accounts)
        {
            uint fromAccount = accounts[_random.Next(0, accounts.Count)].AccNo;
            uint toAccount = accounts[_random.Next(0, accounts.Count)].AccNo;
            while (toAccount == fromAccount)
            {
                toAccount = accounts[_random.Next(accounts.Count)].AccNo;
            }

            return new Transaction
            {
                TransactionId = GenerateRandomTransactionId(),
                FromAccount = fromAccount,
                ToAccount = toAccount,
                Amount = _random.Next(10, 500)
            };
        }

        private static uint GenerateRandomUserId()
        {
            return (uint)_random.Next(1000, 9999);
        }

        private static string GenerateRandomName()
        {
            string[] names = { "Alice", "Bob", "Charlie", "David", "Eva", "Frank", "Grace", "Harry", "Ivy", "Jack" };
            return names[_random.Next(names.Length)];
        }

        private static string GenerateRandomEmail()
        {
            return $"user{_random.Next(1, 100)}@gmail.com.au";
        }

        private static string GenerateRandomAddress()
        {
            return $"Address{_random.Next(1, 1000)}"; // Random address
        }

        private static uint GenerateRandomPhoneNumber()
        {
            return (uint)_random.Next(10000000, 99999999); // 10-digit random phone number
        }

        private static string GenerateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8); // Random password
        }

        private static uint GenerateRandomAccountNumber()
        {
            return (uint)_random.Next(100000, 999999); // 6-digit random account number
        }

        private static uint transactionIdCounter = 1000;
        private static uint GenerateRandomTransactionId()
        {
            return transactionIdCounter++; // 4-digit random transaction ID
        }
    }
}
