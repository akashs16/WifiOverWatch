using System.Windows.Forms;

namespace WifiOverwatch
{
    public static class ValidateInput
    {
        public static bool IsNullOrEmpty(string input, string message = null)
        {
            var result = string.IsNullOrEmpty(input);

            if (result)
            {
                MessageBox.Show(string.IsNullOrEmpty(message)
                    ? "Please enter correct input before proceeding"
                    : message);
            }

            return result;
        }

        public static bool IsNumeric(string text, string message = null)
        {
            int intValue;
            var result = int.TryParse(text, out intValue);


            if (!result)
            {
                MessageBox.Show(string.IsNullOrEmpty(message)
                    ? "The text can only contain numeric values."
                    : message);
            }

            return result;
        }
    }
}