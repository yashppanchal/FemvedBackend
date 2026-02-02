using FemvedBackend.Application.Interfaces.Identity;
using FemvedBackend.Application.Exceptions;
using FemvedBackend.Application.Interfaces.Repositories;

namespace FemvedBackend.Application.UseCases.Profile;

/// <summary>
/// Handles requests to retrieve the current user's profile.
/// </summary>
public sealed class GetProfileHandler
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public GetProfileHandler(ICurrentUserService currentUserService, IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<GetProfileResult> HandleAsync(GetProfileRequest request, CancellationToken cancellationToken = default)
    {
        if (!_currentUserService.IsAuthenticated || _currentUserService.UserId is null)
        {
            throw new ValidationException("user", "User must be authenticated.");
        }

        var user = await _userRepository.GetByIdWithRoleAsync(_currentUserService.UserId.Value, cancellationToken)
            ?? throw new ValidationException("user", "User not found.");

        var roleName = user.Role?.Name ?? string.Empty;

        return new GetProfileResult(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            user.Country,
            user.Currency,
            roleName);
    }
}
