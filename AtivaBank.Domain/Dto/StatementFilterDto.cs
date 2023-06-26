using AtivaBank.Domain.Enum;

namespace AtivaBank.Domain.Dto
{
    public record StatementFilterDto(string id, string? initDate, string? endDate, MovementType type = MovementType.Credit);
}
