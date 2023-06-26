namespace AtivaBank.Domain.Dto
{
    public record TransferDto(string senderId, string receiverId, decimal value);
}
