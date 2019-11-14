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

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
// Disable XML Comment warnings
#pragma warning disable 1591
#pragma warning disable 1587
#pragma warning disable 1570

namespace Aleph
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private void OnPropertyChanged(string prop)
        {
            //PropertyChangedEventHandler handler = PropertyChanged;
            //handler(this, new PropertyChangedEventArgs(name));

            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

        /// <summary>
        /// Main View Model
        /// </summary>
        public MainViewModel()
        {
            LoadControlsDefaults();
        }


        /// <summary>
        /// Load Controls Defaults
        /// </summary>
        public void LoadControlsDefaults()
        {
            Filter_15_IsChecked = true;
            Filter_16_IsChecked = true;
        }


        // --------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Main
        /// </summary>
        // --------------------------------------------------------------------------------------------------------

        // -------------------------
        // Window Top
        // -------------------------
        // Value
        private double _Window_Position_Top = 0;
        public double Window_Position_Top
        {
            get { return _Window_Position_Top; }
            set
            {
                if (_Window_Position_Top == value)
                {
                    return;
                }

                _Window_Position_Top = value;
                OnPropertyChanged("Window_Position_Top");
            }
        }

        // -------------------------
        // Window Left
        // -------------------------
        private double _Window_Position_Left = 0;
        public double Window_Position_Left
        {
            get { return _Window_Position_Left; }
            set
            {
                if (_Window_Position_Left == value)
                {
                    return;
                }

                _Window_Position_Left = value;
                OnPropertyChanged("Window_Position_Left");
            }
        }

        // -------------------------
        // Window Width
        // -------------------------
        // Value
        private double _Window_Width = 0;
        public double Window_Width
        {
            get { return _Window_Width; }
            set
            {
                if (_Window_Width == value)
                {
                    return;
                }

                _Window_Width = value;
                OnPropertyChanged("Window_Width");
            }
        }

        // -------------------------
        // Window Height
        // -------------------------
        // Value
        private double _Window_Height = 0;
        public double Window_Height
        {
            get { return _Window_Height; }
            set
            {
                if (_Window_Height == value)
                {
                    return;
                }

                _Window_Height = value;
                OnPropertyChanged("Window_Height");
            }
        }

        // -------------------------
        // Window Maximized
        // -------------------------
        // Value
        private bool _Window_IsMaximized = true;
        public bool Window_IsMaximized
        {
            get { return _Window_IsMaximized; }
            set
            {
                if (_Window_IsMaximized == value)
                {
                    return;
                }

                _Window_IsMaximized = value;
                OnPropertyChanged("Window_IsMaximized");
            }
        }


        // -------------------------
        // Window State
        // -------------------------
        // Value
        private WindowState _Window_State = WindowState.Normal;
        public WindowState Window_State
        {
            get { return _Window_State; }
            set
            {
                if (_Window_State == value)
                {
                    return;
                }

                _Window_State = value;
                OnPropertyChanged("Window_State");
            }
        }


        // -------------------------
        // Window Title
        // -------------------------
        // Text
        private string _TitleVersion;
        public string TitleVersion
        {
            get { return _TitleVersion; }
            set
            {
                if (value != _TitleVersion)
                {
                    _TitleVersion = value;
                    OnPropertyChanged("TitleVersion");
                }
            }
        }


        // --------------------------------------------------
        // Arabic Numerals - TextBox
        // --------------------------------------------------
        // Text
        private string _ArabicNumerals_Text;
        public string ArabicNumerals_Text
        {
            get { return _ArabicNumerals_Text; }
            set
            {
                if (value != _ArabicNumerals_Text)
                {
                    _ArabicNumerals_Text = value;
                    OnPropertyChanged("ArabicNumerals_Text");
                }
            }
        }

        // --------------------------------------------------
        // Hebrew Numerals - TextBox
        // --------------------------------------------------
        // Text
        private string _HebrewNumerals_Text;
        public string HebrewNumerals_Text
        {
            get { return _HebrewNumerals_Text; }
            set
            {
                if (value != _HebrewNumerals_Text)
                {
                    _HebrewNumerals_Text = value;
                    OnPropertyChanged("HebrewNumerals_Text");
                }
            }
        }

        // --------------------------------------------------
        // Font Size - Arabic
        // --------------------------------------------------
        public double _Arabic_FontSize;
        public double Arabic_FontSize
        {
            get { return _Arabic_FontSize; }
            set
            {
                if (_Arabic_FontSize == value)
                {
                    return;
                }

                _Arabic_FontSize = value;
                OnPropertyChanged("Arabic_FontSize");
            }
        }

        // --------------------------------------------------
        // Font Size - Hebrew
        // --------------------------------------------------
        public double _Hebrew_FontSize;
        public double Hebrew_FontSize
        {
            get { return _Hebrew_FontSize; }
            set
            {
                if (_Hebrew_FontSize == value)
                {
                    return;
                }

                _Hebrew_FontSize = value;
                OnPropertyChanged("Hebrew_FontSize");
            }
        }

        // --------------------------------------------------
        // Filter 15 - CheckBox
        // --------------------------------------------------
        private bool _Filter_15_IsChecked;
        public bool Filter_15_IsChecked
        {
            get { return _Filter_15_IsChecked; }
            set
            {
                if (_Filter_15_IsChecked != value)
                {
                    _Filter_15_IsChecked = value;
                    OnPropertyChanged("Filter_15_IsChecked");
                }
            }
        }

        // --------------------------------------------------
        // Filter 16 - CheckBox
        // --------------------------------------------------
        private bool _Filter_16_IsChecked;
        public bool Filter_16_IsChecked
        {
            get { return _Filter_16_IsChecked; }
            set
            {
                if (_Filter_16_IsChecked != value)
                {
                    _Filter_16_IsChecked = value;
                    OnPropertyChanged("Filter_16_IsChecked");
                }
            }
        }
    }
}
