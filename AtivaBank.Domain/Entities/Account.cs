namespace AtivaBank.Domain.Entities
{
    public struct Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public decimal Limit { get; set; }

        public override string ToString() => $"{Id} - {Name} - {Balance} - {Limit}";
    }
}
