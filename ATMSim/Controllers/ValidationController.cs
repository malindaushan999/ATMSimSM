using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSim.Controllers
{
    public static class ValidationController
    {
        public static bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }
    }
}
