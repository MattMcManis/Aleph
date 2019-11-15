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

using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace Aleph
{
    public class Converter
    {
        /// <summary>
        /// Convert to Hebrew
        /// </summary>
        public static string Hebrew(string n)
        {
            // Result
            string result = string.Empty;

            // Split number into an array of digits
            List<BigInteger> digits = n.Reverse()
                                       .Select(x => BigInteger.Parse(x.ToString()))
                                       .ToList();

            // Hebrew Multiplier
            int multiplier = 0;

            // -------------------------
            // Loop through number array, reading each digit
            // e.g. 1234
            //      ^^^^ pass 1 is ones
            //      ^^^  pass 2 is tens
            //      ^^   pass 3 is hundreds
            //      ^    pass 4 is thousands, reset, geresh ׳
            // -------------------------
            for (int i = 0; i < digits.Count; i++)
            {
                // Append to front with Insert (don't use +=)
                result = result.Insert(0, numerals[multiplier, (int)digits[i]]);

                // Convert the thousands place digit
                if (multiplier == 3)
                {
                    // Reset the multiplier
                    multiplier = 0;

                    // Run the Convert again
                    result = result.Insert(0, numerals[multiplier, (int)digits[i]]);
                }

                // Increase the multiplier x10 each pass
                // e.g. loop 0 = 1 multipier
                //      loop 1 = 10 multipier
                //      loop 2 = 100 multipier
                //      loop 3 = 1000 multipier
                multiplier++;
            }

            // -------------------------
            // Insert quote " before last character
            // -------------------------
            if (result.Length > 1 &&  // Hebrew digit count must be greater than 1
                !result.EndsWith("׳") // Hebrew result must not end with Geresh ׳ for thousand 000
                )
            {
                result = result.Insert(result.Length - 1, "\"");
            }

            // -------------------------
            // Filter
            // -------------------------
            // Filter Number 15 if checkbox is checked
            if (VM.MainView.Filter_15_IsChecked == true)
            {
                result = Filter_15(result);
            }
            // Filter Number 16 if checkbox is checked
            if (VM.MainView.Filter_16_IsChecked == true)
            {
                result = Filter_16(result);
            }

            // -------------------------
            // Display
            // -------------------------
            return result;
        }


        /// <summary>
        /// Hebrew Numerals
        /// </summary>
        public static string[,] numerals = new string[,]
        {
            //0   1    2    3    4    5    6    7    8    9
            {"", "א", "ב", "ג", "ד", "ה", "ו", "ז", "ח", "ט", },      // x1
            {"", "י", "כ", "ל", "מ", "נ", "ס", "ע", "פ", "צ", },      // x10
            {"", "ק", "ר", "ש", "ת", "תק", "תר", "תש", "תת", "תתק" }, // x100
            {"׳", "׳", "׳", "׳", "׳", "׳", "׳", "׳", "׳", "׳" }       // x1000
        };


        /// <summary>
        /// Filter - 15
        /// </summary>
        public static String Filter_15(string input)
        {
            // If the last ones end in a 5 and the last tens end in a 10, (10+5) 15 י"ה, change to (9+6) ט"ו
            // e.g. 150,315,816,167

            // -------------------------
            // Replace regular occurances
            // -------------------------
            if (input.Contains("יה"))
            {
                input = Regex.Replace(input, "(" + "יה" + ")", "טו");
            }

            // -------------------------
            // Replace Ending with quotation mark "
            // -------------------------
            if (input.EndsWith("י\"ה"))
            {
                input = Regex.Replace(input, "(" + "י\"ה" + ")", "ט\"ו");
            }

            return input;
        }


        /// <summary>
        /// Filter - 16
        /// </summary>
        public static String Filter_16(string input)
        {
            // If the last ones end in a 6 and the last tens end in a 10, (10+6) 16 י"ו, change to (9+7) ט"ז
            // e.g. 150,315,816,167

            // -------------------------
            // Replace regular occurances
            // -------------------------
            if (input.Contains("יו"))
            {
                input = Regex.Replace(input, "(" + "יו" + ")", "טז");
            }

            // -------------------------
            // Replace Ending with quotation mark "
            // -------------------------
            if (input.EndsWith("י\"ו"))
            {
                input = Regex.Replace(input, "(" + "י\"ו" + ")", "ט\"ז");
            }

            return input;
        }

    }
}
