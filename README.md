# Expense Tracker

A web application for tracking personal expenses. Built with ASP.NET Core MVC, C#, Entity Framework Core, ASP.NET Core Identity, and MySQL.

## Features

- User registration, login, and logout
- Create, view, edit, and delete personal expenses
- Expenses are private to the signed-in user
- Search and filter expenses by description, category, and date range
- Dashboard with total, monthly, and today's spending
- Category summaries, monthly summaries, and recent expenses
- Validation that prevents future expense dates

## Tech stack

- .NET 6 / ASP.NET Core MVC
- C#
- Entity Framework Core
- ASP.NET Core Identity
- MySQL with Pomelo Entity Framework Core provider

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- MySQL Server

## Getting started

1. Clone the repository and enter the project folder:

   ```bash
   git clone https://github.com/Ochieng285/ExpenseTracker.git
   cd ExpenseTracker
   ```

2. Configure the database connection. Do not commit your real password. Initialize local user secrets and set the connection string:

   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "server=localhost;database=ExpenseTracker;user=root;password=YOUR_PASSWORD;"
   ```

3. Apply the included Entity Framework migrations:

   ```bash
   dotnet ef database update
   ```

4. Run the application:

   ```bash
   dotnet run
   ```

5. Open the HTTPS URL printed in the terminal, register an account, and start adding expenses.

## Configuration

`appsettings.json` intentionally contains a placeholder connection string. Keep production and local database credentials out of source control by using User Secrets for local development or environment variables in deployment.

## License

This project is available for learning and personal use.
