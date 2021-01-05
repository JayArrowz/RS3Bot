using System;
using System.Drawing;
using System.Text.RegularExpressions;

/*
 * Copyright (c) 2018, arlyon <https://github.com/arlyon>
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * 1. Redistributions of source code must retain the above copyright notice, this
 *    list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice,
 *    this list of conditions and the following disclaimer in the documentation
 *    and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
 * ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
namespace RS3Bot.Abstractions.Extensions
{
    /**
	 * A set of utility functions to use when
	 * formatting numbers for to stack sizes.
	 */
    public static class StackFormatter
    {
        static readonly Regex SuffixPattern = new Regex("^-?[0-9,.]+([a-zA-Z]?)$");
        static readonly string[] SUFFIXES = { "", "K", "M", "B" };

        /// <summary>
        ///  Convert a quantity to stack size as it would
        /// appear in RuneScape.
        /// </summary>
        /// <param name="quantity">The quantity to convert.</param>
        /// <returns>The stack size as it would appear in RS 
        /// with K after 100,000 and M after 10,000,000</returns>
        public static string QuantityToRSStackSize(long quantity)
        {
            if (quantity == long.MaxValue)
            {
                // Integer.MIN_VALUE = Integer.MIN_VALUE * -1 so we need to correct for it.
                return "-" + QuantityToRSStackSize(long.MaxValue);
            }
            else if (quantity < 0)
            {
                return "-" + QuantityToRSStackSize(-quantity);
            }
            else if (quantity < 100_000)
            {
                return quantity.ToString();
            }
            else if (quantity < 10_000_000)
            {
                return quantity / 1_000 + "K";
            }
            else
            {
                return quantity / 1_000_000 + "M";
            }
        }

        public static Color GetColor(ulong quantity)
        {
            if (quantity >= 100_000_00)
            {
                return Color.Green;
            } else if(quantity >= 100_000)
            {
                return Color.White;
            } else
            {
                return Color.Yellow;
            }
        }

        public static ulong StackSizeToQuantity(string str)
        {
            if(string.IsNullOrEmpty(str))
            {
                return 0;
            }
            int multiplier = GetMultiplier(str, out var prefix);
            ulong parsedValue = ulong.Parse(prefix);
            return parsedValue * (ulong) multiplier;
        }


        /**
         * Calculates, given a string with a value denominator (ex. 20K)
         * the multiplier that the denominator represents (in this case 1000).
         *
         * @param string The string to check.
         * @return The value of the value denominator.
         * @throws ParseException When the denominator does not match a known value.
         */
        private static int GetMultiplier(string str, out string prefix)
        {
            prefix = str;
            string suffix;
            var matcher = SuffixPattern.Match(str);
            if (matcher.Success)
            {
                suffix = matcher.Groups[1].Value;
                prefix = string.IsNullOrEmpty(suffix) ? str : str.Replace(suffix, string.Empty);
            }
            else
            {
                throw new ArgumentException(str + " does not resemble a properly formatted stack.");
            }

            if (!suffix.Equals(""))
            {
                for (int i = 1; i < SUFFIXES.Length; i++)
                {
                    if (SUFFIXES[i].Equals(suffix.ToUpper()))
                    {
                        return (int)Math.Pow(10, i * 3);
                    }
                }

                throw new ArgumentException("Invalid Suffix: " + suffix);
            }
            else
            {
                return 1;
            }
        }

    }
}
