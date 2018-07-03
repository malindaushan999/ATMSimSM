using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSim.Models
{
    public class Transaction
    {
        public string User_Id { get; set; }
        public string Acc_No { get; set; }
        public DateTime Timestamp { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public string TransferTo { get; set; }

        public Transaction()
        {

        }

        /// <summary>
        /// Withdraw constructor
        /// </summary>
        /// <param name="user_id">User ID</param>
        /// <param name="acc_no">Account number</param>
        /// <param name="timestamp">Time stamp</param>
        /// <param name="amount">Amount to withdraw</param>
        public Transaction(string user_id, string acc_no, DateTime timestamp, double amount)
        {
            User_Id = user_id;
            Acc_No = acc_no;
            Timestamp = timestamp;
            Type = "Withdraw";
            Amount = amount;
        }

        /// <summary>
        /// Transfer constructor
        /// </summary>
        /// <param name="user_id">User ID</param>
        /// <param name="acc_no">Account number</param>
        /// <param name="timestamp">Time stamp</param>
        /// <param name="amount">Amount to transfer</param>
        /// <param name="transferTo">Account number to transfer</param>
        public Transaction(string user_id, string acc_no, DateTime timestamp, double amount, string transferTo)
        {
            User_Id = user_id;
            Acc_No = acc_no;
            Timestamp = timestamp;
            Type = "Transfer";
            Amount = amount;
            TransferTo = transferTo;
        }
    }
}
