using AtivaBank.Domain.Entities;
using AtivaBank.Domain.Repositories;

namespace AtivaBank.Infra
{
    public class MovementRepository : IMovementRepository
    {
        private List<Movements> _movements { get; set; } = new List<Movements>();

        public MovementRepository()
        {

        }

        public void RegisterMovement(Movements movement) => _movements.Add(movement);

        public IEnumerable<Movements> GetAllMovements(string id) 
            => _movements.Where(movement => movement.AccountId.Equals(id, StringComparison.InvariantCultureIgnoreCase));
    }
}