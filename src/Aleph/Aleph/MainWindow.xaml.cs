/* ----------------------------------------------------------------------
Aleph - Hebrew Numerals Converter
Copyright (C) 2019 Matt McManis
http://github.com/MattMcManis
mattmcmanis@outlook.com

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.If not, see <http://www.gnu.org/licenses/>. 
---------------------------------------------------------------------- */

using Aleph.Properties;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Aleph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Current Version
        public static Version currentVersion { get; set; }
        // GitHub Latest Version
        public static Version latestVersion { get; set; }
        // Alpha, Beta, Stable
        public static string currentBuildPhase = "";
        public static string latestBuildPhase { get; set; }
        public static string[] splitVersionBuildPhase { get; set; }

        // System
        public static string appRootDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + @"\"; // Aleph.exe directory

        public static string commonProgramFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFiles).TrimEnd('\\') + @"\";
        public static string commonProgramFilesX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86).TrimEnd('\\') + @"\";
        public static string programFilesDir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles).TrimEnd('\\') + @"\";
        public static string programFilesX86Dir = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86).TrimEnd('\\') + @"\";
        public static string programFilesX64Dir = @"C:\Program Files\";

        public static string programDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).TrimEnd('\\') + @"\";
        public static string appDataLocalDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).TrimEnd('\\') + @"\";
        public static string appDataRoamingDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).TrimEnd('\\') + @"\";
        public static string tempDir = Path.GetTempPath(); // Windows AppData Temp Directory

        public static string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).TrimEnd('\\') + @"\";
        public static string documentsDir = userProfile + @"Documents\"; // C:\Users\Example\Documents\
        public static string downloadDir = userProfile + @"Downloads\"; // C:\Users\Example\Downloads\

        /// <summary>
        /// Main Window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            MinWidth = 600;
            MinHeight = 338;

            // -------------------------
            // Set Current Version to Assembly Version
            // -------------------------
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string assemblyVersion = fvi.FileVersion;
            currentVersion = new Version(assemblyVersion);

            // -------------------------
            // Title + Version
            // -------------------------
            VM.MainView.TitleVersion = "Aleph ~ Hebrew Numerals Converter (" + Convert.ToString(currentVersion) + /*"-" + currentBuildPhase +*/ ")";

            // -------------------------
            // Tool Tips
            // -------------------------
            // Longer Display Time
            ToolTipService.ShowDurationProperty.OverrideMetadata(typeof(DependencyObject),
                                                                 new FrameworkPropertyMetadata(Int32.MaxValue));

            // -------------------------
            // Prevent Loading Corrupt App.Config
            // -------------------------
            try
            {
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            }
            catch (ConfigurationErrorsException ex)
            {
                string filename = ex.Filename;

                if (File.Exists(filename) == true)
                {
                    File.Delete(filename);
                    Settings.Default.Upgrade();
                }
                else
                {

                }
            }

            // -------------------------
            // Window Position
            // -------------------------
            if (Convert.ToDouble(Settings.Default["Left"]) == 0 &&
                Convert.ToDouble(Settings.Default["Top"]) == 0)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            // Load Saved
            else
            {
                this.Top = Settings.Default.Top;
                this.Left = Settings.Default.Left;
                this.Height = Settings.Default.Height;
                this.Width = Settings.Default.Width;

                if (Settings.Default.Maximized)
                {
                    WindowState = WindowState.Maximized;
                }
            }
        }

        /// <summary>
        /// Window Loaded
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = this;
        }

        /// <summary>
        ///     Closing
        /// </summary>
        void Window_Closing(object sender, CancelEventArgs e)
        {
            // Save Window Position

            if (WindowState == WindowState.Maximized)
            {
                // Use the RestoreBounds as the current values will be 0, 0 and the size of the screen
                Settings.Default.Top = RestoreBounds.Top;
                Settings.Default.Left = RestoreBounds.Left;
                Settings.Default.Height = RestoreBounds.Height;
                Settings.Default.Width = RestoreBounds.Width;
                Settings.Default.Maximized = true;
            }
            else
            {
                Settings.Default.Top = this.Top;
                Settings.Default.Left = this.Left;
                Settings.Default.Height = this.Height;
                Settings.Default.Width = this.Width;
                Settings.Default.Maximized = false;
            }

            Settings.Default.Save();

            // Exit
            e.Cancel = true;
            System.Windows.Forms.Application.ExitThread();
            Environment.Exit(0);
        }

        /// <summary>
        ///     Close / Exit (Method)
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            // Force Exit All Executables
            base.OnClosed(e);
            Application.Current.Shutdown();
        }


        // Hebrew Output
        public static string output { get; set; } // Final Hebrew Output
        //public static string outputBackup { get; set; } // Before being filtered

        /// <summary>
        /// Arabic Numerals Textbox
        /// </summary>
        private void tbxArabicNumerals_TextChanged(object sender, TextChangedEventArgs e)
        {
            ArabicNumeralsAsync();
        }


        /// <summary>
        /// Start Convert Process Async
        /// </summary>
        /// <returns></returns>
        public static async Task<int> StartConvertProcess()
        {
            int count = 0;
            await Task.Factory.StartNew(() =>
            {
                string input = FilterNonNumericCharacters(VM.MainView.ArabicNumerals_Text);

                // Convert
                if (!string.IsNullOrWhiteSpace(input) &&
                    !string.IsNullOrEmpty(input))
                {
                    // Hebrew
                    output = Converter.Hebrew(
                                BigInteger.Parse(
                                        input
                                        ));

                    VM.MainView.HebrewNumerals_Text = output;
                }
                else
                {
                    VM.MainView.HebrewNumerals_Text = string.Empty;
                }
            });

            return count;
        }

        /// <summary>
        /// Arabic Numerals Textbox (TextChanged Event)
        /// </summary>
        public async void ArabicNumeralsAsync()
        {
            Task<int> task = StartConvertProcess();
            int count = await task;
        }

        /// <summary>
        /// Hebrew Numerals Textbox
        /// </summary>
        private void tbxHebrewNumerals_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// Filter - Non-Numeric Characters
        /// </summary>
        public static String FilterNonNumericCharacters(string input)
        {
            Regex rgx = new Regex("[^0-9 -]");
            return rgx.Replace(input, "");
        }

        /// <summary>
        /// Allow Only Numbers
        /// </summary>
        public void AllowNumbersOnly(KeyEventArgs e)
        {
            // Only allow Numbers
            // Deny Symbols (Shift + Number)
            if (!(e.Key >= System.Windows.Input.Key.D0 && e.Key <= System.Windows.Input.Key.D9) ||
                e.Key == System.Windows.Input.Key.Back ||
                e.Key == System.Windows.Input.Key.Delete ||
                (Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift) && (e.Key >= System.Windows.Input.Key.D9)) ||
                (Keyboard.IsKeyDown(System.Windows.Input.Key.RightShift) && (e.Key >= System.Windows.Input.Key.D9))
                )
            {
                e.Handled = true;
            }

            // Allow Comma
            if (e.Key == System.Windows.Input.Key.OemComma)
            {
                e.Handled = false;
            }
        }
        // Deny Spaces
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Arabic Numerals - Key Down Event
        /// </summary>
        private void tbxArabicNumerals_KeyDown(object sender, KeyEventArgs e)
        {
            // Only allow Numbers and Backspace
            AllowNumbersOnly(e);
        }

        /// <summary>
        /// Info Button
        /// </summary>
        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
