using FemvedBackend.Application.Interfaces.Notifications;
using FemvedBackend.Application.Interfaces.Repositories;
using FemvedBackend.Application.Notifications;
using FemvedBackend.Domain.Enums;
namespace FemvedBackend.Application.UseCases.Access;

/// <summary>
/// Revokes a user's access to a product.
/// </summary>
public sealed class RevokeUserAccessHandler
{
    private readonly IUserProductAccessRepository _accessRepository;
    private readonly INotificationSender _notificationSender;

    public RevokeUserAccessHandler(
        IUserProductAccessRepository accessRepository,
        INotificationSender notificationSender)
    {
        _accessRepository = accessRepository;
        _notificationSender = notificationSender;
    }

    public async Task HandleAsync(RevokeUserAccessRequest request, CancellationToken cancellationToken = default)
    {
        var access = await _accessRepository.GetAsync(request.UserId, request.ProductId, cancellationToken);
        if (access is null)
        {
            return;
        }

        await _accessRepository.DeleteAsync(access, cancellationToken);

        var message = new NotificationMessage(
            request.UserId,
            NotificationChannel.InApp,
            "Access revoked",
            "Your access to the product has been revoked.",
            "AccessRevoked");

        await _notificationSender.SendAsync(message, cancellationToken);
    }
}
