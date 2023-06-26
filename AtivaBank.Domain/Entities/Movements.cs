using AtivaBank.Domain.Enum;

namespace AtivaBank.Domain.Entities
{
    public struct Movements
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public MovementType Type { get; set; }
        public DateTime Date { get; set; }
    }
}