@"Aleph - Hebrew Numerals Converter
Copyright © 2019 Matt McManis
https://github.com/MattMcManis/Aleph
mattmcmanis@outlook.com

Fonts Noto Sans KR & Noto Serif Hebrew © Google Inc.

Noto is a trademark of Google Inc. Noto fonts are open source. 
All Noto fonts are published under the SIL Open Font License, 
Version 1.1. Language data and some sample texts are from the 
Unicode CLDR project.

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see http://www.gnu.org/licenses/.
"
);
        }

        /// <summary>
        /// Website Button
        /// </summary>
        private void btnWeb_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/MattMcManis/Aleph");
        }

        /// <summary>
        /// Copy Button
        /// </summary>
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(VM.MainView.HebrewNumerals_Text) &&
                !string.IsNullOrWhiteSpace(VM.MainView.HebrewNumerals_Text))
            {
                Clipboard.SetText(VM.MainView.HebrewNumerals_Text, TextDataFormat.UnicodeText);
            }
        }

        /// <summary>
        /// Filter 15 - CheckBox
        /// </summary>
        private void cbxFilter_15_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(VM.MainView.ArabicNumerals_Text) &&
                !string.IsNullOrEmpty(VM.MainView.ArabicNumerals_Text))
            {
                ArabicNumeralsAsync();
            }
        }
        private void cbxFilter_15_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(VM.MainView.ArabicNumerals_Text) &&
                !string.IsNullOrEmpty(VM.MainView.ArabicNumerals_Text))
            {
                ArabicNumeralsAsync();
            }
        }

        /// <summary>
        /// Filter 16 - CheckBox
        /// </summary>
        private void cbxFilter_16_Checked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(VM.MainView.ArabicNumerals_Text) &&
                !string.IsNullOrEmpty(VM.MainView.ArabicNumerals_Text))
            {
                ArabicNumeralsAsync();
            }
        }
        private void cbxFilter_16_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(VM.MainView.ArabicNumerals_Text) &&
                !string.IsNullOrEmpty(VM.MainView.ArabicNumerals_Text))
            {
                ArabicNumeralsAsync();
            }
        }

    }
}
