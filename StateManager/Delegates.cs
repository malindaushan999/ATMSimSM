namespace StateManager
{
    // Base delegate that can be used to inilialize types
    public delegate void DelegateMethod(object param);

    public static class Delegates
    {
        #region ShowMainPage Delegate
        /// <summary>
        /// Show main page delegate
        /// </summary>
        private static DelegateMethod ShowMainPage;

        // Set Show main page delegate
        public static void SetShowMainPage(DelegateMethod delegateMethod)
        {
            ShowMainPage = delegateMethod;
        }

        // Get Show main page delegate
        public static DelegateMethod GetShowMainPage()
        {
            return ShowMainPage;
        }
        #endregion

        #region ShowLoginPage Delegate
        /// <summary>
        /// Show login page delegate
        /// </summary>
        private static DelegateMethod ShowLoginPage;

        // Set Show login page delegate
        public static void SetShowLoginPage(DelegateMethod delegateMethod)
        {
            ShowLoginPage = delegateMethod;
        }

        // Get Show login page delegate
        public static DelegateMethod GetShowLoginPage()
        {
            return ShowLoginPage;
        }
        #endregion

        #region CancelLoginPage Delegate
        /// <summary>
        /// Cancel login page delegate
        /// </summary>
        private static DelegateMethod CancelLoginPage;

        // Set Cancel login page delegate
        public static void SetCancelLoginPage(DelegateMethod delegateMethod)
        {
            CancelLoginPage = delegateMethod;
        }

        // Get Cancel login page delegate
        public static DelegateMethod GetCancelLoginPage()
        {
            return CancelLoginPage;
        }
        #endregion

        #region Authentication Delegate
        /// <summary>
        /// authentication delegate
        /// </summary>
        private static DelegateMethod Authentication;

        // Set authentication delegate
        public static void SetAuthentication(DelegateMethod delegateMethod)
        {
            Authentication = delegateMethod;
        }

        // Get authentication delegate
        public static DelegateMethod GetAuthentication()
        {
            return Authentication;
        }
        #endregion

        #region ShowSessionPage Delegate
        /// <summary>
        /// show session page delegate
        /// </summary>
        private static DelegateMethod ShowSessionPage;

        // Set session page delegate
        public static void SetShowSessionPage(DelegateMethod delegateMethod)
        {
            ShowSessionPage = delegateMethod;
        }

        // Get session page delegate
        public static DelegateMethod GetShowSessionPage()
        {
            return ShowSessionPage;
        }
        #endregion

        #region TimeOut Delegate
        /// <summary>
        /// TimeOut Operation delegate
        /// </summary>
        private static DelegateMethod TimeOutOp;

        // Set TimeOut Operation delegate
        public static void SetTimeOutOp(DelegateMethod delegateMethod)
        {
            TimeOutOp = delegateMethod;
        }

        // Get TimeOut Operation delegate
        public static DelegateMethod GetTimeOutOp()
        {
            return TimeOutOp;
        }
        #endregion

        #region SignOut Delegate
        /// <summary>
        /// SignOut Operation delegate
        /// </summary>
        private static DelegateMethod SignOutOp;

        // Set TimeOut Operation delegate
        public static void SetSignOutOp(DelegateMethod delegateMethod)
        {
            SignOutOp = delegateMethod;
        }

        // Get TimeOut Operation delegate
        public static DelegateMethod GetSignOutOp()
        {
            return SignOutOp;
        }
        #endregion

        #region ShowBalancePage Delegate
        /// <summary>
        /// ShowBalancePage delegate
        /// </summary>
        private static DelegateMethod ShowBalancePage;

        // Set ShowWithdrawPage delegate
        public static void SetShowBalancePage(DelegateMethod delegateMethod)
        {
            ShowBalancePage = delegateMethod;
        }

        // Get ShowWithdrawPage delegate
        public static DelegateMethod GetShowBalancePage()
        {
            return ShowBalancePage;
        }
        #endregion

        #region ShowTransferPage Delegate
        /// <summary>
        /// ShowTransferPage delegate
        /// </summary>
        private static DelegateMethod ShowTransferPage;

        // Set ShowTransferPage delegate
        public static void SetShowTransferPage(DelegateMethod delegateMethod)
        {
            ShowTransferPage = delegateMethod;
        }

        // Get ShowTransferPage delegate
        public static DelegateMethod GetShowTransferPage()
        {
            return ShowTransferPage;
        }
        #endregion

        #region ShowWithdrawPage Delegate
        /// <summary>
        /// ShowWithdrawPage delegate
        /// </summary>
        private static DelegateMethod ShowWithdrawPage;

        // Set ShowWithdrawPage delegate
        public static void SetShowWithdrawPage(DelegateMethod delegateMethod)
        {
            ShowWithdrawPage = delegateMethod;
        }

        // Get ShowWithdrawPage delegate
        public static DelegateMethod GetShowWithdrawPage()
        {
            return ShowWithdrawPage;
        }
        #endregion

        #region CancelBalancePage Delegate
        /// <summary>
        /// Cancel balance page delegate
        /// </summary>
        private static DelegateMethod CancelBalancePage;

        // Set Cancel balance page delegate
        public static void SetCancelBalancePage(DelegateMethod delegateMethod)
        {
            CancelBalancePage = delegateMethod;
        }

        // Get Cancel balance page delegate
        public static DelegateMethod GetCancelBalancePage()
        {
            return CancelBalancePage;
        }
        #endregion

        #region CancelWithdrawPage Delegate
        /// <summary>
        /// Cancel withdraw page delegate
        /// </summary>
        private static DelegateMethod CancelWithdrawPage;

        // Set Cancel page delegate
        public static void SetCancelWithdrawPage(DelegateMethod delegateMethod)
        {
            CancelWithdrawPage = delegateMethod;
        }

        // Get Cancel page delegate
        public static DelegateMethod GetCancelWithdrawPage()
        {
            return CancelWithdrawPage;
        }
        #endregion

        #region CancelTransferPage Delegate
        /// <summary>
        /// Cancel transfer page delegate
        /// </summary>
        private static DelegateMethod CancelTransferPage;

        // Set transfer page delegate
        public static void SetCancelTransferPage(DelegateMethod delegateMethod)
        {
            CancelTransferPage = delegateMethod;
        }

        // Get transfer page delegate
        public static DelegateMethod GetCancelTransferPage()
        {
            return CancelTransferPage;
        }
        #endregion

        #region WithdrawAction Delegate
        /// <summary>
        /// Withdraw action delegate
        /// </summary>
        private static DelegateMethod WithdrawAction;

        // Set withdraw action delegate
        public static void SetWithdrawAction(DelegateMethod delegateMethod)
        {
            WithdrawAction = delegateMethod;
        }

        // Get withdraw action delegate
        public static DelegateMethod GetWithdrawAction()
        {
            return WithdrawAction;
        }
        #endregion

        #region TransferAction Delegate
        /// <summary>
        /// TransferAction delegate
        /// </summary>
        private static DelegateMethod TransferAction;

        // Set TransferAction delegate
        public static void SetTransferAction(DelegateMethod delegateMethod)
        {
            TransferAction = delegateMethod;
        }

        // Get TransferAction delegate
        public static DelegateMethod GetTransferAction()
        {
            return TransferAction;
        }
        #endregion

        #region Print Receipt Delegate
        /// <summary>
        /// PrintReceipt action delegate
        /// </summary>
        private static DelegateMethod PrintReceipt;

        // Set PrintReceipt delegate
        public static void SetPrintReceipt(DelegateMethod delegateMethod)
        {
            PrintReceipt = delegateMethod;
        }

        // Get PrintReceipt delegate
        public static DelegateMethod GetPrintReceipt()
        {
            return PrintReceipt;
        }
        #endregion
    }
}
