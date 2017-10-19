using System;
using System.Windows.Forms;

namespace WifiOverwatch.Clients
{
    public class FormClient
    {
        public static void OutputValue(ResultSet resultSet, RichTextBox outputTextBox)
        {
            if (outputTextBox.InvokeRequired)
            {
                outputTextBox.Invoke(new Action<ResultSet, RichTextBox>(OutputValue), resultSet, outputTextBox);
            }
            else
            {
                outputTextBox.SelectionBackColor = resultSet.DisplayColor;
                outputTextBox.AppendText($"{DateTime.Now.ToString("yy-mm-dd:hh:MM:ss")} --> {resultSet.StringValue};\n");
                outputTextBox.SelectionStart = outputTextBox.Text.Length;
                outputTextBox.ScrollToCaret();
            }
        }
    }
}