<<<<<<<< HEAD:README.md
Solution Structure
Domain - holds the main classes like Student, Equipment, and Borrowing. These just store data and handle their own small rules.
Application - holds BorrowEquipmentService, this is where the actual borrowing rules are checked.
Infrastructure - holds the repository classes that store the data, right now it just uses Lists instead of a real database.
Tests - holds the test project, this is where we can test if the service works correctly.


ConsoleDemo (future UI)
        │
        v
   Application
     │      ^
     v      │
   Domain   │ 
            │
   Infrastructure

   explanation: 
   ConsoleDemo uses Application. Application uses Domain.
   Infrastructure also uses Domain and Application (so it can implement Application's interfaces), 
   but nothing depends on Infrastructure ( can be replaced with database implimentation in the future ).

  
Reflection
                        
1. Why depend on an interface instead of a database directly?
- So the code doesn't care where the data comes from. this is just to test it without changing the service.
2. What stays the same if SQLite is added later?
- Domain and Application stay the same. only the infrastructure changes into SQLite implimentation.
3. Which project would have Avalonia Views?
- A new UI project separate from Domain,Application,Infrastructure.
4. Should a button run database queries directly?
- No. The button should just call the Application service. The database queries should run in Infrastructure.
5. What represents the actual business operation?
BorrowEquipmentService,  this is the class that checks everything and creates the borrowing.
========
ConsoleDemo (future UI)
        │
        v
   Application
     │      ^
     v      │
   Domain   │ 
            │
   Infrastructure

   explanation: 
   ConsoleDemo uses Application. Application uses Domain.
   Infrastructure also uses Domain and Application (so it can implement Application's interfaces), 
   but nothing depends on Infrastructure ( can be replaced with database implimentation in the future ).

  
Reflection
                        
1. Why depend on an interface instead of a database directly?
- So the code doesn't care where the data comes from. this is just to test it without changing the service.
2. What stays the same if SQLite is added later?
- Domain and Application stay the same. only the infrastructure changes into SQLite implimentation.
3. Which project would have Avalonia Views?
- A new UI project separate from Domain,Application,Infrastructure.
4. Should a button run database queries directly?
- No. The button should just call the Application service. The database queries should run in Infrastructure.
5. What represents the actual business operation?
BorrowEquipmentService,  this is the class that checks everything and creates the borrowing.
>>>>>>>> 7d355c7d781df832b7aacf266203c07c23687da0:Readme.md

## Lab Activity 2 — Avalonia UI and MVVM

### 1. Desktop Project

EquipmentBorrowing.Desktop contains the graphical interface. It references Application and
Infrastructure so it can display data and trigger operations, but Domain and Application
have no reference back to Desktop or Avalonia — they remain completely UI-independent.

### 2. Updated Architecture

Avalonia View
      │  Binding / Command
      ▼
  ViewModel
      │  Application Operation
      ▼
Application Service
      │
      ├──────► Domain
      │
      ▼
Repository Interface
      ▲
      │
Infrastructure Implementation

### 3. Borrow Equipment Flow

The user selects a student, equipment, and return date in EquipmentView, then presses
"Borrow Equipment." This runs EquipmentViewModel.BorrowAsync, which checks that a student
and equipment were actually selected (presentation validation only), then calls
BorrowEquipmentService.ExecuteAsync. That service checks all the business rules and either
creates a Borrowing or returns a failure reason. The ViewModel displays the result in
StatusMessage and refreshes the equipment list.

### 4. Return Equipment Flow

The user selects an active borrowing in BorrowingsView and presses "Return Equipment."
BorrowingsViewModel.ReturnAsync calls ReturnEquipmentService.ExecuteAsync, which finds the
borrowing, marks it Returned, and marks the equipment Available again. The result is shown
in StatusMessage, and the list refreshes.

### 5. Architectural Reflection

1. Why should the View not call a repository directly?
   The View only knows how to display things and react to user input — it shouldn't know
   about data storage. Keeping that logic in the ViewModel/Application layer means the UI
   can change completely without touching how data is fetched or validated.

2. Why should business rules not be implemented in the ViewModel?
   If the rules were duplicated in the ViewModel, they'd need to be kept in sync with the
   Application layer's rules, and a second UI (web, mobile) would have to repeat them again.
   Keeping rules in one place (Application/Domain) means every entry point uses the same logic.

3. What is the responsibility of the ViewModel?
   To hold presentation state (selected items, status messages), expose commands the View
   can bind to, and call into Application services — never to decide the business outcome itself.

4. Why can the existing Application layer work without knowing that Avalonia is being used?
   Because Application only depends on Domain and its own repository interfaces — it never
   references Desktop or any UI framework, so it has no idea what's calling it.

5. What advantage is gained from registering dependencies in one composition point?
   All the wiring (which repository implementation, which services) lives in one place
   (App.axaml.cs), so it's easy to see and change everything the app depends on without
   hunting through ViewModels for scattered "new SomeService()" calls.

6. If the in-memory repository were replaced by SQLite later, which parts of the current
   interface should remain largely unchanged?
   The Views, ViewModels, and Application services would stay the same — only the
   registrations in App.axaml.cs and the Infrastructure repository classes would change.