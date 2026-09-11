using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Domain;

namespace EquipmentBorrowing.Application.Services;

public class BorrowEquipmentResult
{
    public bool Success { get; }
    public string? FailureReason { get; }
    public Borrowing? Borrowing { get; }

    private BorrowEquipmentResult(bool success, string? failureReason, Borrowing? borrowing)
    {
        Success = success;
        FailureReason = failureReason;
        Borrowing = borrowing;
    }

    public static BorrowEquipmentResult Fail(string reason) => new(false, reason, null);
    public static BorrowEquipmentResult Ok(Borrowing borrowing) => new(true, null, borrowing);
}

public class BorrowEquipmentService
{
    private const int MaxActiveBorrowings = 3;

    private readonly IStudentRepository _studentRepository;
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IBorrowingRepository _borrowingRepository;

    public BorrowEquipmentService(
        IStudentRepository studentRepository,
        IEquipmentRepository equipmentRepository,
        IBorrowingRepository borrowingRepository)
    {
        _studentRepository = studentRepository;
        _equipmentRepository = equipmentRepository;
        _borrowingRepository = borrowingRepository;
    }

    public async Task<BorrowEquipmentResult> ExecuteAsync(
        int studentId,
        int equipmentId,
        DateTime expectedReturnDate,
        CancellationToken cancellationToken = default)
    {
        var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
        if (student is null)
            return BorrowEquipmentResult.Fail("Student does not exist.");

        if (!student.IsAllowedtoBorrow)
            return BorrowEquipmentResult.Fail("Student is not allowed to borrow equipment.");

        var equipment = await _equipmentRepository.GetByIdAsync(equipmentId, cancellationToken);
        if (equipment is null)
            return BorrowEquipmentResult.Fail("Equipment does not exist.");

        if (!equipment.IsAvailable)
            return BorrowEquipmentResult.Fail("Equipment is not available.");

        var activeCount = await _borrowingRepository.CountActiveByStudentIdAsync(studentId, cancellationToken);
        if (activeCount >= MaxActiveBorrowings)
            return BorrowEquipmentResult.Fail("Student has reached the maximum number of active borrowings.");

        var borrowing = new Borrowing(
            id: new Random().Next(1000, 999999), // simple id generation for this activity
            studentId: studentId,
            equipmentId: equipmentId,
            dateBorrowed: DateTime.UtcNow,
            expectedReturnDate: expectedReturnDate);

        equipment.MarkAsBorrowed();
        await _borrowingRepository.AddAsync(borrowing, cancellationToken);

        return BorrowEquipmentResult.Ok(borrowing);
    }
}