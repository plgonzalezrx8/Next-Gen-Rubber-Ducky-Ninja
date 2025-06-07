using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;
using WinRT.Interop;
using Windows.Storage;
using System.Diagnostics;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Media;

namespace Next_Gen_Rubber_Ducky_Ninja
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private string currentFilePath = "";
        private DuckyScriptProcessing duckyProcessor = new DuckyScriptProcessing();
        private bool duckEncodeFound = false;

        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = "🥷 The Rubber Ducky Ninja - Next Gen";
            
            // Set window size for horizontal layout
            this.AppWindow.Resize(new Windows.Graphics.SizeInt32(1000, 700));
            
            // Initialize the application
            InitializeApplication();
        }

        private void InitializeApplication()
        {
            // Check for Java and DuckEncode
            CheckForJavaAndDuckEncode();
            
            // Show welcome message
            ShowNotification("🎯 Welcome!", 
                "The Rubber Ducky Ninja - Next Generation is ready. Load a DuckyScript file to begin.", 
                InfoBarSeverity.Informational);
        }

        private void CheckForJavaAndDuckEncode()
        {
            // Check if duckencode.jar exists
            if (File.Exists("duckencode.jar"))
            {
                duckEncodeFound = true;
            }
            else
            {
                ShowNotification("⚠️ Encoder Not Found", 
                    "duckencode.jar not found. Binary encoding feature will be disabled.", 
                    InfoBarSeverity.Warning);
            }

            // Could also check for Java installation here
            // For now, we'll assume it's available if duckencode.jar exists
        }

        private async void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            
            // Make it work in WinUI 3 (get window handle)
            var hwnd = WindowNative.GetWindowHandle(this);
            InitializeWithWindow.Initialize(picker, hwnd);
            
            picker.ViewMode = PickerViewMode.List;
            picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            picker.FileTypeFilter.Add(".txt");
            picker.FileTypeFilter.Add("*"); // Allow all files
            picker.CommitButtonText = "Load DuckyScript";

            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                currentFilePath = file.Path;
                FilePathDisplay.Text = $"📄 {file.Name}";
                
                // Enable validation and configuration
                ValidateButton.IsEnabled = true;
                SetDelayButton.IsEnabled = true;
                EditButton.IsEnabled = true;
                
                // Update button text to show success
                var originalContent = LoadFileButton.Content;
                LoadFileButton.Content = "✅ File Loaded";
                
                // Reset button text after 2 seconds
                var timer = DispatcherQueue.CreateTimer();
                timer.Interval = TimeSpan.FromSeconds(2);
                timer.Tick += (s, args) =>
                {
                    LoadFileButton.Content = originalContent;
                    timer.Stop();
                };
                timer.Start();
                
                ShowNotification("✅ File Loaded", 
                    $"Successfully loaded: {file.Name}", 
                    InfoBarSeverity.Success);
            }
        }

        private void SetDelayButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int delay = (int)DelayNumberBox.Value;
                duckyProcessor.SetDelay(delay);
                
                ShowNotification("⏱️ Delay Set", 
                    $"Delay between commands set to {delay}ms", 
                    InfoBarSeverity.Informational);
            }
            catch (Exception)
            {
                ShowNotification("❌ Invalid Delay", 
                    "Please enter a valid number between 0-10000", 
                    InfoBarSeverity.Error);
            }
        }

        private async void ValidateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            // Visual feedback
            var originalContent = ValidateButton.Content;
            ValidateButton.Content = "🔄 Validating...";
            ValidateButton.IsEnabled = false;

            try
            {
                bool isValid = false;
                string errorMessage = "";

                await Task.Run(() =>
                {
                    try
                    {
                        isValid = duckyProcessor.validateCode(currentFilePath);
                    }
                    catch (Validation.ValidationException ex)
                    {
                        errorMessage = $"Line {ex.LineNumber}: {ex.Message}";
                        isValid = false;
                    }
                    catch (Exception ex)
                    {
                        errorMessage = $"Validation error: {ex.Message}";
                        isValid = false;
                    }
                });
                
                // Update UI on main thread
                if (isValid)
                {
                    ExecuteButton.IsEnabled = true;
                    if (duckEncodeFound)
                    {
                        EncodeButton.IsEnabled = true;
                    }
                    
                    ShowNotification("✅ Validation Success", 
                        "No problems found in the script! You can now execute it.", 
                        InfoBarSeverity.Success);
                }
                else
                {
                    string message = string.IsNullOrEmpty(errorMessage) 
                        ? "Please check your script for errors." 
                        : errorMessage;
                        
                    ShowNotification("❌ Validation Failed", 
                        message, 
                        InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("❌ Validation Error", 
                    $"Error during validation: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
            finally
            {
                ValidateButton.Content = originalContent;
                ValidateButton.IsEnabled = true;
            }
        }

        private async void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            // Warning dialog
            var dialog = new ContentDialog()
            {
                Title = "⚠️ Execute Script",
                Content = "This will simulate keyboard input on your system. Make sure you're ready!\n\n" +
                         "• The script will start in 3 seconds after clicking Execute\n" +
                         "• Press WIN+D to minimize all windows first\n" +
                         "• Make sure target application is ready\n\n" +
                         "Are you ready to execute?",
                PrimaryButtonText = "Execute",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();
            if (result != ContentDialogResult.Primary) return;

            // Visual feedback
            var originalContent = ExecuteButton.Content;
            ExecuteButton.IsEnabled = false;

            try
            {
                // Give user time to prepare
                for (int i = 3; i > 0; i--)
                {
                    ExecuteButton.Content = $"⏳ Starting in {i}...";
                    await Task.Delay(1000);
                }

                ExecuteButton.Content = "⏳ Running Script...";
                
                // Execute the script in background thread
                await Task.Run(async () =>
                {
                    // Minimize all windows first (like the original app)
                    WindowsInput.InputSimulator.SimulateModifiedKeyStroke(
                        WindowsInput.VirtualKeyCode.LWIN, 
                        WindowsInput.VirtualKeyCode.VK_D);
                    
                    await Task.Delay(500); // Give time for windows to minimize
                    
                    // Execute the script
                    await duckyProcessor.ReadFileAsync(currentFilePath);
                });

                ShowNotification("🎯 Execution Complete", 
                    "Script execution finished successfully!", 
                    InfoBarSeverity.Success);
            }
            catch (Exception ex)
            {
                ShowNotification("❌ Execution Error", 
                    $"Error during execution: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
            finally
            {
                ExecuteButton.Content = originalContent;
                ExecuteButton.IsEnabled = true;
            }
        }

        private async void EncodeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            if (!duckEncodeFound)
            {
                ShowNotification("❌ Encoder Not Available", 
                    "duckencode.jar not found. Please ensure Java and the encoder are installed.", 
                    InfoBarSeverity.Error);
                return;
            }

            // Visual feedback
            var originalContent = EncodeButton.Content;
            EncodeButton.Content = "🔄 Encoding...";
            EncodeButton.IsEnabled = false;

            try
            {
                string outputPath = Path.ChangeExtension(currentFilePath, ".bin");
                
                await Task.Run(() =>
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "java",
                        Arguments = $"-jar duckencode.jar -i \"{currentFilePath}\" -o \"{outputPath}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    using (var process = Process.Start(startInfo))
                    {
                        process?.WaitForExit();
                    }
                });

                if (File.Exists(outputPath))
                {
                    ShowNotification("🔧 Encoding Complete", 
                        $"Binary file created: {Path.GetFileName(outputPath)}", 
                        InfoBarSeverity.Success);
                }
                else
                {
                    ShowNotification("❌ Encoding Failed", 
                        "Could not create binary file. Check if Java is installed.", 
                        InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("❌ Encoding Error", 
                    $"Error during encoding: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
            finally
            {
                EncodeButton.Content = originalContent;
                EncodeButton.IsEnabled = true;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            try
            {
                // Try different editors in order of preference
                bool opened = false;
                
                // Try Notepad++ first
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "notepad++.exe",
                        Arguments = $"\"{currentFilePath}\"",
                        UseShellExecute = true
                    });
                    opened = true;
                }
                catch
                {
                    // Try VSCode
                    try
                    {
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "code",
                            Arguments = $"\"{currentFilePath}\"",
                            UseShellExecute = true
                        });
                        opened = true;
                    }
                    catch
                    {
                        // Fallback to Notepad
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = "notepad.exe",
                            Arguments = $"\"{currentFilePath}\"",
                            UseShellExecute = true
                        });
                        opened = true;
                    }
                }

                if (opened)
                {
                    ShowNotification("📝 Editor Opened", 
                        "Script file opened in external editor.", 
                        InfoBarSeverity.Informational);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("❌ Edit Error", 
                    $"Could not open editor: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
        }

        private void RestoreButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create system restore point using VBS (like original app)
                if (File.Exists("restore.vbs"))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cscript.exe",
                        Arguments = "restore.vbs",
                        UseShellExecute = true,
                        Verb = "runas" // Run as administrator
                    });
                    
                    ShowNotification("💾 Restore Point", 
                        "System restore point creation initiated. This may take a few moments.", 
                        InfoBarSeverity.Informational);
                }
                else
                {
                    ShowNotification("❌ Script Not Found", 
                        "restore.vbs not found. Cannot create system restore point.", 
                        InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                ShowNotification("❌ Restore Error", 
                    $"Could not create restore point: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
        }

        private async void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog()
            {
                Title = "🥷 About The Rubber Ducky Ninja",
                Content = "The Rubber Ducky Ninja - Next Generation\n" +
                         "Version 2.0.0\n\n" +
                         "A modern DuckyScript testing and validation toolkit built with WinUI 3.\n\n" +
                         "Features:\n" +
                         "• DuckyScript validation and execution\n" +
                         "• Binary encoding support\n" +
                         "• Modern Fluent Design interface\n" +
                         "• Cross-compatibility with USB Rubber Ducky\n\n" +
                         "Copyright © 2025",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void UACSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = @"C:\Windows\System32\UserAccountControlSettings.exe",
                    UseShellExecute = true
                });
                
                ShowNotification("🛡️ UAC Settings", 
                    "UAC settings opened. Lower settings may improve keyboard simulation.", 
                    InfoBarSeverity.Informational);
            }
            catch (Exception ex)
            {
                ShowNotification("❌ UAC Settings Error", 
                    $"Could not open UAC settings: {ex.Message}", 
                    InfoBarSeverity.Error);
            }
        }

        private void ShowNotification(string title, string message, InfoBarSeverity severity)
        {
            // Clear any existing notifications first (only show 1 at a time)
            NotificationArea.Children.Clear();

            // Create a floating notification InfoBar with enhanced styling
            var infoBar = new InfoBar()
            {
                Title = title,
                Message = message,
                Severity = severity,
                IsOpen = true,
                CornerRadius = new CornerRadius(16),
                BorderThickness = new Thickness(2),
                Margin = new Thickness(0),
                Opacity = 1.0, // Full opacity for better visibility
                // Enhanced visual appearance
                Padding = new Thickness(20, 16, 20, 16)
            };

            // Add enhanced styling based on severity
            switch (severity)
            {
                case InfoBarSeverity.Success:
                    infoBar.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 16, 124, 16)); // Vibrant green
                    infoBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 255, 255, 255)); // White text
                    break;
                case InfoBarSeverity.Error:
                    infoBar.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 196, 43, 28)); // Vibrant red
                    infoBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 255, 255, 255)); // White text
                    break;
                case InfoBarSeverity.Warning:
                    infoBar.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 157, 93, 0)); // Vibrant orange
                    infoBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 255, 255, 255)); // White text
                    break;
                case InfoBarSeverity.Informational:
                    infoBar.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 0, 95, 184)); // Vibrant blue
                    infoBar.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Windows.UI.Color.FromArgb(255, 255, 255, 255)); // White text
                    break;
            }

            // Add to notification area
            NotificationArea.Children.Add(infoBar);

            // Auto-close after 4 seconds for non-error messages (slightly faster)
            if (severity != InfoBarSeverity.Error)
            {
                var timer = DispatcherQueue.CreateTimer();
                timer.Interval = TimeSpan.FromSeconds(4);
                timer.Tick += (s, e) =>
                {
                    infoBar.IsOpen = false;
                    timer.Stop();
                    
                    // Remove from UI after fade animation
                    var removeTimer = DispatcherQueue.CreateTimer();
                    removeTimer.Interval = TimeSpan.FromMilliseconds(300);
                    removeTimer.Tick += (s2, e2) =>
                    {
                        NotificationArea.Children.Remove(infoBar);
                        removeTimer.Stop();
                    };
                    removeTimer.Start();
                };
                timer.Start();
            }
            else
            {
                // Error messages stay longer but still auto-close
                var timer = DispatcherQueue.CreateTimer();
                timer.Interval = TimeSpan.FromSeconds(8);
                timer.Tick += (s, e) =>
                {
                    infoBar.IsOpen = false;
                    timer.Stop();
                    
                    var removeTimer = DispatcherQueue.CreateTimer();
                    removeTimer.Interval = TimeSpan.FromMilliseconds(300);
                    removeTimer.Tick += (s2, e2) =>
                    {
                        NotificationArea.Children.Remove(infoBar);
                        removeTimer.Stop();
                    };
                    removeTimer.Start();
                };
                timer.Start();
            }
        }
    }
}
