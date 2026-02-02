namespace FemvedBackend.Domain.Enums;

/// <summary>
/// Identifies the type of payment gateway.
/// </summary>
public enum PaymentGatewayType
{
    Unknown = 0,
    Stripe = 1,
    PayPal = 2,
    Razorpay = 3
}
