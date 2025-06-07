using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Next_Gen_Rubber_Ducky_Ninja
{

    class Validation
    {
        //This class is going to be used to validate the DuckyScript (instead of just checking the 1st word like before)

        private string[] validFKeys = new string[12] { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" };
        private string[] validShiftKeys = new string[12] { "DELETE", "HOME", "INSERT", "PAGEUP", "PAGEDOWN", "WINDOWS", "GUI", "UPARROW", "DOWNARROW", "LEFTARROW", "RIGHTARROW", "TAB" };
        private string[] validCTRLkeys = new string[4] { "BREAK", "PAUSE", "ESCAPE", "ESC" };
        private string[] validAltKeys = new string[6] { "ALT", "END", "ESC", "ESCAPE", "SPACE", "TAB" };
        private string[] validBooleanValues = new string[2] { "TRUE", "FALSE" };

        public class ValidationException : Exception
        {
            public int LineNumber { get; }
            
            public ValidationException(string message, int lineNumber) : base(message)
            {
                LineNumber = lineNumber;
            }
        }

        public bool LineCheck(string command, string keys, int currentLine)
        {
            try
            {
                switch (command)
                {
                    case "VAR":
                        if (string.IsNullOrEmpty(keys))
                        {
                            throw new ValidationException("VAR command requires a variable name and value.", currentLine);
                        }
                        // Check for proper VAR syntax: VAR $NAME = VALUE
                        if (!keys.Contains("="))
                        {
                            throw new ValidationException("VAR command requires an equals sign (e.g., VAR $NAME = VALUE).", currentLine);
                        }
                        string[] varParts = keys.Split(new[] { '=' }, 2);
                        if (varParts.Length != 2)
                        {
                            throw new ValidationException("VAR command requires both a variable name and value.", currentLine);
                        }
                        string varNameWithPrefix = varParts[0].Trim();
                        string varValue = varParts[1].Trim();
                        
                        // Check variable name
                        if (!varNameWithPrefix.StartsWith("$"))
                        {
                            throw new ValidationException("Variable name must start with $ (e.g., $VARIABLE).", currentLine);
                        }
                        string varNameWithoutPrefix = varNameWithPrefix.Substring(1); // Remove $ for validation
                        if (string.IsNullOrEmpty(varNameWithoutPrefix) || !Regex.IsMatch(varNameWithoutPrefix, @"^[A-Za-z0-9_]+$"))
                        {
                            throw new ValidationException("Variable name must contain only letters, numbers, and underscores (after the $ prefix).", currentLine);
                        }
                        
                        // Check variable value
                        if (validBooleanValues.Contains(varValue.ToUpper()))
                        {
                            // Boolean value is valid
                            break;
                        }
                        
                        // Try to parse as integer
                        if (!int.TryParse(varValue, out int intValue))
                        {
                            throw new ValidationException("Variable value must be an integer (0-65535) or TRUE/FALSE.", currentLine);
                        }
                        
                        // Check integer range
                        if (intValue < 0 || intValue > 65535)
                        {
                            throw new ValidationException("Integer variable value must be between 0 and 65535.", currentLine);
                        }
                        break;
                        
                    case "DEFINE":
                        if (string.IsNullOrEmpty(keys))
                        {
                            throw new ValidationException("DEFINE command requires a variable name and value.", currentLine);
                        }
                        string[] defineParts = keys.Split(new[] { ' ' }, 2);
                        if (defineParts.Length != 2)
                        {
                            throw new ValidationException("DEFINE command requires both a variable name and value (e.g., DEFINE #MYVAR myvalue).", currentLine);
                        }
                        string varName = defineParts[0].Trim();
                        if (varName.StartsWith("#"))
                        {
                            varName = varName.Substring(1);
                        }
                        if (string.IsNullOrEmpty(varName) || !Regex.IsMatch(varName, @"^[A-Za-z0-9_]+$"))
                        {
                            throw new ValidationException("Variable name must contain only letters, numbers, and underscores (after the # prefix if used).", currentLine);
                        }
                        break;
                        
                    case "REM":
                        break;
                        
                    case "STRING":
                        if (keys.Length <= 0)
                        {
                            // This is a warning, not an error
                            // Could implement a warning system later
                        }
                        break;
                        
                    case "STRINGLN":
                        if (keys.Length <= 0)
                        {
                            // This is a warning, not an error
                            // Could implement a warning system later
                        }
                        break;
                        
                    case "DEFAULT_DELAY":
                    case "DEFAULTDELAY":
                        try
                        {
                            Convert.ToInt32(keys);
                        }
                        catch
                        {
                            throw new ValidationException("The command following DEFAULT_DELAY/DEFAULTDELAY is not an integer (ex 500)", currentLine);
                        }
                        break;

                    case "DELAY":
                        try
                        {
                            Convert.ToInt32(keys);
                        }
                        catch
                        {
                            throw new ValidationException("The command following delay is not a integer (ex 500)", currentLine);
                        }
                        break;

                    case "WINDOWS":
                    case "GUI":
                        // GUI/WINDOWS can be used alone or with a single key
                        // Valid examples: GUI, GUI r, WINDOWS r
                        if (keys.Length > 0 && keys.Split(' ').Length > 1)
                        {
                            throw new ValidationException("GUI/WINDOWS can only be used with a single key or alone.", currentLine);
                        }
                        break;

                    case "ENTER":
                        if (keys.Length > 0)
                        {
                            throw new ValidationException("There is a command following ENTER. ENTER function doesn't support keyboard combos", currentLine);
                        }
                        break;

                    case "APP":
                    case "MENU":
                        if (keys.Length > 0)
                        {
                            throw new ValidationException("There is a command following MENU/APP. MENU function doesn't support keyboard combos", currentLine);
                        }
                        break;

                    case "SHIFT":
                        if (keys.Length > 0 && !validShiftKeys.Contains(keys))
                        {
                            throw new ValidationException("The command following SHIFT is invalid. Please look at DuckyScript documentation for more info", currentLine);
                        }
                        break;

                    case "ALT":
                        // ALT can be used alone or with function keys, letters, or other valid keys
                        if (keys.Length > 0)
                        {
                            // Check if it's a valid F-key, single letter, or in validAltKeys
                            if (!validFKeys.Contains(keys) && !validAltKeys.Contains(keys) && 
                                !(keys.Length == 1 && char.IsLetterOrDigit(keys[0])))
                            {
                                throw new ValidationException("The command following ALT is not valid. See official DuckyScript documentation for compatible functions", currentLine);
                            }
                        }
                        break;

                    case "CONTROL":
                    case "CTRL":
                        // CTRL can be used alone or with function keys, letters, or other valid keys
                        if (keys.Length > 0)
                        {
                            // Check if it's a valid F-key, single letter, or in validCTRLkeys
                            if (!validFKeys.Contains(keys) && !validCTRLkeys.Contains(keys) && 
                                !(keys.Length == 1 && char.IsLetterOrDigit(keys[0])))
                            {
                                throw new ValidationException("The command following CTRL is not valid. See official DuckyScript documentation for compatible functions", currentLine);
                            }
                        }
                        break;

                    case "DOWNARROW":
                    case "DOWN":
                    case "LEFTARROW":
                    case "LEFT":
                    case "RIGHTARROW":
                    case "RIGHT":
                    case "UPARROW":
                    case "UP":
                    case "BREAK":
                    case "PAUSE":
                    case "CAPSLOCK":
                    case "DELETE":
                    case "END":
                    case "ESC":
                    case "ESCAPE":
                    case "HOME":
                    case "INSERT":
                    case "NUMLOCK":
                    case "PAGEUP":
                    case "PAGEDOWN":
                    case "PRINTSCREEN":
                    case "SCROLLLOCK":
                    case "SPACE":
                    case "TAB":
                        // These commands should not have additional parameters
                        if (keys.Length > 0)
                        {
                            throw new ValidationException($"The command {command} does not accept additional parameters.", currentLine);
                        }
                        break;

                    case "F1":
                    case "F2":
                    case "F3":
                    case "F4":
                    case "F5":
                    case "F6":
                    case "F7":
                    case "F8":
                    case "F9":
                    case "F10":
                    case "F11":
                    case "F12":
                        // Function keys should not have additional parameters
                        if (keys.Length > 0)
                        {
                            throw new ValidationException($"The function key {command} does not accept additional parameters.", currentLine);
                        }
                        break;

                    case "REPLAY":
                        // REPLAY should not have additional parameters
                        if (keys.Length > 0)
                        {
                            throw new ValidationException("REPLAY command does not accept additional parameters.", currentLine);
                        }
                        break;

                    default:
                        // Unknown command - could be a single character or typo
                        if (command.Length > 1)
                        {
                            // Could be an unknown command, but we'll allow it for now
                            // Future enhancement: more strict validation
                        }
                        break;
                }
                return true;
            }
            catch (ValidationException)
            {
                // Re-throw validation exceptions
                throw;
            }
            catch (Exception)
            {
                // Handle any other exceptions as validation errors
                throw new ValidationException($"Unexpected error validating line: {command} {keys}", currentLine);
            }
        }
    }
} 