using FemvedBackend.Application.Interfaces.Notifications;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.Notifications;
using FemvedBackend.Domain.Entities;
using FemvedBackend.Domain.Enums;
namespace FemvedBackend.Application.UseCases.Access;

/// <summary>
/// Grants a user access to a product.
/// </summary>
public sealed class GrantUserAccessHandler
{
    private readonly IUserProductAccessRepository _accessRepository;
    private readonly INotificationSender _notificationSender;

    public GrantUserAccessHandler(
        IUserProductAccessRepository accessRepository,
        INotificationSender notificationSender)
    {
        _accessRepository = accessRepository;
        _notificationSender = notificationSender;
    }

    public async Task HandleAsync(GrantUserAccessRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _accessRepository.GetAsync(request.UserId, request.ProductId, cancellationToken);
        if (existing is not null)
        {
            existing.ExpiresAt = request.ExpiresAt;
            await _accessRepository.UpdateAsync(existing, cancellationToken);
            return;
        }

        var access = new UserProductAccess
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            ProductId = request.ProductId,
            GrantedAt = DateTimeOffset.UtcNow,
            ExpiresAt = request.ExpiresAt
        };

        await _accessRepository.AddAsync(access, cancellationToken);

        var message = new NotificationMessage(
            request.UserId,
            NotificationChannel.InApp,
            "Access granted",
            "Your access to the product has been granted.",
            "AccessGranted");

        await _notificationSender.SendAsync(message, cancellationToken);
    }
}
