using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EquipmentBorrowing.Application.Interfaces;
using EquipmentBorrowing.Application.Services;
using EquipmentBorrowing.Desktop.ViewModels;
using EquipmentBorrowing.Desktop.Views;
using EquipmentBorrowing.Domain;
using EquipmentBorrowing.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace EquipmentBorrowing.Desktop;

public partial class App : Avalonia.Application
{
    public static ServiceProvider? Services { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Services.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Seed data shared across the whole app session
        var students = new List<Student>
        {
            new Student(1, "Juan Dela Cruz", isAllowedtoBorrow: true),
            new Student(2, "Maria Santos", isAllowedtoBorrow: false)
        };

        var equipment = new List<Equipment>
        {
            new Equipment(101, "Digital Multimeter", isAvailable: true),
            new Equipment(101, "Digital Multimeter", isAvailable: true),
            new Equipment(102, "Oscilloscope", isAvailable: true)
        };

        // Registering as singletons so state (borrowings, availability) persists
        // across the whole app session instead of resetting between views.
        services.AddSingleton<IStudentRepository>(new InMemoryStudentRepository(students));
        services.AddSingleton<IEquipmentRepository>(new InMemoryEquipmentRepository(equipment));
        services.AddSingleton<IBorrowingRepository, InMemoryBorrowingRepository>();

        services.AddTransient<BorrowEquipmentService>();
        services.AddTransient<ReturnEquipmentService>();

        services.AddTransient<EquipmentViewModel>();
        services.AddTransient<BorrowingsViewModel>();
        services.AddSingleton<MainWindowViewModel>();
    }
}