using StateManager.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager
{
    public abstract class BaseState
    {
        public abstract Result DoProcess(ATMSimCommand command);

        public abstract Result Enter();

        public void Exit()
        {
            StateHolder.CurrentState = null;
        }

        public abstract BaseState GetNextState(ATMSimCommand command);
    }
}
