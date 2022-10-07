Feature: Rename customer

As an administrator
I want to rename a customer
In order to have the right information

Background:
	Given the following new customer registered
		| First name | Last name |
		| John       | Doe       |

Scenario: Renaming a customer can list the new name
	When I rename the customer "John Doe" to "Albert Rose"
	Then the customer list is
		| Full name   |
		| Albert Rose |

@ErrorHandling
Scenario: Cannot rename an unknown customer
	When I rename an unknown customer to "Albert Rose"
	Then an internal server error error occurred with message "Cannot found the user with id (.*)"
