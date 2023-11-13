using BankDataServer.Models;
using System.Data.SQLite;

namespace BankDataServer.Data
{
    public class BankAccountManager
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
                            DROP TABLE BankAccount";

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
                            CREATE TABLE BankAccount (
                                accNo INTEGER PRIMARY KEY,
                                balance REAL,
                                userId INTEGER,
                                FOREIGN KEY(userId) REFERENCES User(userId)
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

        public static bool Insert(BankAccount acc)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO BankAccount (accNo, balance, userId)
                        VALUES (@accNo, @balance, @userId)";

                        command.Parameters.AddWithValue("@accNo", acc.AccNo);
                        command.Parameters.AddWithValue("@balance", acc.Balance);
                        command.Parameters.AddWithValue("@userId", acc.UserId);

                        int rowsInserted = command.ExecuteNonQuery();

                        c.Close();
                        if (rowsInserted > 0)
                        {
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

        public static bool Delete(uint accNo)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM BankAccount WHERE accNo = @accNo";
                        command.Parameters.AddWithValue("@accNo", accNo);

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

        public static bool Update(BankAccount acc)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = $"UPDATE BankAccount SET balance = @balance WHERE accNo = @accNo";

                        command.Parameters.AddWithValue("@balance", acc.Balance);
                        command.Parameters.AddWithValue("@accNo", acc.AccNo);

                        int rowsUpdated = command.ExecuteNonQuery();

                        c.Close();
                        if (rowsUpdated > 0) { return true; }
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

        public static List<BankAccount> GetAll()
        {
            List<BankAccount> accs = new List<BankAccount>();

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM BankAccount";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BankAccount acc = new BankAccount();
                                acc.AccNo = Convert.ToUInt32(reader["accNo"]);
                                acc.Balance = Convert.ToUInt32(reader["balance"]);
                                acc.UserId = Convert.ToUInt32(reader["userId"]);
                                accs.Add(acc);
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
            return accs;
        }

        public static BankAccount GetByAccNo(uint accNo)
        {
            BankAccount account = null;
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM BankAccount WHERE accNo = @accNo";
                        command.Parameters.AddWithValue("@accNo", accNo);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                BankAccount acc = new BankAccount();
                                acc.AccNo = Convert.ToUInt32(reader["accNo"]);
                                acc.UserId = Convert.ToUInt32(reader["userId"]);
                                acc.Balance = Convert.ToUInt32(reader["balance"]);
                                account = acc;
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

            return account;
        }

        public static void DBInitialise()
        {
            //need to give fake data;
        }
    }
}
