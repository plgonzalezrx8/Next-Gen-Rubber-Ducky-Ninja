# AGENTS – Repository Guide for AI Coding Assistants

This document provides a comprehensive guide for AI agents working with "The Rubber Ducky Ninja - Next Generation" codebase. It includes project structure, coding standards, testing procedures, and development workflows for this modern WinUI 3 DuckyScript testing and validation toolkit.

## 📁 Project Structure

### Root Directory

- **`/Next Gen Rubber Ducky Ninja/`** – Main WinUI 3 application source code
- **`/lib/`** – External dependencies and libraries
- **`/Assets/`** – Application assets, icons, and resources
- **`/Properties/`** – Project properties and publishing profiles
- **`/bin/`** – Compiled output directory
- **`/obj/`** – Build intermediate files
- **Configuration files** – Project file, manifest, gitignore, documentation

### Application Structure (`/Next Gen Rubber Ducky Ninja/`)

#### Core Application Files

- **`MainWindow.xaml`** – Primary UI definition using WinUI 3 controls with Fluent Design
- **`MainWindow.xaml.cs`** (21KB) – Primary UI controller handling user interactions, async operations, and state management
- **`App.xaml`** – Application-level resources and theming configuration
- **`App.xaml.cs`** – Application entry point and lifecycle management
- **`app.manifest`** – Application manifest with UAC elevation capabilities

#### Business Logic Components

- **`DuckyScriptProcessing.cs`** (21KB) – Enhanced script parsing engine supporting:
  - Async file processing (`ReadFileAsync`)
  - Synchronous compatibility mode (`ReadFile`)
  - Command parsing and execution (`Calculate`, `KeyboardAction`)
  - Variable and constant management
  - Input simulation via InputSimulator
- **`Validation.cs`** (13KB) – Enhanced validation engine featuring:
  - Custom `ValidationException` with line number tracking
  - Comprehensive command syntax validation
  - Variable and constant type checking
  - Line-by-line error reporting

#### External Dependencies

- **`/lib/InputSimulator.dll`** – Low-level keyboard and mouse input simulation library
- **`duckencode.jar`** – Official DuckyScript to binary encoder (Java-based)
- **`restore.vbs`** – Windows system restore automation script
- **`helloworld.txt`** – Sample DuckyScript file for testing

## 🛠️ Technology Stack and Versions

### Core Technologies

- **C#** with **nullable reference types enabled** – Primary programming language
- **.NET 8.0** targeting **Windows 10.0.19041.0** – Runtime framework
- **WinUI 3** via **Microsoft.WindowsAppSDK 1.7.250513003** – Modern Windows UI framework
- **Windows SDK BuildTools 10.0.26100.4188** – Build toolchain

### Architecture Decisions

- **Unpackaged Deployment** (`WindowsPackageType=None`) – Standard .exe output without MSIX
- **UseRidGraph=true** – Ensures .NET 8 compatibility with Windows App SDK
- **UAC Integration** – Manifest configured for elevation capabilities
- **Async-First Design** – Non-blocking operations with proper UI thread marshaling

### Key Dependencies

- **InputSimulator.dll** – Referenced with HintPath, copied to output
- **Java Runtime** – Required for duckencode.jar binary encoding functionality
- **Windows App Runtime 1.6+** – Required runtime dependency

## 📝 Coding Standards and Best Practices

### C# and .NET Standards

- **Nullable Reference Types**: Enabled (`<Nullable>enable</Nullable>`)
- **Async Patterns**: Use `async/await` for all I/O operations
- **Exception Handling**: Implement custom exception types with context
- **Resource Management**: Proper disposal patterns and cleanup
- **Thread Safety**: UI thread marshaling for all UI operations

### WinUI 3 and XAML Conventions

- **Fluent Design**: Use system-integrated Mica backdrop and card-based layouts
- **InfoBar Notifications**: Use InfoBarSeverity for user feedback (Success, Warning, Error, Informational)
- **NumberBox Controls**: For numeric input with validation
- **Modern File Picker**: Use WinUI 3 FileOpenPicker with proper initialization
- **Theme Integration**: Support system theme changes and accessibility

### File Naming and Organization

- **XAML Files**: PascalCase with descriptive names (`MainWindow.xaml`)
- **Code-Behind**: Match XAML naming (`MainWindow.xaml.cs`)
- **Business Logic**: Descriptive class names (`DuckyScriptProcessing.cs`, `Validation.cs`)
- **Constants**: Use clear, descriptive naming in uppercase

