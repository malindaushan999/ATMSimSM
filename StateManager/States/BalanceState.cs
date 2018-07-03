using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.States
{
    public class BalanceState : SessionState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            base.DoProcess(command);
            if(command.CommandId == CommandId.Cancel)
            {
                Delegates.GetCancelBalancePage()(null);
                return Result.GoBack;
            }
            else if (command.CommandId == CommandId.Print)
            {
                Delegates.GetPrintReceipt()(null);
                return Result.NoResult;
            }
            else
            {
                return Result.NoResult;
            }
        }

        public override Result Enter()
        {
            Delegates.GetShowBalancePage()(null);
            return Result.Success;
        }

        public override BaseState GetNextState(ATMSimCommand command)
        {
            return null;
        }
    }
}
