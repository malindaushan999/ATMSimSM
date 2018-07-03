using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.States
{
    public class WithdrawState : SessionState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            base.DoProcess(command);
            if (command.CommandId == CommandId.Cancel)
            {
                Delegates.GetCancelWithdrawPage()(null);
                return Result.GoBack;
            }
            else if (command.CommandId == CommandId.Withdraw)
            {
                Delegates.GetWithdrawAction()(command.Param);
                return Result.NoResult;
            }
            else
            {
                return Result.NoResult;
            }
        }

        public override Result Enter()
        {
            Delegates.GetShowWithdrawPage()(null);
            return Result.Success;
        }

        public override BaseState GetNextState(ATMSimCommand command)
        {
            if (command.CommandId == CommandId.Withdraw)
            {
                return StateHolder.SessionState;
            }
            else
            {
                return null;
            }
        }
    }
}
