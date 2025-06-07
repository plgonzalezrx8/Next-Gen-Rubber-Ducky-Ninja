using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Next_Gen_Rubber_Ducky_Ninja
{ 
    internal class DuckyScriptProcessing
    {
        private bool defaultdelay = false;
        private int defaultdelayvalue = 0;
        private string lastCommand = "";
        private string lastKey = "";
        private bool isCapsEnabled = false;
        private Dictionary<string, string> constants = new Dictionary<string, string>();
        private Dictionary<string, object> variables = new Dictionary<string, object>();
        private Validation validation = new Validation();
                
        public void SetDelay(int delay) //sets the global delay
        {
            if (delay > 1)
            {
                defaultdelay = true; 
            }
            defaultdelayvalue = delay;
        }

        public async Task ReadFileAsync(String FilePath) //Reads file and calls calculate for each line (async version)
        {
            string[] duckyFile = await File.ReadAllLinesAsync(FilePath);
            foreach (var currentLine in duckyFile)
            {
                Calculate(currentLine);
                await Task.Delay(1); // Allow UI updates
            }
        }

        public void ReadFile(String FilePath) //Synchronous version for compatibility
        {
            string[] duckyFile = File.ReadAllLines(FilePath);
            foreach (var currentLine in duckyFile)
            {
                Calculate(currentLine);
            }
        }

        public void Calculate(string currentLine) //splits the line into command & keys
        {
            // Skip empty lines
            if (string.IsNullOrWhiteSpace(currentLine))
                return;

            string[] words = currentLine.Split(' ');
            string command = words[0];
            string keys = "";
            int flag = 0;
            for (int i = 1; i < words.Length; i++)
            {
                if (flag == 0)
                {
                    keys += words[i];
                    flag++;
                }
                else
                {
                    keys += " " + words[i];
                }
            }
            KeyboardAction(command, keys);
        }

        private void CheckDefaultSleep() //checks if their is a delay set. If so, delays
        {
            if (defaultdelay == true)
            {
                Thread.Sleep(defaultdelayvalue);
            }
        }

        private void setLastCommand(string command, string keys) //sets the last command (for replay function)
        {
            lastCommand = command;
            lastKey = keys;
        }

        private string SubstituteConstants(string input) //substitutes constants in strings
        {
            if (string.IsNullOrEmpty(input)) return input;
            
            // Match constants in the format #CONSTANT
            var matches = Regex.Matches(input, @"#([A-Za-z0-9_]+)");
            string result = input;
            
            foreach (Match match in matches)
            {
                string constName = match.Groups[1].Value;
                if (constants.ContainsKey(constName))
                {
                    // Replace the entire match (#CONSTANT) with the constant value
                    result = result.Replace(match.Value, constants[constName]);
                }
            }
            
            return result;
        }

        public bool validateCode(string FilePath) //validates the commands in a duckyscript
        {
            string[] duckyFile = File.ReadAllLines(FilePath);
            int currentLineNum = 1;
            foreach (var currentLine in duckyFile)
            {
                string[] words = currentLine.Split(' ');
                string command = words[0];
                string keys = "";
                int flag = 0;
                for (int i = 1; i < words.Length; i++)
                {
                    if (flag == 0)
                    {
                        keys += words[i];
                        flag++;
                    }
                    else
                    {
                        keys += " " + words[i];
                    }
                }
                bool result = validation.LineCheck(command,keys,currentLineNum);
                if (result == false)
                {
                    return false;
                }
                currentLineNum++;
            }
            return true;
        }

        private object GetVariableValue(string varName)
        {
            if (string.IsNullOrEmpty(varName)) return null;
            
            // Remove $ prefix if present
            if (varName.StartsWith("$"))
            {
                varName = varName.Substring(1);
            }
            
            if (variables.ContainsKey(varName))
            {
                return variables[varName];
            }
            
            return null;
        }

        private void KeyboardAction(string command, string keys) //executes the code line by line.
        {
            string keyboardkey = keys.ToUpper();
            VirtualKeyCode code;
            try
            {

                switch (command)
                {
                    case "DEFAULT_DELAY":
                    case "DEFAULTDELAY":
                        defaultdelay = true;
                        defaultdelayvalue += Convert.ToInt32(keys);
                        break;

                    case "DELAY":
                        CheckDefaultSleep();
                        Thread.Sleep(Convert.ToInt32(keys));
                        break;

                    case "STRING":
                        CheckDefaultSleep();
                        string textToType = SubstituteConstants(keys);
                        if (isCapsEnabled == true)
                        {
                            InputSimulator.SimulateTextEntry((textToType.ToUpper()));
                        } else
                        {
                            InputSimulator.SimulateTextEntry(textToType);
                        }
                        break;

                    case "STRINGLN":
                        CheckDefaultSleep();
                        string textToTypeLn = SubstituteConstants(keys);
                        if (isCapsEnabled == true)
                        {
                            InputSimulator.SimulateTextEntry((textToTypeLn.ToUpper()));
                        } else
                        {
                            InputSimulator.SimulateTextEntry(textToTypeLn);
                        }
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                        break;

                    case "WINDOWS":
                    case "GUI":
                        CheckDefaultSleep();
                        if (keyboardkey.Length > 0)
                        {
                            code = GetKeyCode(keyboardkey);
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LWIN, code);
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LWIN);
                        }
                        break;

                    case "MENU":
                    case "APP":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.APPS);
                        break;

                    case "SHIFT":
                        CheckDefaultSleep();
                        if (keyboardkey.Length > 0)
                        {
                            code = GetKeyCode(keyboardkey);
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LSHIFT, code);
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LSHIFT);
                        }
                        break;

                    case "ALT":
                        CheckDefaultSleep();
                        if (keyboardkey.Length > 0)
                        {
                            code = GetKeyCode(keyboardkey);
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LMENU, code);
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LMENU);
                        }
                        break;

                    case "CONTROL":
                    case "CTRL":
                        CheckDefaultSleep();
                        if (keyboardkey.Length > 0)
                        {
                            code = GetKeyCode(keyboardkey);
                            InputSimulator.SimulateModifiedKeyStroke(VirtualKeyCode.LCONTROL, code);
                        }
                        else
                        {
                            InputSimulator.SimulateKeyPress(VirtualKeyCode.LCONTROL);
                        }
                        break;

                    case "DOWNARROW":
                    case "DOWN":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.DOWN);
                        break;

                    case "LEFTARROW":
                    case "LEFT":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.LEFT);
                        break;

                    case "RIGHTARROW":
                    case "RIGHT":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RIGHT);
                        break;

                    case "UPARROW":
                    case "UP":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.UP);
                        break;

                    case "BREAK":
                    case "PAUSE":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.PAUSE);
                        break;

                    case "CAPSLOCK":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.CAPITAL);
                        isCapsEnabled = !isCapsEnabled;
                        break;

                    case "DELETE":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.DELETE);
                        break;

                    case "END":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.END);
                        break;

                    case "ESC":
                    case "ESCAPE":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.ESCAPE);
                        break;

                    case "HOME":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.HOME);
                        break;

                    case "INSERT":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.INSERT);
                        break;

                    case "NUMLOCK":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.NUMLOCK);
                        break;

                    case "PAGEUP":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.PRIOR);
                        break;

                    case "PAGEDOWN":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.NEXT);
                        break;

                    case "PRINTSCREEN":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SNAPSHOT);
                        break;

                    case "SCROLLLOCK":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SCROLL);
                        break;

                    case "SPACE":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.SPACE);
                        break;

                    case "TAB":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.TAB);
                        break;

                    case "ENTER":
                        CheckDefaultSleep();
                        InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
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
                        CheckDefaultSleep();
                        code = GetKeyCode(command);
                        InputSimulator.SimulateKeyPress(code);
                        break;

                    case "REPLAY":
                        Calculate(lastCommand + " " + lastKey);
                        break;

                    case "VAR":
                        // Handle variable assignment: VAR $NAME = VALUE
                        if (keys.Contains("="))
                        {
                            string[] varParts = keys.Split(new[] { '=' }, 2);
                            if (varParts.Length == 2)
                            {
                                string varName = varParts[0].Trim();
                                string varValue = varParts[1].Trim();
                                
                                // Remove $ prefix for storage
                                if (varName.StartsWith("$"))
                                {
                                    varName = varName.Substring(1);
                                }
                                
                                // Try to parse as boolean first
                                if (varValue.ToUpper() == "TRUE")
                                {
                                    variables[varName] = true;
                                }
                                else if (varValue.ToUpper() == "FALSE")
                                {
                                    variables[varName] = false;
                                }
                                // Try to parse as integer
                                else if (int.TryParse(varValue, out int intValue))
                                {
                                    variables[varName] = intValue;
                                }
                                else
                                {
                                    // Store as string
                                    variables[varName] = varValue;
                                }
                            }
                        }
                        break;

                    case "DEFINE":
                        // Handle constant definition: DEFINE #CONSTANT value
                        string[] defineParts = keys.Split(new[] { ' ' }, 2);
                        if (defineParts.Length == 2)
                        {
                            string constName = defineParts[0].Trim();
                            string constValue = defineParts[1].Trim();
                            
                            // Remove # prefix if present
                            if (constName.StartsWith("#"))
                            {
                                constName = constName.Substring(1);
                            }
                            
                            constants[constName] = constValue;
                        }
                        break;

                    case "REM":
                        // Comment - do nothing
                        break;

                    default:
                        // Single character or unknown command
                        if (command.Length == 1)
                        {
                            CheckDefaultSleep();
                            InputSimulator.SimulateTextEntry(command);
                        }
                        break;
                }

                setLastCommand(command, keys);
            }
            catch (Exception)
            {
                // Handle any exceptions during keyboard simulation
                setLastCommand(command, keys);
            }
        }

        private VirtualKeyCode GetKeyCode(string key)
        {
            switch (key.ToUpper())
            {
                case "F1": return VirtualKeyCode.F1;
                case "F2": return VirtualKeyCode.F2;
                case "F3": return VirtualKeyCode.F3;
                case "F4": return VirtualKeyCode.F4;
                case "F5": return VirtualKeyCode.F5;
                case "F6": return VirtualKeyCode.F6;
                case "F7": return VirtualKeyCode.F7;
                case "F8": return VirtualKeyCode.F8;
                case "F9": return VirtualKeyCode.F9;
                case "F10": return VirtualKeyCode.F10;
                case "F11": return VirtualKeyCode.F11;
                case "F12": return VirtualKeyCode.F12;
                case "A": return VirtualKeyCode.VK_A;
                case "B": return VirtualKeyCode.VK_B;
                case "C": return VirtualKeyCode.VK_C;
                case "D": return VirtualKeyCode.VK_D;
                case "E": return VirtualKeyCode.VK_E;
                case "F": return VirtualKeyCode.VK_F;
                case "G": return VirtualKeyCode.VK_G;
                case "H": return VirtualKeyCode.VK_H;
                case "I": return VirtualKeyCode.VK_I;
                case "J": return VirtualKeyCode.VK_J;
                case "K": return VirtualKeyCode.VK_K;
                case "L": return VirtualKeyCode.VK_L;
                case "M": return VirtualKeyCode.VK_M;
                case "N": return VirtualKeyCode.VK_N;
                case "O": return VirtualKeyCode.VK_O;
                case "P": return VirtualKeyCode.VK_P;
                case "Q": return VirtualKeyCode.VK_Q;
                case "R": return VirtualKeyCode.VK_R;
                case "S": return VirtualKeyCode.VK_S;
                case "T": return VirtualKeyCode.VK_T;
                case "U": return VirtualKeyCode.VK_U;
                case "V": return VirtualKeyCode.VK_V;
                case "W": return VirtualKeyCode.VK_W;
                case "X": return VirtualKeyCode.VK_X;
                case "Y": return VirtualKeyCode.VK_Y;
                case "Z": return VirtualKeyCode.VK_Z;
                case "0": return VirtualKeyCode.VK_0;
                case "1": return VirtualKeyCode.VK_1;
                case "2": return VirtualKeyCode.VK_2;
                case "3": return VirtualKeyCode.VK_3;
                case "4": return VirtualKeyCode.VK_4;
                case "5": return VirtualKeyCode.VK_5;
                case "6": return VirtualKeyCode.VK_6;
                case "7": return VirtualKeyCode.VK_7;
                case "8": return VirtualKeyCode.VK_8;
                case "9": return VirtualKeyCode.VK_9;
                case "DELETE": return VirtualKeyCode.DELETE;
                case "HOME": return VirtualKeyCode.HOME;
                case "INSERT": return VirtualKeyCode.INSERT;
                case "PAGEUP": return VirtualKeyCode.PRIOR;
                case "PAGEDOWN": return VirtualKeyCode.NEXT;
                case "UPARROW": return VirtualKeyCode.UP;
                case "DOWNARROW": return VirtualKeyCode.DOWN;
                case "LEFTARROW": return VirtualKeyCode.LEFT;
                case "RIGHTARROW": return VirtualKeyCode.RIGHT;
                case "TAB": return VirtualKeyCode.TAB;
                case "END": return VirtualKeyCode.END;
                case "ESC":
                case "ESCAPE": return VirtualKeyCode.ESCAPE;
                case "SPACE": return VirtualKeyCode.SPACE;
                case "BREAK":
                case "PAUSE": return VirtualKeyCode.PAUSE;
                default: 
                    // Try to convert single character
                    if (key.Length == 1)
                    {
                        char c = key.ToUpper()[0];
                        if (c >= 'A' && c <= 'Z')
                        {
                            return (VirtualKeyCode)((int)VirtualKeyCode.VK_A + (c - 'A'));
                        }
                        else if (c >= '0' && c <= '9')
                        {
                            return (VirtualKeyCode)((int)VirtualKeyCode.VK_0 + (c - '0'));
                        }
                    }
                    return VirtualKeyCode.SPACE; // Fallback
            }
        }
    }
} 