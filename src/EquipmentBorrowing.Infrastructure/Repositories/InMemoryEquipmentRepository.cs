using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Infrastructure.Repositories;

public class InMemoryEquipmentRepository : IEquipmentRepository
{
    private readonly List<Equipment> _equipment;

    public InMemoryEquipmentRepository(IEnumerable<Equipment> seedData)
    {
        _equipment = seedData.ToList();
    }

    public Task<Equipment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var item = _equipment.FirstOrDefault(e => e.Id == id);
        return Task.FromResult(item);
    }
    public Task<IReadOnlyList<Equipment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Equipment> all = _equipment.ToList();
        return Task.FromResult(all);
    }
}