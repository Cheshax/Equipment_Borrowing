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

Lab Activity 2 — Avalonia UI and MVVM

1. Desktop Project
Desktop - this is where the User Interface lives (the Views and ViewModels). It references Application and Infrastructure to actually work, but Domain and Application have no reference back to Desktop or Avalonia.

2. Updated Architecture

Avalonia View
      │
      v
  ViewModel
      │
      v
Application Service
      │
      ├──► Domain
      │
      v
Repository Interface
      ^
      │
Infrastructure Implementation

explanation:
The View is just what you see on screen. The ViewModel grabs what the user picked and calls the Application service. The service checks if it's allowed and talks to the repository to get/save stuff. Infrastructure is what actually does the storing.

3. Borrow Equipment Flow
User picks a student, equipment, and a return date, then hits Borrow. The ViewModel checks nothing's empty first, then asks BorrowEquipmentService to actually do it. The service checks all the rules and either creates the borrowing or says why it can't. Whatever happens shows up on screen.

4. Return Equipment Flow
Same idea but for returning. User picks a borrowing and hits Return. ReturnEquipmentService marks it as returned and makes the equipment available again. Result shows up on screen too.

5. Architectural Reflection

    1. Why shouldn't the View talk to a repository directly?
        - The View's only job is to show stuff, not fetch data. Keeping it separate means we can change the UI without messing up how data works.
   
    2. Why shouldn't business rules be in the ViewModel?
        - If we put the rules there, we'd have to copy them again for any new UI later. Keeping them in one place means every UI follows the same rules.

    3. What's the ViewModel actually for?
        - It holds what the user picked, shows messages, and calls the Application service. It doesn't decide anything on its own.

    4. Why does Application not need to know Avalonia exists?
        - Because it only depends on Domain and its own interfaces, nothing about Desktop or Avalonia.

    5. Why register everything in one place?
        - So it's easy to see and change what the app needs, instead of digging through every ViewModel to find it.
    
    6. If we swapped in SQLite later, what stays the same?
        - Views, ViewModels, and the Application services stay the same. Only Infrastructure and the setup in App.axaml.cs would change.