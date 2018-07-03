using System;
using MySql.Data.MySqlClient;

namespace ATMSim.Models
{
    public class DBHelper
    {
        private MySqlConnection connection;
        private MySqlCommand command;
        private string server;
        private string database;
        private string uid;
        private string password;

        // constructor
        public DBHelper()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "localhost";
            database = "atmsim_db";
            uid = "malinda";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";SSLMode=None";

            connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        /// Check entered account number aginst the db
        /// </summary>
        /// <param name="accNo">Entered account number</param>
        /// <returns></returns>
        public bool CheckAccountNo(string accNo)
        {
            try
            {
                connection.Open();
                string query = string.Concat(@"SELECT acc_no FROM account WHERE acc_no=", accNo);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();

                string _accNo = "";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _accNo = reader.GetString(0);
                    }
                }

                if (accNo.Equals(_accNo))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("SQL ERROR: ", ex.Message));
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// verify pin with account number aginst the db
        /// </summary>
        /// <param name="accNo">Entered account number</param>
        /// <param name="pin">Entered pin</param>
        /// <returns></returns>
        public bool VerifyPin(string accNo, string pin)
        {
            try
            {
                connection.Open();
                string query = string.Concat(@"SELECT pin FROM account WHERE acc_no=", accNo);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();

                string _pin = "";
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _pin = reader.GetString(0);
                    }
                }

                if (pin.Equals(_pin))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("SQL ERROR: ", ex.Message));
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Get user details for specific account
        /// </summary>
        /// <param name="accNo">Entered account number</param>
        /// <returns></returns>
        public User GetUser(string accNo)
        {
            try
            {
                connection.Open();
                string query = string.Concat(@"SELECT u.* FROM user AS u INNER JOIN account AS acc ON u.user_id = acc.user_id AND acc.acc_no=", accNo);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();

                User user = new User();
                
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.User_Id = reader.GetString(0);
                        user.Name = reader.GetString(1);
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("SQL ERROR: ", ex.Message));
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Get account balance for specific account
        /// </summary>
        /// <param name="accNo">Entered account number</param>
        /// <returns></returns>
        public double GetBalance(string accNo)
        {
            try
            {
                connection.Open();
                string query = string.Concat(@"SELECT amount FROM account WHERE acc_no=", accNo);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();

                Account account = new Account();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        account.Balance = reader.GetDouble(0);
                    }
                }
                return account.Balance;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("SQL ERROR: ", ex.Message));
                return -1;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Withdraw amount from specific account
        /// </summary>
        /// <param name="accNo">Transaction object</param>
        /// <returns></returns>
        public ReturnResult Withdraw(Transaction transaction)
        {
            try
            {
                connection.Open();
                string query = string.Concat(@"SELECT amount FROM account WHERE acc_no=", transaction.Acc_No);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();

                Account account = new Account();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        account.Balance = reader.GetDouble(0);
                    }
                }

                if (account.Balance > 0)
                {
                    if (account.Balance >= transaction.Amount)
                    {
                        // Update amount in accounts table
                        query = "UPDATE account SET amount=@amount WHERE acc_no=@acc_no";
                        command = new MySqlCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@amount", account.Balance - transaction.Amount);
                        command.Parameters.AddWithValue("@acc_no", transaction.Acc_No);
                        command.Connection = connection;
                        command.ExecuteNonQuery();

                        // Add transaction entry to transaction table
                        query = "INSERT INTO transaction(user_id,acc_no,timestamp,type,amount) VALUES(@user_id,@acc_no,@timestamp,@type,@amount)";
                        command = new MySqlCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@user_id", transaction.User_Id);
                        command.Parameters.AddWithValue("@acc_no", transaction.Acc_No);
                        command.Parameters.AddWithValue("@timestamp", transaction.Timestamp);
                        command.Parameters.AddWithValue("@type", transaction.Type);
                        command.Parameters.AddWithValue("@amount", transaction.Amount);
                        command.Connection = connection;
                        command.ExecuteNonQuery();

                        return ReturnResult.Success;
                    }
                    else
                    {
                        return ReturnResult.AmountIsGreater;
                    }
                }
                else if (account.Balance == 0)
                {
                    return ReturnResult.IsEmpty;
                }
                else
                {
                    return ReturnResult.Error;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("SQL ERROR: ", ex.Message));
                return ReturnResult.Error;
            }
            finally
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Transfer amount to specific account
        /// </summary>
        /// <param name="accNo">Transaction object</param>
        /// <returns>Success</returns>
        public ReturnResult Transfer(Transaction transaction)
        {
            try
            {
                connection.Open();

                // Get sender's account details
                string query = string.Concat(@"SELECT amount FROM account WHERE acc_no=", transaction.Acc_No);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();
                Account senderAccount = new Account(transaction.Acc_No);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        senderAccount.Balance = reader.GetDouble(0);
                    }
                }

                // Get receiver's account details
                query = string.Concat(@"SELECT amount FROM account WHERE acc_no=", transaction.TransferTo);
                command = new MySqlCommand();
                command.CommandText = query;
                command.Connection = connection;
                command.ExecuteNonQuery();
                Account receiverAccount = new Account(transaction.TransferTo);
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        receiverAccount.Balance = reader.GetDouble(0);
                    }
                }

                // check sender's account balance > 0 and > transaction.Amount
                if (senderAccount.Balance > 0)
                {
                    if (senderAccount.Balance >= transaction.Amount)
                    {
                        // Deduct transfer amount from sender's account
                        query = "UPDATE account SET amount=@amount WHERE acc_no=@acc_no";
                        command = new MySqlCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@amount", senderAccount.Balance - transaction.Amount);
                        command.Parameters.AddWithValue("@acc_no", senderAccount.AccNumber);
                        command.Connection = connection;
                        command.ExecuteNonQuery();

                        // Add transfer amount to receiver's account
                        query = "UPDATE account SET amount=@amount WHERE acc_no=@acc_no";
                        command = new MySqlCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@amount", receiverAccount.Balance + transaction.Amount);
                        command.Parameters.AddWithValue("@acc_no", receiverAccount.AccNumber);
                        command.Connection = connection;
                        command.ExecuteNonQuery();

                        // Add transaction entry to transaction table
                        query = "INSERT INTO transaction(user_id,acc_no,timestamp,type,amount,transfered_to) VALUES(@user_id,@acc_no,@timestamp,@type,@amount,@transfered_to)";
                        command = new MySqlCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@user_id", transaction.User_Id);
                        command.Parameters.AddWithValue("@acc_no", transaction.Acc_No);
                        command.Parameters.AddWithValue("@timestamp", transaction.Timestamp);
                        command.Parameters.AddWithValue("@type", transaction.Type);
                        command.Parameters.AddWithValue("@amount", transaction.Amount);
                        command.Parameters.AddWithValue("@transfered_to", transaction.TransferTo);
                        command.Connection = connection;
                        command.ExecuteNonQuery();

                        return ReturnResult.Success;
                    }
                    else
                    {
                        return ReturnResult.AmountIsGreater;
                    }
                }
                else if (senderAccount.Balance == 0)
                {
                    return ReturnResult.IsEmpty;
                }
                else
                {
                    return ReturnResult.Error;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Concat("SQL ERROR: ", ex.Message));
                return ReturnResult.Error;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
