using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using System;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests;

public class AccountServiceTests 
{
    private readonly AccountService _accountService;

    public AccountServiceTests()
    {
        _accountService = new AccountService();
    }

    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsBacs_ThenAccountIsUpdatedWithCorrectAmount()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.Bacs);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account{ AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs };

        // Act
        _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        accountDataStoreMock.Verify(x => x.UpdateAccount(account, request.Amount), Times.Once);
    }

    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsBacsAndAccountDoesNotAllowBacs_ThenThrowsError()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.Bacs);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps | AllowedPaymentSchemes.FasterPayments};

        // Act
        var act = () => _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsFasterPaymentsAndAccountBalanceIsSufficient_ThenAccountIsUpdatedWithCorrectAmount()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.FasterPayments);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Balance = 200.0m
        };

        // Act
        _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        accountDataStoreMock.Verify(x => x.UpdateAccount(account, request.Amount), Times.Once);
    }


    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsFasterPaymentsAndAccountBalanceIsNotSufficient_ThenThrowsError()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.FasterPayments);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account
        {
            AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
            Balance = 100.0m
        };

        // Act
        var act = () => _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsChaps_ThenAccountIsUpdatedWithCorrectAmount()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.Chaps);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps };

        // Act
        _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        accountDataStoreMock.Verify(x => x.UpdateAccount(account, request.Amount), Times.Once);
    }

    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsChapsAndAccountDoesNotAllowChaps_ThenThrowsError()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.Chaps);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Bacs };

        // Act
        var act = () => _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void WhenPaymentMade_AndPaymentSchemeIsChapsAndAccounIsNotLive_ThenThrowsError()
    {
        //Arrange
        var request = GetMakePaymentRequest(PaymentScheme.Chaps);

        var accountDataStoreMock = new Mock<IDataStore>();

        var account = new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps, Status = AccountStatus.Disabled};

        // Act
        var act = () => _accountService.PayFromAccount(request, account, accountDataStoreMock.Object);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    private static MakePaymentRequest GetMakePaymentRequest(PaymentScheme paymentScheme)
    {
        return new MakePaymentRequest
        {
            Amount = 123.4m,
            CreditorAccountNumber = It.IsAny<string>(),
            DebtorAccountNumber = It.IsAny<string>(),
            PaymentDate = It.IsAny<DateTime>(),
            PaymentScheme = paymentScheme
        };
    }
}

