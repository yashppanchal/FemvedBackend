namespace FemvedBackend.Api.Contracts.Experts;

/// <summary>
/// Represents duration-based pricing for a program.
/// </summary>
public sealed record ExpertProgramDurationDto(
    short Weeks,
    decimal Price,
    decimal? Discount,
    decimal? FinalPrice);
