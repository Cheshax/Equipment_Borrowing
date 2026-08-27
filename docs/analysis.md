Actors
	Student
	- Students needs to be able to:
		- Check which equipment is currently available
		- request to borrow a piece of equipment
		- return borrowed equipment
		- be told why a request to borrow equipment was denied

Use Cases
	Use case 1
	- Borrow Equipment
		Primary Actor: Student
		Preconditions: Student is logged in and has an active account
		Main action: Student selects a piece of equipment to borrow and submits a request
		Expected result: Student receives confirmation of the request and is informed of the expected return date
		Possible Failure: Student is not allowed to borrow, equipment does not exist, equipment is unavailable, 
		or student has reached the maximum number of active borrowings.

	Use case 2
	- Return Equipment
		Primary Actor: Student
		Preconditions: Student has an active Borrowing record for the equipment
		Main action: Student returns the borrowed equipment
		Expected result: The Borrowing record is marked as Returned, and the equipment becomes available again
		Possible Failure: No active borrowing exists for that student

	Use case 3
	- Find Available Equipment
		Primary Actor: Student
		Preconditions: Student is logged in and has an active account
		Main action: Student searches for equipment that is currently available for borrowing
		Expected result: Student sees a list of available equipment
		Possible Failure: No equipment is available for borrowing

Domain Concepts
 	- Equipment
		- Attributes: ID, Name, Availability Status
		- State: MarkAsBorrowed / MarkAsAvailable
		- Should NOT: know which student is borrowing it, check the student's eligibility, or enforce the borrowing limit
	- Student
		- Attributes: ID, Name,isAllowedtoBorrow
		- State: allowed/not allowed
		- Should NOT: know how many equipment items exist, check equipment availability, or decide whether this specific borrowing is allowed
	- Borrowing Record
		- Attributes: ID, Student ID, Equipment ID, Borrow Date, ExpectedReturnDate, Status
		- State: Active / Returned
		- Should NOT: know the details of the equipment or student, enforce borrowing rules, or manage equipment availability