### DuckyScript Processing Standards

- **Command Support**: Maintain comprehensive command set (STRING, DELAY, GUI, etc.)
- **Variable System**: Support `$VARIABLE` syntax with type validation
- **Constant System**: Support `#CONSTANT` syntax with substitution
- **Error Reporting**: Provide line-number specific error messages
- **Input Validation**: Validate all commands before execution

## 🧪 Testing and Quality Assurance

### Current Testing Approach

- **Manual Testing**: Comprehensive script validation and execution testing
- **Build Validation**: .NET compiler and WinUI 3 build process validation
- **Runtime Testing**: Real-world DuckyScript execution and validation

### Quality Assurance Commands

```bash
# Build and compile
dotnet build                     # Build the application
dotnet clean                     # Clean build artifacts

# Runtime testing
# Start application manually for testing
# Load sample scripts from helloworld.txt
# Test validation, execution, and encoding workflows
```

### Essential Test Scenarios

1. **Script Loading**: Test file picker and script loading functionality
2. **Validation Engine**: Test comprehensive command validation
3. **Execution Engine**: Test async script execution with timing
4. **Error Handling**: Test ValidationException with line numbers
5. **UI Responsiveness**: Ensure async operations don't block UI
6. **System Integration**: Test UAC, system restore, and external editor features

## 📋 Development Workflow

### Pre-Commit Checklist

Before committing code, ensure:

```bash
# Build validation
dotnet build                     # ✅ Application builds successfully
dotnet clean && dotnet build     # ✅ Clean build succeeds

# Manual testing checklist
# ✅ Script loading works correctly
# ✅ Validation provides accurate error reporting
# ✅ Execution engine handles timing properly
# ✅ UI remains responsive during operations
# ✅ InfoBar notifications display correctly
# ✅ All features work without exceptions
```

### Code Quality Standards

- **Exception Handling**: Always use try-catch with specific exception types
- **Async Operations**: Use `Task.Run` for CPU-bound operations, `await` for I/O
- **UI Updates**: Always marshal to UI thread using `DispatcherQueue`
- **Resource Cleanup**: Implement proper disposal patterns
- **Error Reporting**: Use custom exceptions with detailed context

## 🔄 Pull Request Guidelines

### PR Title Format

Use descriptive, component-oriented titles:

- `[UI] Enhance InfoBar notification system with custom styling`
- `[Engine] Improve async processing performance in DuckyScriptProcessing`
- `[Validation] Add support for new DuckyScript commands`
- `[Fix] Resolve thread marshaling issue in script execution`
- `[Feature] Implement external editor integration`
- `[Security] Enhance UAC handling and privilege management`

### PR Description Template

```markdown
## Summary

Brief description of changes made to the DuckyScript toolkit.

## Changes Made

- List specific changes to UI, processing engine, or validation
- Include any new DuckyScript command support
- Mention WinUI 3 or .NET 8 specific improvements
- Note any breaking changes to script compatibility

## Testing

- [ ] Application builds successfully (`dotnet build`)
- [ ] Manual testing of affected features completed
- [ ] Script validation and execution tested
- [ ] UI responsiveness verified for async operations
- [ ] No regressions in existing DuckyScript command support

## Compatibility

- [ ] Maintains backward compatibility with existing scripts
- [ ] WinUI 3 design guidelines followed
- [ ] .NET 8 best practices implemented

## Related Issues

Closes #[issue-number]
```

### Code Review Focus Areas

- [ ] **Async Patterns**: Proper use of async/await without blocking UI
- [ ] **Exception Handling**: Specific exception types with meaningful messages
- [ ] **WinUI 3 Integration**: Proper use of Fluent Design and modern controls
- [ ] **DuckyScript Compatibility**: Maintains support for all documented commands
- [ ] **Resource Management**: Proper cleanup of InputSimulator and file resources
- [ ] **Thread Safety**: UI operations properly marshaled to main thread

## 🎯 Development Environment Setup

### Prerequisites

- **Visual Studio 2022** with Windows App SDK workload
- **.NET 8 SDK** installed
- **Java Runtime** (for duckencode.jar functionality)
- **Git** for version control

### Initial Setup

