using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.States
{
    public class TransferState : SessionState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            base.DoProcess(command);
            if (command.CommandId == CommandId.Cancel)
            {
                Delegates.GetCancelTransferPage()(null);
                return Result.GoBack;
            }
            else if (command.CommandId == CommandId.Transfer)
            {
                Delegates.GetTransferAction()(null);
                return Result.NoResult;
            }
            else
            {
                return Result.NoResult;
            }
        }

        public override Result Enter()
        {
            Delegates.GetShowTransferPage()(null);
            return Result.Success;
        }
    }
}
