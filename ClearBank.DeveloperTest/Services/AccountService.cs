using System;
using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public class AccountService : IAccountService
{
    // In this method, I am testing Balance >= request.Amount and account.Status == AccountStatus.Live exactly as in the original code supplied for the test.
    // However, should we be checking Balance >= request.Amount and account.Status == AccountStatus.Live for ALL payment schemes?
    // If so, we should do this check in the caller (PaymentService.MakePayment()) and only come here if the account is in a valid state.

    public void PayFromAccount(MakePaymentRequest request, Account account, IDataStore dataStore)
    {
        switch (request.PaymentScheme)
        {
            case PaymentScheme.Bacs when
                account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs):

            case PaymentScheme.FasterPayments when
                account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments) &&
                account.Balance >= request.Amount:

            case PaymentScheme.Chaps when
                account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
                account.Status == AccountStatus.Live:

                dataStore.UpdateAccount(account, request.Amount);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(request), "Unknown request payment scheme or account state not valid for payment");
        }
    }
}