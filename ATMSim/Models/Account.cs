using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSim.Models
{
    public class Account
    {
        public string AccNumber { get; set; }
        public double Balance { get; set; }
        public string OwnerID { get; set; }

        public Account()
        {

        }

        public Account(string _accountNo)
        {
            AccNumber = _accountNo;
        }
    }
}
