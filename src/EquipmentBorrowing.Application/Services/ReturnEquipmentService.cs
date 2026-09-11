using EquipmentBorrowing.Application.Interfaces;

namespace EquipmentBorrowing.Application.Services;

public record ReturnEquipmentResult(bool Success, string? FailureReason)
{
    public static ReturnEquipmentResult Fail(string reason) => new(false, reason);
    public static ReturnEquipmentResult Ok() => new(true, null);
}

public class ReturnEquipmentService
{
    private readonly IBorrowingRepository _borrowingRepository;
    private readonly IEquipmentRepository _equipmentRepository;

    public ReturnEquipmentService(
        IBorrowingRepository borrowingRepository,
        IEquipmentRepository equipmentRepository)
    {
        _borrowingRepository = borrowingRepository;
        _equipmentRepository = equipmentRepository;
    }

    public async Task<ReturnEquipmentResult> ExecuteAsync(
        int borrowingId,
        CancellationToken cancellationToken = default)
    {
        var borrowing = await _borrowingRepository.GetByIdAsync(borrowingId, cancellationToken);
        if (borrowing is null)
            return ReturnEquipmentResult.Fail("Borrowing record not found.");

        if (borrowing.Status == Domain.BorrowingStatus.Returned)
            return ReturnEquipmentResult.Fail("This borrowing has already been returned.");

        var equipment = await _equipmentRepository.GetByIdAsync(borrowing.EquipmentId, cancellationToken);
        if (equipment is null)
            return ReturnEquipmentResult.Fail("Equipment record not found.");

        borrowing.MarkReturned();
        equipment.MarkAsAvailable();

        await _borrowingRepository.UpdateAsync(borrowing, cancellationToken);

        return ReturnEquipmentResult.Ok();
    }
}