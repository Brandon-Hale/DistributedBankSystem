using BankDataServer.Models;
using System.Data.SQLite;

namespace BankDataServer.Data
{
    public class TransactionManager
    {
        private static string connection = "Data Source=bankdatabase.db;Version=3;";
        public static bool DropTable()
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();
                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = @"
                            DROP TABLE ""Transaction""";

                        command.ExecuteNonQuery();
                        c.Close();
                    }
                    Console.WriteLine("Tables Deleted");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return false;
        }
        public static bool CreateTables()
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();
                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = @"
                            CREATE TABLE ""Transaction"" (
                                transactionId INTEGER PRIMARY KEY,
                                amount REAL,
                                fromAccount INTEGER,
                                toAccount INTEGER,
                                FOREIGN KEY (fromAccount) REFERENCES BankAccount(accNo),
                                FOREIGN KEY (toAccount) REFERENCES BankAccount(accNo)
                        )";

                        command.ExecuteNonQuery();
                        c.Close();
                    }
                    Console.WriteLine("Tables Created Successfully");
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return false;
        }


        public static bool Insert(Transaction t) //inserting transaction correctly but not bank account
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO ""Transaction"" (transactionId, fromAccount, toAccount, amount)
                        VALUES (@transactionId, @fromAccount, @toAccount, @amount)"
                        ;

                        command.Parameters.AddWithValue("@transactionId", t.TransactionId);
                        command.Parameters.AddWithValue("@fromAccount", t.FromAccount);
                        command.Parameters.AddWithValue("@toAccount", t.ToAccount);
                        command.Parameters.AddWithValue("@amount", t.Amount);

                        int rowsInserted = command.ExecuteNonQuery();

                        c.Close();
                        if (rowsInserted > 0)
                        {
                            BankAccountManager.GetByAccNo(t.FromAccount).Balance -= t.Amount;
                            BankAccountManager.GetByAccNo(t.ToAccount).Balance += t.Amount;
                            return true;
                        }
                    }
                    c.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return false;
        }

        public static bool Delete(uint transactionId)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM \"Transaction\" WHERE transactionId = @transactionId";
                        command.Parameters.AddWithValue("@transactionId", transactionId);

                        int rowsDeleted = command.ExecuteNonQuery();

                        c.Close();
                        if (rowsDeleted > 0) { return true; }
                    }
                    c.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return false;
        }

        public static bool Update(Transaction t) //updating transaction correctly but not bank account
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = $"UPDATE \"Transaction\" SET amount = @amount, fromAccount = @fromAccount, toAccount = @toAccount WHERE transactionId = @transactionId";

                        command.Parameters.AddWithValue("@transactionId", t.TransactionId);
                        command.Parameters.AddWithValue("@fromAccount", t.FromAccount);
                        command.Parameters.AddWithValue("@toAccount", t.ToAccount);
                        command.Parameters.AddWithValue("@amount", t.Amount);

                        int rowsUpdated = command.ExecuteNonQuery();

                        c.Close();
                        if (rowsUpdated > 0)
                        {
                            BankAccountManager.GetByAccNo(t.FromAccount).Balance -= t.Amount;
                            BankAccountManager.GetByAccNo(t.ToAccount).Balance += t.Amount;
                            return true;
                        }
                    }

                    c.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return false;
        }

        public static List<Transaction> GetAll() //working
        {
            List<Transaction> ts = new List<Transaction>();

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM \"Transaction\"";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Transaction t = new Transaction();
                                t.TransactionId = Convert.ToUInt32(reader["transactionId"]);
                                t.FromAccount = Convert.ToUInt32(reader["fromAccount"]);
                                t.ToAccount = Convert.ToUInt32(reader["toAccount"]);
                                t.Amount = Convert.ToDouble(reader["amount"]);
                                ts.Add(t);
                            }
                        }
                    }
                    c.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ts;
        }

        public static Transaction GetById(uint transactionId) //working correctly
        {
            Transaction transaction = null;

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM \"Transaction\" WHERE transactionId = @transactionId";
                        command.Parameters.AddWithValue("@transactionId", transactionId);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Transaction t = new Transaction();
                                t.TransactionId = Convert.ToUInt32(reader["transactionId"]);
                                t.FromAccount = Convert.ToUInt32(reader["fromAccount"]);
                                t.ToAccount = Convert.ToUInt32(reader["toAccount"]);
                                t.Amount = Convert.ToDouble(reader["amount"]);
                                transaction = t;
                            }
                        }
                    }
                    c.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return transaction;
        }
    }
}
