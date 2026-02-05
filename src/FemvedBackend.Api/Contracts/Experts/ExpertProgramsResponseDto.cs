namespace FemvedBackend.Api.Contracts.Experts;

/// <summary>
/// Represents an expert with their programs and duration-based pricing.
/// </summary>
public sealed record ExpertProgramsResponseDto(
    Guid ExpertId,
    string ExpertName,
    string? Bio,
    IReadOnlyList<ExpertProgramDto> Programs);
