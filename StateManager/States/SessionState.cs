namespace StateManager.States
{
    public class SessionState : BaseState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            if (command.CommandId == CommandId.Timeout)
            {
                Delegates.GetTimeOutOp()(null);
                ChangeToIdleState();
                return Result.NoResult;
            }
            else if (command.CommandId == CommandId.SignOut)
            {
                Delegates.GetSignOutOp()(null);
                ChangeToIdleState();
                return Result.NoResult;
            }
            else if(command.CommandId == CommandId.ShowBalancePage || command.CommandId == CommandId.ShowWithdrawPage || command.CommandId == CommandId.ShowTransferPage)
            {
                return Result.Success;
            }
            else
            {
                return Result.NoResult;
            }
        }

        public override Result Enter()
        {
            Delegates.GetShowSessionPage()(null);
            return Result.Success;
        }

        public override BaseState GetNextState(ATMSimCommand command)
        {
            if (command.CommandId == CommandId.ShowBalancePage)
            {
                return StateHolder.BalanceState;
            }
            else if (command.CommandId == CommandId.ShowWithdrawPage)
            {
                return StateHolder.WithdrawState;
            }
            else if (command.CommandId == CommandId.ShowTransferPage)
            {
                return StateHolder.TransferState;
            }
            else
                return null;
        }

        private void ChangeToIdleState()
        {
            StateHolder.PreviousState = StateHolder.CurrentState;                  // assign current state as previous state
            StateHolder.CurrentState.Exit();                                       // exit from current state
            StateHolder.CurrentState = StateHolder.IdleState;                      // assign next state as current state
            StateHolder.NextState = null;                                          // set next state as null
        }
    }
}
