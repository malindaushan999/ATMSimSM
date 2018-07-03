using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateManager.States
{
    public static class StateHolder
    {
        // Previous state
        public static BaseState PreviousState;

        // Next state
        public static BaseState NextState;

        // Current state
        public static BaseState CurrentState;

        // Idle state
        public static IdleState IdleState = new IdleState();

        // Login state
        public static LoginState LoginState = new LoginState();

        // Session state
        public static SessionState SessionState = new SessionState();

        // Authentication state
        public static AuthenticationState AuthenticationState = new AuthenticationState();

        // Balance State
        public static BalanceState BalanceState = new BalanceState();

        // Withdraw State
        public static WithdrawState WithdrawState = new WithdrawState();

        // Transfer State
        public static TransferState TransferState = new TransferState();
    }
}
