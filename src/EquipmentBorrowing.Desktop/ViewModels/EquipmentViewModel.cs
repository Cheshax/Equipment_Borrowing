using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Application.Services;
using EquipmentBorrowing.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EquipmentBorrowing.Desktop.ViewModels;

public partial class EquipmentViewModel : ObservableObject
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly BorrowEquipmentService _borrowEquipmentService;

    public ObservableCollection<Equipment> AvailableEquipment { get; } = new();
    public ObservableCollection<Student> Students { get; } = new();

    [ObservableProperty]
    private Equipment? selectedEquipment;

    [ObservableProperty]
    private Student? selectedStudent;

    [ObservableProperty]
    private DateTimeOffset expectedReturnDate = DateTimeOffset.Now.AddDays(7);

    [ObservableProperty]
    private string? statusMessage;

    public EquipmentViewModel(
        IEquipmentRepository equipmentRepository,
        IStudentRepository studentRepository,
        BorrowEquipmentService borrowEquipmentService)
    {
        _equipmentRepository = equipmentRepository;
        _studentRepository = studentRepository;
        _borrowEquipmentService = borrowEquipmentService;
    }

    public async Task LoadAsync()
    {
        AvailableEquipment.Clear();
        var equipment = await _equipmentRepository.GetAllAsync();
        foreach (var item in equipment)
            AvailableEquipment.Add(item);

        Students.Clear();
        var students = await _studentRepository.GetAllAsync();
        foreach (var s in students)
            Students.Add(s);
    }

    [RelayCommand]
    private async Task BorrowAsync()
    {
        // Presentation-level validation only — no business rules here.
        if (SelectedStudent is null)
        {
            StatusMessage = "Please select a student.";
            return;
        }

        if (SelectedEquipment is null)
        {
            StatusMessage = "Please select equipment.";
            return;
        }

        var result = await _borrowEquipmentService.ExecuteAsync(
            SelectedStudent.Id,
            SelectedEquipment.Id,
            ExpectedReturnDate.DateTime);

        StatusMessage = result.Success
            ? $"Borrowed successfully. Borrowing #{result.Borrowing!.Id}."
            : $"Failed: {result.FailureReason}";

        if (result.Success)
            await LoadAsync(); // refresh the list so the borrowed item disappears/updates
    }
}