```bash
# Clone and setup
git clone [repository-url]
cd "Next Gen Rubber Ducky Ninja"

# Open in Visual Studio
# File -> Open -> Project/Solution
# Select "Next Gen Rubber Ducky Ninja.sln"

# Build and run
# Press F5 or Build -> Start Debugging
```

### Required Dependencies

- **Windows App Runtime 1.6+** (automatically prompted if missing)
- **InputSimulator.dll** (included in /lib directory)
- **Java Runtime** (for binary encoding features)

## 🛡️ Security and Safety Considerations

### Input Validation

- **Script Content**: All DuckyScript commands validated before execution
- **File Paths**: Validate file picker results and path security
- **User Input**: NumberBox values validated for delay settings
- **Command Parameters**: Comprehensive parameter validation in Validation.cs

### System Integration Safety

- **UAC Handling**: Proper elevation request and graceful degradation
- **System Restore**: Automatic restore point creation before risky operations
- **InputSimulator Safety**: Proper exception handling for input simulation
- **File Operations**: Safe file handling with proper error management

### Error Handling Architecture

```csharp
// Use custom exceptions with context
throw new ValidationException("Invalid command syntax", lineNumber);

// Provide user-friendly error messages via InfoBar
ShowNotification("❌ Validation Failed", errorMessage, InfoBarSeverity.Error);

// Handle async operations safely
try
{
    await ProcessScriptAsync(filePath);
}
catch (ValidationException ex)
{
    // Handle with line-specific error reporting
}
```

## 🔧 Advanced Development Guidelines

### WinUI 3 Specific Patterns

```csharp
// Proper async file picker initialization
var picker = new FileOpenPicker();
var hwnd = WindowNative.GetWindowHandle(this);
InitializeWithWindow.Initialize(picker, hwnd);

// InfoBar notification pattern
var infoBar = new InfoBar()
{
    Title = title,
    Message = message,
    Severity = severity,
    IsOpen = true
};
```

### DuckyScript Processing Patterns

```csharp
// Async processing with UI updates
public async Task ReadFileAsync(String FilePath)
{
    string[] duckyFile = await File.ReadAllLinesAsync(FilePath);
    foreach (var currentLine in duckyFile)
    {
        Calculate(currentLine);
        await Task.Delay(1); // Allow UI updates
    }
}

// Proper validation with line numbers
public bool validateCode(string FilePath)
{
    // Implementation with ValidationException throwing
}
```

### Memory Management

- **InputSimulator**: Proper disposal after use
- **File Streams**: Use `using` statements for file operations
- **UI Elements**: Remove event handlers to prevent memory leaks
- **Timer Cleanup**: Dispose DispatcherQueue timers properly

## 📚 Key Features and Command Support

### Supported DuckyScript Commands

#### Basic Commands

- `STRING` / `STRINGLN` – Text input with constant substitution
- `DELAY` / `DEFAULT_DELAY` – Timing control
- `ENTER`, `TAB`, `ESC` – Basic keys
- `REM` – Comments (ignored during execution)

#### Modifier Keys

- `CTRL` / `CONTROL` – Control key combinations
- `ALT` – Alt key combinations
- `SHIFT` – Shift key combinations
- `GUI` / `WINDOWS` – Windows key combinations

#### Advanced Features

- `VAR $NAME = VALUE` – Variable definition with type checking
- `DEFINE #NAME value` – Constant definition with substitution
- `REPLAY` – Command repetition
- System controls (`CAPSLOCK`, `NUMLOCK`, etc.)

### Modern UI Features

- **Mica Backdrop**: System-integrated translucent appearance
- **Card-Based Layout**: Organized functionality sections
- **Real-time Notifications**: InfoBar system for all operations
- **Responsive Design**: Adapts to different window sizes
- **Fluent Design**: Native Windows 11 design language

## 📖 Additional Resources

- **Project Documentation**: See README.md for user-facing documentation
- **TODO Roadmap**: See TODO.md for planned features and improvements
- **WinUI 3 Documentation**: Microsoft's official WinUI 3 development guide
- **DuckyScript Reference**: Official USB Rubber Ducky documentation
- **InputSimulator Library**: Documentation for keyboard/mouse simulation

---

**Note for AI Agents**: This project prioritizes modern Windows development practices, user safety through comprehensive validation, and maintaining backward compatibility with existing DuckyScript files. Always ensure that UI operations are properly marshaled to the main thread and that async operations don't block the user interface.
