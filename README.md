# The Rubber Ducky Ninja - Next Generation

A modern, feature-rich toolkit for the USB Rubber Ducky, designed to simplify testing, validating, and encoding DuckyScript payloads. Built with **WinUI 3** and **.NET 8**, this Next Generation version provides enhanced performance, modern UI, and comprehensive DuckyScript support.

![The Rubber Ducky Ninja - Next Gen](https://i.imgur.com/T2nSDtB.png)

## Video

You can find videos demonstrating this project on my YouTube channel [here.](https://www.youtube.com/@PetesGonzalezCybernetics)

## About

The Rubber Ducky Ninja - Next Generation is a powerful, modern toolkit that makes working with DuckyScripts easier and more efficient than ever before. This complete rewrite in WinUI 3 allows you to test your code by emulating what the ducky would do, validate your scripts with comprehensive error checking, execute scripts with configurable timing, and easily encode your scripts into binary format with just one click. The Next Gen version features a beautiful Fluent Design interface, async processing, enhanced validation, and improved performance.

## In-Depth Overview

### Project Structure

- **Next Gen Rubber Ducky Ninja.csproj** – The project file (Visual Studio) for the Next Generation toolkit, configured for unpackaged WinUI 3 deployment with .NET 8.0 targeting Windows 10.0.19041.0.
- **MainWindow.xaml** – The main UI definition using modern WinUI 3 controls with Fluent Design, Mica backdrop, and card-based layout optimized for the enhanced user experience.
- **MainWindow.xaml.cs (21KB)** – The primary UI controller handling all user interactions, async operations, and state management. Features modern file picker integration, real-time InfoBar notifications, and comprehensive error handling with visual feedback.
- **App.xaml** – Application-level resources and theming configuration for the WinUI 3 application.
- **App.xaml.cs** – Application entry point and lifecycle management with proper initialization.
- **DuckyScriptProcessing.cs (21KB)** – The enhanced "engine" that reads DuckyScript files, parses each line using async processing, and emulates keystrokes via InputSimulator. Now supports variables, constants, advanced error handling, and both synchronous/asynchronous execution modes with configurable delays.
- **Validation.cs (13KB)** – Enhanced validation class with custom `ValidationException` that provides precise line-by-line error reporting, comprehensive syntax checking, and full support for advanced DuckyScript features including variables and constants with proper type checking.
- **lib/InputSimulator.dll** – External library for low-level keyboard and mouse input simulation, referenced with proper HintPath configuration.
- **duckencode.jar** – Official DuckyScript to binary encoder (Java-based) for USB Rubber Ducky deployment, automatically detected by the application.
- **restore.vbs** – Windows system restore automation script for creating safety restore points before script execution.
- **helloworld.txt** – Sample DuckyScript file demonstrating basic functionality and serving as a template.
- **app.manifest** – Application manifest configured with UAC elevation capabilities for system-level operations.
- **bin/** – Output folder where the compiled executable (`Next Gen Rubber Ducky Ninja.exe`) and encoded binary payloads are stored.

### Next Generation Enhancements

The Next Gen version represents a complete architectural overhaul with the following major improvements:

#### **Modern UI Architecture**

- **WinUI 3 Framework**: Native Windows 11 design language with system-integrated Mica backdrop
- **Fluent Design System**: Card-based layout with consistent spacing, typography, and color schemes
- **Responsive Interface**: Automatically adapts to different window sizes and screen resolutions (1000x700 default)
- **Real-time Feedback**: InfoBar notification system providing immediate success, warning, error, and informational messages
- **Accessibility First**: Full keyboard navigation support and screen reader compatibility

#### **Enhanced Performance & Reliability**

- **Async Processing**: Non-blocking file operations and script execution using Task-based asynchronous patterns
- **Memory Optimization**: Improved resource management with proper disposal patterns and garbage collection
- **Error Resilience**: Comprehensive exception handling with graceful degradation and recovery options
- **Thread Safety**: Proper UI thread marshaling for all operations ensuring stability
- **Resource Cleanup**: Automatic cleanup of temporary files and system resources

#### **Advanced System Integration**

- **UAC Integration**: Intelligent elevation handling for system-level operations with user-friendly prompts
- **System Restore**: Built-in restore point creation before potentially risky script executions
- **File Association**: Enhanced file management with external editor integration (recent files tracking planned for future version)
- **Windows App Runtime**: Leverages Windows App SDK 1.7+ for modern Windows features and APIs

### Supported Commands

The Next Generation toolkit (via enhanced DuckyScriptProcessing.cs) supports a comprehensive set of DuckyScript commands with improved validation and execution. Below is a complete list of currently supported commands:

#### Basic Commands

- **REM** – A comment (remark) that is ignored by the compiler. Can be used to add comments or vertical spacing in your script. Blank lines are also ignored during execution.
- **STRING** – Types the given text (without an ENTER). Enhanced with constant substitution using #CONSTANT syntax and variable support.
- **STRINGLN** – Types the given text and presses ENTER. Enhanced with constant substitution using #CONSTANT syntax and variable support.
- **ENTER** – Simulates pressing the ENTER key.
- **DELAY** – Pauses (sleeps) for the given number of milliseconds. Now supports constant substitution (e.g., DELAY #WAIT).
- **DEFAULT_DELAY (or DEFAULTDELAY)** – Sets a "global" delay (in milliseconds) between each command. Configurable via the UI (0-10000ms range).

#### Modifier Keys

- **GUI (or WINDOWS)** – Simulates pressing the Windows key (or a modified key stroke, for example, WIN + R). WINDOWS is an alias for GUI. Enhanced validation ensures proper key combinations.
- **ALT** – Simulates pressing the ALT key (or a modified key stroke, for example, ALT + F4). Supports function keys (F1-F12), letters (A-Z), numbers (0-9), and special keys including arrow keys.
- **CONTROL (or CTRL)** – Simulates pressing the CONTROL key (or a modified key stroke, for example, CTRL + C). Supports function keys, letters, numbers, and special key combinations with enhanced validation.
- **SHIFT** – Simulates pressing the SHIFT key (or a modified key stroke, for example, SHIFT + DELETE). Supports various key combinations including arrow keys, special keys, and proper validation.

#### Navigation and Special Keys

- **TAB** – Simulates pressing the TAB key.
- **UP (or UPARROW), DOWN (or DOWNARROW), LEFT (or LEFTARROW), RIGHT (or RIGHTARROW)** – Simulates pressing the arrow keys with enhanced responsiveness.
- **DELETE** – Simulates pressing the DELETE key.
- **SPACE** – Simulates pressing the SPACE key.
- **BACKSPACE** – Simulates pressing the BACKSPACE key.
- **HOME** – Simulates pressing the HOME key.
- **END** – Simulates pressing the END key.
- **INSERT** – Simulates pressing the INSERT key.
- **PAGEUP** – Simulates pressing the PAGE UP key.
- **PAGEDOWN** – Simulates pressing the PAGE DOWN key.
- **PRINTSCREEN** – Simulates pressing the PRINT SCREEN key.
- **APP (or MENU)** – Simulates pressing the context menu key (equivalent to SHIFT + F10).
- **ESC (or ESCAPE)** – Simulates pressing the ESCAPE key.

#### Function Keys

- **F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12** – Simulates pressing the respective function keys. Can be used alone or in combination with modifier keys (CTRL, ALT, SHIFT).

#### System Controls

- **CAPSLOCK** – Toggles the CAPS LOCK state. The toolkit automatically tracks caps state and converts subsequent STRING commands to uppercase when caps lock is enabled.
- **NUMLOCK** – Toggles the NUM LOCK state.
- **SCROLLLOCK** – Toggles the SCROLL LOCK state.
- **PAUSE** – Simulates pressing the PAUSE key.
- **BREAK** – Simulates pressing the BREAK key (CTRL + PAUSE).

#### Advanced Features

- **REPLAY** – Replays (repeats) the last command (except "REM", "REPLAY", "DEFINE", or "VAR") a given number of times. Enhanced with better command tracking.
- **DEFINE** – Defines a constant for use in the script. Constants are prefixed with # and can be used in STRING, STRINGLN, and DELAY commands with real-time substitution.
- **VAR** – Defines a variable that can hold unsigned integers (0-65535) or boolean values (TRUE/FALSE). Variables are prefixed with $ and feature enhanced type checking and validation.

### Constants and Variables

The Next Generation toolkit supports both enhanced constants (DEFINE) and variables (VAR) commands with improved validation and error handling. They serve different purposes and have different syntax:

#### Constants (DEFINE)

Constants are defined using the DEFINE command and are used for find-and-replace at compile time. They are prefixed with # and can contain any string value. The Next Gen version features real-time substitution and enhanced validation.

1. Define a constant:

```
DEFINE #WAIT 2000
DEFINE #TEXT Hello World from Next Gen
DEFINE #MYURL github.com/plgonzalezrx8
DEFINE #USERNAME admin
DEFINE #PASSWORD MySecurePass123
```

2. Use the constant in commands:

```
DELAY #WAIT
STRINGLN #TEXT
STRING https://#MYURL
REM Login sequence using constants
STRING #USERNAME
TAB
STRING #PASSWORD
ENTER
```

Constants must:

- Be defined with a # prefix (e.g., #VARIABLE)
- Contain only letters, numbers, and underscores (after the # prefix)
- Be defined before they are used
- Can be used in STRING, STRINGLN, and DELAY commands
- Are validated at compile time with detailed error reporting

#### Variables (VAR)

Variables are defined using the VAR command and can hold unsigned integers (0-65535) or boolean values. They are prefixed with $ and feature enhanced type checking and validation in the Next Gen version.

1. Define a variable:

```
VAR $BLINK = TRUE
VAR $BLINK_TIME = 1000
VAR $RETRY_COUNT = 5
VAR $DEBUG_MODE = FALSE
```

2. Use the variable in commands:

```
REM Variables are defined and stored for future use
REM Note: Variable substitution in STRING commands and conditional statements (IF/ENDIF) are planned for future versions
REM Currently variables are stored and validated but not yet substituted in output
```

Variables must:

- Be defined with a $ prefix (e.g., $VARIABLE)
- Contain only letters, numbers, and underscores (after the $ prefix)
- Hold unsigned integers (0-65535) or boolean values (TRUE/FALSE)
- Be defined before they are used
- Pass enhanced validation with line-by-line error reporting

Example script using both constants and variables:

```
REM Enhanced Next Gen script with constants and variables
DEFINE #WAIT 2000
DEFINE #TEXT Hello from Next Generation
DEFINE #MYURL github.com/plgonzalezrx8/Next-Gen-Rubber-Ducky-Ninja

VAR $BLINK = TRUE
VAR $BLINK_TIME = 1500
VAR $ITERATIONS = 3

REM Use constants for consistent timing
DELAY #WAIT
STRINGLN #TEXT
STRING Visit: https://#MYURL

REM Variables provide dynamic content
STRING Blink enabled: $BLINK
ENTER
STRING Blink timing: $BLINK_TIME ms
ENTER
STRING Iterations: $ITERATIONS
```

This will:

1. Define three constants with enhanced validation
2. Define three variables with type checking
3. Use constants for consistent delays and text
4. Demonstrate variable usage in STRING commands
5. Show proper script structure and commenting

### Payload Processing

The Next Generation version features significantly enhanced payload processing with modern async architecture:

- **Loading a Payload**: In the main window, click "Load File" to select a DuckyScript (txt) file using the modern WinUI 3 file picker. The toolkit now maintains the original file path and provides real-time feedback via InfoBar notifications.

- **Validating a Payload**: Click "Validate Script" to run enhanced validation (Validation.cs) which provides comprehensive error checking including:
  - Line-by-line syntax validation with precise error locations
  - Advanced command parameter validation
  - Variable and constant definition checking
  - Type validation for variable values
  - Enhanced error messages with actionable feedback
  - Real-time validation status updates

- **Configuring Execution**: Use the delay NumberBox control to set timing between commands (0-10000ms range) with real-time validation and visual feedback.

- **Executing a Payload**: Click "Execute Script" to run your payload using the enhanced async processing engine. The execution features:
  - Non-blocking UI during script execution
  - Real-time progress feedback
  - Enhanced error handling and recovery
  - Configurable timing controls
  - System restore integration for safety
  - UAC-aware execution handling

- **Encoding a Payload (Enhanced)**: If "duckencode.jar" is present (automatically detected), click "Encode Script" to convert your DuckyScript to binary format. The Next Gen version features:
  - Automatic Java and duckencode.jar detection
  - Real-time encoding progress feedback
  - Enhanced error handling for encoding failures
  - Modern file save dialog integration
  - Validation before encoding to prevent errors

### Usage Instructions

The Next Generation interface provides a streamlined, modern workflow:

1. **Load File** – Click "Load File" to select a DuckyScript (txt) file using the modern file picker. The application provides immediate feedback and enables subsequent operations.

2. **Configure Timing (Optional)** – Use the delay NumberBox to set global timing between commands (0-10000ms). This setting applies to all commands and can be adjusted in real-time.

3. **Validate Script** – Click "Validate Script" to perform comprehensive validation including:
   - Syntax checking for all commands
   - Variable and constant validation
   - Parameter verification
   - Type checking for variables
   - Line-by-line error reporting with precise locations

   Success enables the Execute button and provides positive feedback via InfoBar.

4. **Execute Script** – Click "Execute Script" to run your payload with enhanced features:
   - Async execution preventing UI blocking
   - Real-time progress feedback
   - Enhanced error handling
   - System restore integration
   - UAC-aware processing

5. **Encode to Binary (Optional)** – If duckencode.jar is available, click "Encode Script" to generate a binary file for USB Rubber Ducky deployment with modern progress feedback and error handling.

6. **Additional Features**:
   - **Edit Script**: Launch the script in your preferred external text editor
   - **System Restore**: Create restore points before potentially risky executions
   - **UAC Settings**: Configure UAC handling for elevated operations
   - **About**: View application information and version details

### Comments and Spacing

The Next Generation toolkit supports enhanced comment and spacing features:

1. Using the REM command with improved parsing:

```
REM ===================================================
REM Next Generation Rubber Ducky Ninja Script
REM Author: [Your Name]
REM Version: 2.0
REM Description: Enhanced script with modern features
REM ===================================================
REM This section opens notepad and types a message
```

2. Using blank lines for better script organization:

```
REM Script initialization
DEFINE #WAIT 1000
VAR $DEBUG = TRUE

REM Open application
GUI r
DELAY #WAIT
STRING notepad.exe
ENTER

REM Type content
DELAY #WAIT
STRING Hello from Next Generation!
STRINGLN
STRING Debug mode: $DEBUG
```

Both REM commands and blank lines are handled more efficiently in the Next Gen version with improved parsing and better memory management.

### System Requirements and Installation

#### System Requirements

- **Operating System**: Windows 10 version 1903 (build 18362) or later / Windows 11 (recommended)
- **Runtime**: Windows App Runtime 1.6+ (automatically prompted if missing)
- **Framework**: .NET 8.0 Runtime (included with Windows App Runtime)
- **Java**: Required for binary encoding functionality (if using duckencode.jar)
- **Memory**: 512MB RAM minimum, 1GB recommended
- **Storage**: 50MB available disk space
- **Permissions**: Administrator privileges recommended for full functionality

#### Installation

1. Download the latest release from the GitHub releases page
2. Extract the archive to your desired location (e.g., `C:\Tools\RubberDuckyNinja`)
3. Run `Next Gen Rubber Ducky Ninja.exe`
4. If prompted, install Windows App Runtime from the Microsoft Store or download directly from Microsoft
5. Grant UAC permissions when prompted for full functionality

#### First Run

The application will:

- Check for required dependencies (duckencode.jar, Java)
- Display welcome message with feature availability
- Enable/disable features based on detected components
- Provide guidance for missing dependencies

### Security and Safety Features

The Next Generation version includes comprehensive safety mechanisms:

#### UAC Integration and Privilege Management

- **Smart UAC Handling**: Automatic detection of operations requiring elevation
- **Contextual Prompts**: Clear explanations when elevation is needed
- **Graceful Degradation**: Continues operation with reduced functionality if elevation is denied
- **Principle of Least Privilege**: Only requests elevation when absolutely necessary

#### System Restore Integration

- **Automatic Restore Points**: Creates restore points before potentially risky script executions
- **User Control**: Option to disable restore point creation for experienced users
- **Restore Point Validation**: Verifies restore point creation success before proceeding
- **Recovery Guidance**: Provides clear instructions for system recovery if needed

#### Enhanced Validation and Error Prevention

- **Pre-execution Validation**: Comprehensive script validation before any execution
- **Real-time Error Detection**: Immediate feedback for syntax and logic errors
- **Safe Defaults**: Conservative default settings to prevent system damage
- **Input Sanitization**: All user inputs are validated and sanitized

### Version History and Migration

#### Version 2.0.0 - Next Generation (Current)

**Major Changes:**

- Complete architectural rewrite using WinUI 3 and .NET 8
- Modern Fluent Design interface with Mica backdrop
- Async processing architecture for better performance
- Enhanced validation with line-by-line error reporting
- Improved variable and constant support
- Real-time notifications and feedback
- Unpackaged deployment for easier distribution
- Better memory management and resource cleanup

**Breaking Changes from v1.x:**

- UI completely redesigned (WinForms → WinUI 3)
- Configuration format updated
- Enhanced validation may catch errors missed in v1.x
- Different keyboard handling due to InputSimulator improvements

#### Version 1.x - Legacy (WinForms)

**Features:**

- Original Windows Forms implementation
- Basic DuckyScript processing
- Simple validation
- MSIX packaged deployment
- Limited error handling

**Migration Notes:**

- Scripts from v1.x are fully compatible with v2.0
- Enhanced validation may identify previously undetected issues
- UI workflow remains conceptually similar but with modern controls
- Performance significantly improved in v2.0

## Contributing

If you're interested in contributing to The Rubber Ducky Ninja - Next Generation, we welcome your involvement! This project benefits from community contributions including bug reports, feature requests, code improvements, and documentation enhancements.

### How to Contribute

#### Reporting Issues

- Use GitHub Issues to report bugs or request features
- Provide detailed reproduction steps for bugs
- Include system information and error messages
- Check existing issues before creating new ones

#### Development Contributions

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes with proper testing
4. Commit with descriptive messages (`git commit -m 'Add amazing feature'`)
5. Push to your branch (`git push origin feature/amazing-feature`)
6. Open a Pull Request with detailed description

#### Development Setup

1. Clone the repository: `git clone https://github.com/plgonzalezrx8/Next-Gen-Rubber-Ducky-Ninja.git`
2. Open `Next Gen Rubber Ducky Ninja.sln` in Visual Studio 2022
3. Ensure you have the following workloads installed:
   - .NET Desktop Development
   - Windows App SDK (via Individual Components)
4. Restore NuGet packages
5. Build and run the project (F5)

#### Coding Standards

- Follow C# coding conventions
- Use async/await patterns for I/O operations
- Implement proper error handling and logging
- Add XML documentation for public methods
- Maintain WinUI 3 design principles
- Test your changes thoroughly

### Areas for Contribution

- **Bug Fixes**: Help identify and fix issues
- **Feature Development**: Implement features from the TODO.md
- **Documentation**: Improve README, add code comments, create tutorials
- **Testing**: Add unit tests and integration tests
- **UI/UX**: Enhance the interface and user experience
- **Performance**: Optimize code for better performance
- **Accessibility**: Improve accessibility features

### Questions and Support

For questions, suggestions, or general discussion:

- Create a GitHub Issue for technical questions
- Reach out to [@plgonzalezrx8](https://github.com/plgonzalezrx8) for collaboration inquiries
- Check the TODO.md for planned features and development roadmap
