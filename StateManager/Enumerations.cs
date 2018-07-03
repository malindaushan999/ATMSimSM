namespace StateManager
{
    public enum Result
    {
        Success = 0,
        NotSuccess,
        Error,
        NoResult,
        GoBack
    };

    public enum CommandId
    {
        ShowMainPage = 0,
        ShowLoginPage,
        ShowBalancePage,
        ShowWithdrawPage,
        ShowTransferPage,
        StartAuthentication,
        AuthenticationSuccess,
        AuthenticationFailure,
        PinVerificationFailure,
        Withdraw,
        Transfer,
        Print,
        Cancel,
        SignIn,
        SignOut,
        OK,
        Timeout
    };
}
