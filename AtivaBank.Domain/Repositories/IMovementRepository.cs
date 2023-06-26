using AtivaBank.Domain.Entities;

namespace AtivaBank.Domain.Repositories
{
    public interface IMovementRepository
    {
        void RegisterMovement(Movements movement);
        IEnumerable<Movements> GetAllMovements(string id);
    }
}
