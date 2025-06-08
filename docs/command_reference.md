# Command Reference

This document lists the DuckyScript commands supported by the Next Gen Rubber Ducky Ninja toolkit.

## Basic Commands
- **REM** – Comment line ignored by the compiler.
- **STRING** – Types text without pressing `ENTER`.
- **STRINGLN** – Types text and presses `ENTER`.
- **ENTER** – Simulates the `ENTER` key.
- **DELAY** – Waits the specified milliseconds (supports constants).
- **DEFAULT_DELAY** – Sets a global delay between commands.

## Modifier Keys
- **GUI / WINDOWS** – Presses the Windows key or combination (e.g., `WIN + R`).
- **ALT** – Presses `ALT` or combination.
- **CONTROL / CTRL** – Presses `CTRL` or combination.
- **SHIFT** – Presses `SHIFT` or combination.

## Navigation & Special Keys
TAB, arrow keys, DELETE, SPACE, BACKSPACE, HOME, END, INSERT, PAGEUP, PAGEDOWN, PRINTSCREEN, APP/MENU, ESC.

## Function Keys
`F1`‒`F12` – Can be used alone or with modifiers.

## System Controls
CAPSLOCK, NUMLOCK, SCROLLLOCK, PAUSE, BREAK.

## Advanced Features
- **REPLAY** – Repeats the last command a number of times.
- **DEFINE** – Declares a constant prefixed with `#`.
- **VAR** – Declares a variable prefixed with `$`.

## Constants and Variables
Constants use `DEFINE` and are substituted at compile time. Variables use `VAR` and store unsigned integers or boolean values. Both must be defined before use and contain only letters, numbers and underscores (after their prefix).

Example:
```text
DEFINE #WAIT 2000
DEFINE #TEXT Hello
VAR $ITERATIONS = 3
DELAY #WAIT
STRINGLN #TEXT
STRING Iterations: $ITERATIONS
```

## Payload Processing Workflow
1. **Load File** – Select a DuckyScript file with the file picker.
2. **Validate Script** – Performs syntax and parameter checks.
3. **Execute Script** – Runs the payload asynchronously with progress feedback.
4. **Encode Script** – Generates a binary using `duckencode.jar` if available.

## Usage Tips
- Use `DEFAULT_DELAY` or the UI control to configure timing.
- Blank lines and `REM` comments are ignored during execution.

