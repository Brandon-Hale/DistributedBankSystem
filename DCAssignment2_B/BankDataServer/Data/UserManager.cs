using BankDataServer.Models;
using System.Data.SQLite;

namespace BankDataServer.Data
{
    public class UserManager
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
                            DROP TABLE User";

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
                            CREATE TABLE User (
                                userId INTEGER PRIMARY KEY,
                                name TEXT,
                                email TEXT,
                                address TEXT,
                                phoneNo INTEGER,
                                password TEXT,
                                userType TEXT
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

        public static bool Insert(User user)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = @"
                        INSERT INTO User (userId, name, email, address, phoneNo, password, userType)
                        VALUES (@userId, @name, @email, @address, @phoneNo, @password, @userType)";

                        command.Parameters.AddWithValue("@userId", user.UserId);
                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@address", user.Address);
                        command.Parameters.AddWithValue("@phoneNo", user.PhoneNo);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@userType", user.UserType);

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

        public static bool Delete(uint UserId)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = $"DELETE FROM User WHERE userId = @userId";
                        command.Parameters.AddWithValue("@userId", UserId);

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

        public static bool Update(User user)
        {
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = $"UPDATE User SET name = @name, email = @email, address = @address, phoneNo = @phoneNo, password = @password, userType = @userType WHERE userId = @userId";

                        command.Parameters.AddWithValue("@userId", user.UserId);
                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@address", user.Address);
                        command.Parameters.AddWithValue("@phoneNo", user.PhoneNo);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@userType", user.UserType);

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

        public static List<User> GetAll()
        {
            List<User> users = new List<User>();

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User";

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User();
                                user.UserId = Convert.ToUInt32(reader["userId"]);
                                user.Name = reader["name"].ToString();
                                user.Email = reader["email"].ToString();
                                user.Address = reader["address"].ToString();
                                user.PhoneNo = Convert.ToUInt64(reader["phoneNo"]);
                                user.Password = reader["password"].ToString();
                                user.UserType = reader["userType"].ToString();

                                users.Add(user);
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
            return users;
        }

        public static User GetById(uint id)
        {
            User user = null;

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User WHERE userId = @userId";
                        command.Parameters.AddWithValue("@userId", id);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User();
                                user.UserId = Convert.ToUInt32(reader["userId"]);
                                user.Name = reader["name"].ToString();
                                user.Email = reader["email"].ToString();
                                user.Address = reader["address"].ToString();
                                user.PhoneNo = Convert.ToUInt64(reader["phoneNo"]);
                                user.Password = reader["password"].ToString();
                                user.UserType = reader["userType"].ToString();
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

            return user;
        }

        public static User GetByEmail(string email)
        {
            User user = null;

            try
            {
                using (SQLiteConnection c = new SQLiteConnection(connection))
                {
                    c.Open();

                    using (SQLiteCommand command = c.CreateCommand())
                    {
                        command.CommandText = "SELECT * FROM User WHERE email = @email";
                        command.Parameters.AddWithValue("@email", email);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User();
                                user.UserId = Convert.ToUInt32(reader["userId"]);
                                user.Name = reader["name"].ToString();
                                user.Email = reader["email"].ToString();
                                user.Address = reader["address"].ToString();
                                user.PhoneNo = Convert.ToUInt64(reader["phoneNo"]);
                                user.Password = reader["password"].ToString();
                                user.UserType = reader["userType"].ToString();
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

            return user;
        }
    }
}
