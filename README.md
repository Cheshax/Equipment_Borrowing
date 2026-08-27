Solution Structure
Domain - holds the main classes, Student, Equipment etc. also their datas.
Application - holds BorrowEquipmentService, where the main borrowing process is happening.
Infrastructure - holds the connectivity to databases,apis, etc.
Tests - to do tests, to check if the application is working correctly.


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

  
Use Case Mapping

Actor: Student
Use Case: Borrow Equipment
Application Service: BorrowEquipmentService
Domain Objects Used: Student, Equipment, Borrowing, BorrowingStatus
Repository Interfaces Used: IStudentRepository, IEquipmentRepository, and IBorrowingRepository
Infrastructure Implementations Used: InMemoryStudentRepository, InMemoryEquipmentRepository, and InMemoryBorrowingRepository


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