using System;

namespace ClearBank.DeveloperTest.Exceptions;

public class AccountNotFoundException : Exception
{
    public AccountNotFoundException(string message)
        : base(message)
    {
    }
}