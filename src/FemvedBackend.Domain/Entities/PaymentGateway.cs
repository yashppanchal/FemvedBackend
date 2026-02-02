using FemvedBackend.Domain.Enums;

namespace FemvedBackend.Domain.Entities;

/// <summary>
/// Represents a payment gateway configuration.
/// </summary>
public class PaymentGateway
{
    public short Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public PaymentGatewayType Type { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
