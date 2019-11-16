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
        // Info
        // --------------------------------------------------
        // Text
        private string _Info_Text = string.Empty;
        public string Info_Text
        {
            get { return _Info_Text; }
            set
            {
                if (_Info_Text == value)
                {
                    return;
                }

                _Info_Text = value;
                OnPropertyChanged("Info_Text");
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
        public double _Arabic_FontSize = 22;
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
        public double _Hebrew_FontSize = 22 * 1.2;
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
