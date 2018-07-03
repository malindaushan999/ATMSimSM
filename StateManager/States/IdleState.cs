using StateManager.States;

namespace StateManager
{
    public class IdleState : BaseState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            if (command.CommandId == CommandId.ShowLoginPage)
            {
                return Result.Success;
            }
            else
            {
                return Result.NotSuccess;
            }
        }

        public override Result Enter()
        {
            if(Delegates.GetShowMainPage() != null)
            {
                Delegates.GetShowMainPage()(null);
                return Result.Success;
            }
            return Result.NoResult;
        }
        
        public override BaseState GetNextState(ATMSimCommand command)
        {
            if (command.CommandId == CommandId.ShowLoginPage)
            {
                return StateHolder.LoginState;
            }
            else
            {
                return null;
            }
        }

    }
}
