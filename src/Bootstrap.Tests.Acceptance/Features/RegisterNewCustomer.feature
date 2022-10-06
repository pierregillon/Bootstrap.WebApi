Feature: Register new customer

As an administrator
I want to register a new customer
In order to handle it

Scenario: Initializing new customer lists it
	When I register the following new customer
		| First name | Last name |
		| John       | Doe       |
	Then the customer list is
		| Full name |
		| John Doe  |
