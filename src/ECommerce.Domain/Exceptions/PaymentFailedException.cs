namespace ECommerce.Domain.Exceptions;

public class PaymentFailedException : DomainException
{
    public PaymentFailedException(string message)
        : base(message)
    {
    }
}