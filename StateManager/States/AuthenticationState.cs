using System;

namespace StateManager.States
{
    public class AuthenticationState : BaseState
    {
        public override Result DoProcess(ATMSimCommand command)
        {
            //if (command.CommandId == CommandId.Authenticating)
            //{
            //    return Result.NoReult;
            //}
            if (command.CommandId == CommandId.AuthenticationSuccess)
            {
                return Result.Success;
            }
            else if (command.CommandId == CommandId.AuthenticationFailure)
            {
                ChangeToLoginState();
                return Result.NotSuccess;
            }
            else if (command.CommandId == CommandId.PinVerificationFailure)
            {
                ChangeToIdleState();
                return Result.NoResult;
            }
            else
            {
                return Result.NoResult;
            }

        }

        public override Result Enter()
        {
            Delegates.GetAuthentication()(null);
            return Result.Success;
        }

        public override BaseState GetNextState(ATMSimCommand command)
        {
            if (command.CommandId == CommandId.AuthenticationSuccess)
            {
                return StateHolder.SessionState;
            }
            else
            {
                return null;
            }
        }

        private void ChangeToLoginState()
        {
            var temp = StateHolder.CurrentState;
            StateHolder.CurrentState.Exit();
            StateHolder.CurrentState = StateHolder.LoginState;
            StateHolder.PreviousState = temp;
        }

        private void ChangeToIdleState()
        {
            var temp = StateHolder.CurrentState;
            StateHolder.CurrentState.Exit();
            StateHolder.CurrentState = StateHolder.IdleState;
            StateHolder.PreviousState = temp;
        }

    }
}
