using StateManager;

namespace StateManager.States
{
    public class LoginState : BaseState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            if(command.CommandId == CommandId.SignIn)
            {
                ATMSimCommand authenticationCommand = new ATMSimCommand(CommandId.StartAuthentication, this);
                ATMSimStateManager.AddToQueue(authenticationCommand);
                return Result.Success;
            }
            else if(command.CommandId == CommandId.Cancel)
            {
                StateHolder.PreviousState = StateHolder.IdleState;
                return Result.GoBack;
            }
            else
            {
                return Result.Error;
            }
        }

        public override Result Enter()
        {
            Delegates.GetShowLoginPage()(null);
            return Result.Success;
        }
                
        public override BaseState GetNextState(ATMSimCommand command)
        {
            command = StateManager.ATMSimStateManager.GeneralQueue.Dequeue();
            if (command.CommandId == CommandId.StartAuthentication)
            {
                return StateHolder.AuthenticationState;
            }
            else
            {
                return null;
            }
        }
    }
}
