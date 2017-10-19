using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WifiOverwatch.Clients;

namespace WifiOverwatch
{
    public static class ValidateInput
    {
        public static bool IsNullOrEmpty(string input, RichTextBox outputTextBox, string message = null)
        {
            var result = string.IsNullOrEmpty(input);
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.DeepPink
            };

            if (!result)
            {
                return false;
            }

            resultSet.StringValue = string.IsNullOrEmpty(message) ? "Please enter correct input before proceeding" : message;
            FormClient.OutputValue(resultSet, outputTextBox);
            return true;
        }

        public static ParseResult<int> IsNumeric(string text, RichTextBox outputTextBox, string message = null)
        {
            int intValue;
            var result = int.TryParse(text, out intValue);
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.DeepPink
            };

            if (!result)
            {
                resultSet.StringValue = string.IsNullOrEmpty(message) ? "The text can only contain numeric values." : message;
                FormClient.OutputValue(resultSet, outputTextBox);
            }

            return new ParseResult<int>()
            {
                IsParseSuccessful = result,
                Value = intValue
            };
        }

        public static ParseResult<TimeSpan> IsValidTimeFormat(string text, string timeFormat, RichTextBox outputTextBox, string message = "Ilegal date time format")
        {
            TimeSpan value;
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.DeepPink
            };

            var format = GetFormatMatch(timeFormat);
            if (!MatchFormat(text, format))
            {
                resultSet.StringValue = message;
                FormClient.OutputValue(resultSet, outputTextBox);
                return new ParseResult<TimeSpan>()
                {
                    IsParseSuccessful = false
                };
            }
            var verdict = TimeSpan.TryParse(text, out value);

            return new ParseResult<TimeSpan>()
            {
                Value = value,
                IsParseSuccessful = verdict
            };
        }

        private static string GetFormatMatch(string inputString)
        {
            if (Regex.IsMatch(inputString, "hh:MM"))
            {
                return "[0-9][0-9]:[0-9][0-9]";
            }

            throw new Exception("No valid pattern found for the time input");
        }

        private static bool MatchFormat(string inputString, string format)
        {
            return Regex.IsMatch(inputString, format);
        }
    }

    public class ParseResult<T>
    {
        public T Value { get; set; }
        public bool IsParseSuccessful { get; set; }
    }
}