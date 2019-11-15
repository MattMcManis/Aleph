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
        public static string Hebrew(BigInteger n)
        {
            // Result
            string result = string.Empty;

            // Count digits in BigInteger Number
            BigInteger digits = n.ToString().Length;

            // Digit in the Number by Place (ones, tens, hundreds, thousands)
            // e.g. 1234
            //       ^^ (3 is in the tens place)
            //       ^  (2 is in the hundreds place)
            BigInteger nPlaceDigit = 0;

            // Hebrew Place multiplier
            BigInteger multiplier = 1;

            // Loop Counter
            BigInteger loopCount = 0;

            // -------------------------
            // Loop through each digit
            // Each pass increase the number place multiplier by x10
            // e.g. 1234
            //      ^^^^ pass 1 is x1 (ones)
            //      ^^^  pass 2 is x10 (tens)
            //      ^^   pass 3 is x100 (hundreds)
            //      ^    pass 4 is x1000 (thousands, reset, geresh ׳)
            // -------------------------
            for (BigInteger i = 0; i < digits; i++)
            {
                loopCount++;

                // Increase the multiplier x10 each pass
                // e.g. 1 x 10 = 10 multipier
                //      10 x 10 = 100 multipier
                //      100 x 10 = 1000 multipier
                if (loopCount > 1)
                {
                    multiplier = (multiplier * 10);
                }

                // Digit in the Number by Place
                nPlaceDigit = (n / multiplier) % 10;

                // Append to front with Insert, don't use +=
                result = result.Insert(0, Convert((int)nPlaceDigit, loopCount));

                // -------------------------
                // Convert the next 3 digits
                // -------------------------
                if (loopCount > 3)
                {
                    // Reset the loop counter
                    loopCount = 1;

                    // Insert Geresh (Apostrophe)
                    result = result.Insert(0, "׳");

                    // Run the Convert again
                    result = result.Insert(0, Convert((int)nPlaceDigit, loopCount));
                }
            }

            // -------------------------
            // Insert quotation mark before last character
            // -------------------------
            if (n > 10 &&  // Must be Greater than 10
                n != 20 && // Is not a multiple of 10 (20-100)
                n != 30 &&
                n != 40 &&
                n != 50 &&
                n != 60 &&
                n != 70 &&
                n != 80 &&
                n != 90 &&
                n != 100 &&
                n != 200 &&
                n != 300 &&
                n != 400 &&
                !result.EndsWith("׳") // Hebrew result must not end with Geresh ׳ for thousand 000
                )
            {
                try
                {
                    result = result.Insert(result.Length - 1, "\"");
                }
                catch
                {

                }
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
        /// Convert Digits
        /// </summary>
        public static String Convert(int nPlaceDigit, BigInteger loopCount)
        {
            // One
            // 1
            if (loopCount == 1)
            {
                return Ones(nPlaceDigit);
            }
            // Ten
            // 10
            else if (loopCount == 2)
            {
                return Tens(nPlaceDigit);
            }
            // Hundred
            // 100
            else if (loopCount == 3)
            {
                return Hundreds(nPlaceDigit);
            }
            // None
            // (Loop Count 4+)
            else
            {
                return string.Empty;
            }
        }


        /// <summary>
        /// Ones
        /// </summary>
        public static String Ones(int npd)
        {
            switch (npd)
            {
                case 1:
                    return "א";
                case 2:
                    return "ב";
                case 3:
                    return "ג";
                case 4:
                    return "ד";
                case 5:
                    return "ה";
                case 6:
                    return "ו";
                case 7:
                    return "ז";
                case 8:
                    return "ח";
                case 9:
                    return "ט";
                case 0:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }


        /// <summary>
        /// Tens
        /// </summary>
        public static String Tens(int npd)
        {
            switch (npd)
            {
                case 1:
                    return "י";
                case 2:
                    return "כ";
                case 3:
                    return "ל";
                case 4:
                    return "מ";
                case 5:
                    return "נ";
                case 6:
                    return "ס";
                case 7:
                    return "ע";
                case 8:
                    return "פ";
                case 9:
                    return "צ";
                case 0:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }


        /// <summary>
        /// Hundreds
        /// </summary>
        public static String Hundreds(int npd)
        {
            switch (npd)
            {
                case 1:
                    return "ק";
                case 2:
                    return "ר";
                case 3:
                    return "ש";
                case 4:
                    return "ת";
                case 5:
                    return "תק";
                case 6:
                    return "תר";
                case 7:
                    return "תש";
                case 8:
                    return "תת";
                case 9:
                    return "תתק";
                case 0:
                    return string.Empty;
                default:
                    return string.Empty;
            }
        }


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
