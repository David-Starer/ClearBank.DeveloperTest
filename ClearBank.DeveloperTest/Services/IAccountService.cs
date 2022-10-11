using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public interface IAccountService
{
    void PayFromAccount(MakePaymentRequest request, Account account, IDataStore dataStore);
}