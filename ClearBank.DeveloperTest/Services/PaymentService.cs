using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Exceptions;
using ClearBank.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using System;

namespace ClearBank.DeveloperTest.Services;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration configuration;
    private readonly IAccountService accountService;

    public PaymentService(IConfiguration configuration, IAccountService accountService)
    {
        // TODO: Create an appsettings.json and add code in Startup using the DI builder to add Configuration to the container.
        this.configuration = configuration;
        this.accountService = accountService;
    }

    public void MakePayment(MakePaymentRequest request)
    {
        var dataStoreType = configuration["DataStoreType"];

        IDataStore dataStore = dataStoreType == "Backup"
            ? new BackupAccountDataStore()
            : new AccountDataStore();

        var account = dataStore.GetAccount(request.DebtorAccountNumber);

        if (account == null)
        {
            throw new AccountNotFoundException($"Could not find account: {request.DebtorAccountNumber}");
        }

        try
        {
            accountService.PayFromAccount(request, account, dataStore);
        }
        catch (Exception ex)
        {
            // TODO: handle error appropriately
        }
    }

   
}