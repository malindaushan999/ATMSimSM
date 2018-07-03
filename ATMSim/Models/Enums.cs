using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMSim.Models
{
    public enum ReturnResult
    {
        Success = 0,
        IsEmpty,
        AmountIsGreater,
        Error
    };
}
