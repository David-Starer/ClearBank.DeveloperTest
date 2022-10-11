using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services;

public interface IPaymentService
{
    void MakePayment(MakePaymentRequest request);
}