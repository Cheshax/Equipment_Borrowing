using EquipmentBorrowing.Application.Services;
using EquipmentBorrowing.Domain;
using EquipmentBorrowing.Infrastructure.Repositories;
using Xunit;

namespace EquipmentBorrowing.Tests;

public class BorrowEquipmentServiceTests
{
    [Fact]
    public async Task Successful_Borrow_When_Student_And_Equipment_Are_Valid()
    {
        var students = new List<Student> { new Student(1, "Juan Dela Cruz", isAllowedtoBorrow: true) };
        var equipment = new List<Equipment> { new Equipment(101, "Digital Multimeter", isAvailable: true) };

        var service = new BorrowEquipmentService(
            new InMemoryStudentRepository(students),
            new InMemoryEquipmentRepository(equipment),
            new InMemoryBorrowingRepository());

        var result = await service.ExecuteAsync(1, 101, DateTime.UtcNow.AddDays(7));

        Assert.True(result.Success);
        Assert.NotNull(result.Borrowing);
    }

    [Fact]
    public async Task Failed_Borrow_When_Equipment_Is_Unavailable()
    {
        var students = new List<Student> { new Student(1, "Juan Dela Cruz", isAllowedtoBorrow: true) };
        var equipment = new List<Equipment> { new Equipment(102, "Oscilloscope", isAvailable: false) };

        var service = new BorrowEquipmentService(
            new InMemoryStudentRepository(students),
            new InMemoryEquipmentRepository(equipment),
            new InMemoryBorrowingRepository());

        var result = await service.ExecuteAsync(1, 102, DateTime.UtcNow.AddDays(7));

        Assert.False(result.Success);
        Assert.Equal("Equipment is not available.", result.FailureReason);
    }

    [Fact]
    public async Task Failed_Borrow_When_Student_Is_Not_Allowed()
    {
        var students = new List<Student> { new Student(2, "Maria Santos", isAllowedtoBorrow: false) };
        var equipment = new List<Equipment> { new Equipment(101, "Digital Multimeter", isAvailable: true) };

        var service = new BorrowEquipmentService(
            new InMemoryStudentRepository(students),
            new InMemoryEquipmentRepository(equipment),
            new InMemoryBorrowingRepository());

        var result = await service.ExecuteAsync(2, 101, DateTime.UtcNow.AddDays(7));

        Assert.False(result.Success);
        Assert.Equal("Student is not allowed to borrow equipment.", result.FailureReason);
    }
}