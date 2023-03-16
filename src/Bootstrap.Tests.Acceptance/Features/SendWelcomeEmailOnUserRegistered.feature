Feature: Send welcome email on user registered

As a user
I want to receive an email when I registered
In order to be welcome and have product information

Background:
    Given the current date is 2022-12-07

@RealHtmlTemplateRenderer
@Integration
Scenario: Send welcome email on user registered
    When I register and log in with
      | Field         | Value                  |
      | Email address | john.doe@mycompany.com |
      | Password      | P@ssw0rd!              |
    Then the following single email has been sent
      | Field              | Value                        |
      | From email address | support@test.com             |
      | From name          | Best team                    |
      | To email address   | john.doe@mycompany.com       |
      | Subject            | Welcome to the app |
      | Html content       | welcome-email.html           |

Scenario: Failing to send a welcome email on user registered, does not rollback registration
    Given the email sender is down
    When I register and log in with
      | Field         | Value                  |
      | Email address | john.doe@mycompany.com |
      | Phone number  | +1-202-555-0159        |
      | Password      | P@ssw0rd!              |
    Then no email have been sent
    And no error occurred