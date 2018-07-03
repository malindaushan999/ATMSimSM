using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager
{
    public class ATMSimCommand
    {
        public CommandId CommandId { get; set; }
        public object Param { get; set; }

        public ATMSimCommand(CommandId commandId, object param)
        {
            CommandId = commandId;
            Param = param;
        }
    }
}
