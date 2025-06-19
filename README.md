# Source project (before refactoring)
   - https://github.com/baminmru/DFM/tree/main/mapper 


# Mapper (Refactored)

This is a refactored version of the Mapper application with improved architecture and modern practices.

## Improvements

1. **Modern Architecture**
   - Dependency Injection using Microsoft.Extensions.DependencyInjection
   - Configuration management using Microsoft.Extensions.Configuration
   - Proper separation of concerns with services and utilities
   - Async/await pattern for database operations

2. **Better Error Handling**
   - Structured logging using Serilog
   - Proper exception handling and user feedback
   - Async operations with proper cancellation support

3. **Database Management**
   - Improved connection management with proper disposal
   - Async database operations
   - Better error handling and logging for database operations

4. **Code Organization**
   - Clear folder structure
   - Separation of concerns
   - Interface-based design for better testability
   - Modern C# features and patterns

## Project Structure

```
mapper_refactor/
├── src/                    # Source code
│   ├── Program.cs         # Application entry point
│   ├── MainForm.cs        # Main form
│   └── MainForm.Designer.cs
├── Services/              # Business services
│   └── DatabaseService.cs  # Database operations
├── Models/                # Data models
├── Utils/                 # Utility classes
│   └── TreeNodeUtils.cs   # Tree node operations
├── Data/                  # Data access
└── appsettings.json      # Application configuration
```

## Dependencies

- .NET 6.0
- Microsoft.Extensions.Configuration
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.Logging
- Npgsql
- Serilog

## Configuration

The application uses `appsettings.json` for configuration. Update the connection string and other settings as needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=5432;Database=mapper;User Id=postgres;Password=;"
  }
}
```

## Getting Started

1. Update the connection string in `appsettings.json`
2. Build and run the application
3. The application will automatically connect to the database on startup

## Development

- Use dependency injection for new services
- Follow async/await patterns for I/O operations
- Add proper error handling and logging
- Keep the separation of concerns
- Write unit tests for new functionality 
