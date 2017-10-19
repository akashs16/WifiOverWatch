using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WifiOverwatch.Clients;

namespace WifiOverwatch
{
    public partial class WifiOverwatchMainForm : Form
    {
        private bool ContinuePingOperation { get; set; }
        private PingClient WebClient { get; }
        private TimeSpan SleepTimeSpan { get; }
        private readonly List<bool> _failureQueue;
        private int FailureCountForAutoRetry { get; set; }
        private WifiClient WifiConnectionClient { get; set; }
        private const string AccessPointName = "CHANEL";
        private DateTime TimeToEnd { get; set; }

        public WifiOverwatchMainForm()
        {
            InitializeComponent();
            WebClient = new PingClient();
            SleepTimeSpan = TimeSpan.FromSeconds(2);
            _failureQueue = new List<bool>();
        }

        private async void startPingButton_Click(object sender, EventArgs e)
        {
            ContinuePingOperation = true;

            var autoReconnectEnabled = autoReconnectCheckbox.Checked;

            if (ValidateAutoReconnectionValues(autoReconnectEnabled))
            {
                return;
            }

            var nullOrEmptyResult = ValidateInput.IsNullOrEmpty(inputTextBox.Text, outputTextBox, "The field 'Input Website/DNS to ping:' cannot be null or emptly, please enter a vaild site or ip to continue.");
            if (nullOrEmptyResult)
            {
                return;
            }

            await StartPingingProcess();
            await StopPingingProcess();
        }

        private void saveSessionButton_Click(object sender, EventArgs e)
        {
            var errorResultSet = new ResultSet()
            {
                DisplayColor = Color.DeepPink
            };
            if (ContinuePingOperation)
            {
                errorResultSet.StringValue = @"Cannot save session while the ping operation is in progress, Please stop for the ping operation";
                FormClient.OutputValue(errorResultSet, outputTextBox);
                return;
            }
            if (string.IsNullOrEmpty(outputTextBox.Text))
            {
                errorResultSet.StringValue = @"Nothing to export, please run the Ping first";
                FormClient.OutputValue(errorResultSet, outputTextBox);
                return;
            }

            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName == null)
            {
                errorResultSet.StringValue = @"The working directory was null cannot continue.";
                FormClient.OutputValue(errorResultSet, outputTextBox);
                return;
            }
            const string folderName = "Reports";
            var folderPath = Path.Combine(directoryName, folderName);
            var fileName = $"Report-{DateTime.Now.ToString("yy-mm-dd-hh-MM-ss")}.txt";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            if (!File.Exists(filePath))
            {
                var fileStream = File.Create(filePath);
                fileStream.Close();
            }

            try
            {
                var stringData = outputTextBox.Text.Split(';');
                File.WriteAllLines(filePath, stringData);
            }
            catch (Exception)
            {
                errorResultSet.StringValue = @"The write operation failed; aborting save.";
                FormClient.OutputValue(errorResultSet, outputTextBox);
                File.Delete(filePath);
                return;
            }

            ClearOutput();
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.Yellow,
                IsSuccessful = true,
                StringValue = $"The session data is now saved on the path :{filePath}"
            };
            FormClient.OutputValue(resultSet, outputTextBox);
        }

        private async Task StopPingingProcess()
        {
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.Pink,
                IsSuccessful = true,
                StringValue = "Ping Operation Complete!"
            };

            await Task.Run(() => FormClient.OutputValue(resultSet, outputTextBox));
        }

        private async Task StartPingingProcess()
        {
            if (automaticEndTimeCheckbox.Checked)
            {
                TimeToEnd = DateTime.Now;
                if (ValidateInput.IsNullOrEmpty(endTimeTextBox.Text, outputTextBox, "The End Time text box cannot be null or empty"))
                {
                    return;
                }
                var validationResult = ValidateInput.IsValidTimeFormat(endTimeTextBox.Text, "hh:MM", outputTextBox,
                    "The time is in an invalid format please use hh:MM");
                if (!validationResult.IsParseSuccessful)
                {
                    return;
                }

                TimeToEnd = TimeToEnd.Add(validationResult.Value);

#pragma warning disable 4014
                Task.Run(() => CountDown(TimeToEnd, validationResult.Value));
            }

            if (quickTestCheckBox.Checked)
            {
                for (var i = 0; i < 3; i++)
                {
                    await Task.Run(() => CarryOutPingOperation());
                }
            }

            else
            {
                do
                {
                    await Task.Run(() => CarryOutPingOperation());
                    if (!autoReconnectCheckbox.Checked)
                    {
                        continue;
                    }
                    await Task.Run(() => CheckAndAutoReconnect());
                } while (ContinuePingOperation);
            }
        }

        private bool ValidateAutoReconnectionValues(bool autoReconnectEnabled)
        {
            if (!autoReconnectEnabled)
            {
                return false;
            }
            WifiConnectionClient = new WifiClient();

            var validationResult = string.IsNullOrEmpty(autoReconnectFailureCountInputTextBox.Text);
            if (validationResult)
            {
                FailureCountForAutoRetry = 4;
            }
            else
            {
                var isNumericResult = ValidateInput.IsNumeric(autoReconnectFailureCountInputTextBox.Text, outputTextBox, "Only integers values are allowed in the field 'Auto Reconnect Failure Count'; " +
                                                                                                          "please re-enter the value to continue or leave it blank and a default " +
                                                                                                          "value of 4 would be used.");
                if (!isNumericResult.IsParseSuccessful)
                {
                    return true;
                }
                FailureCountForAutoRetry = isNumericResult.Value;
            }
            return false;
        }

        private async void CheckAndAutoReconnect()
        {
            if (_failureQueue.Count < FailureCountForAutoRetry)
            {
                return;
            }

            ContinuePingOperation = false;
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.Orange,
                IsSuccessful = false,
                StringValue = "Starting the wifi reconnection procedure please wait...."
            };
            FormClient.OutputValue(resultSet, outputTextBox);

            await Task.Run(() => CarryOutReconnectionProcedure());
        }

        private void CarryOutReconnectionProcedure()
        {
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = "Disconnecting from current network."
            };
            FormClient.OutputValue(resultSet, outputTextBox);
            WifiConnectionClient.DisconnectWifi();

            resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = "Getting a list of access points."
            };
            FormClient.OutputValue(resultSet, outputTextBox);
            var accessPoint = WifiConnectionClient.GetAccessPoints().First(x => string.Equals(x.Name, AccessPointName, StringComparison.InvariantCultureIgnoreCase));

            resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = $"Trying to reconnect to access point: {AccessPointName}"
            };
            WifiConnectionClient.ReConnectWifi(accessPoint);
            FormClient.OutputValue(resultSet, outputTextBox);

            resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = $"Reconnection to {AccessPointName} complete, please try ping operation again."
            };
            FormClient.OutputValue(resultSet, outputTextBox);
        }

        private void CarryOutPingOperation()
        {
            Thread.Sleep(SleepTimeSpan);
            var resultSet = WebClient.CarryOutPings(inputTextBox.Text);
            if (resultSet.NonRecoverableException)
            {
                ContinuePingOperation = false;
            }
            if (!resultSet.IsSuccessful)
            {
                _failureQueue.Add(resultSet.IsSuccessful);
            }
            else
            {
                _failureQueue.Clear();
            }
            FormClient.OutputValue(resultSet, outputTextBox);
        }

        private void stopPingButton_Click(object sender, EventArgs e)
        {
            ContinuePingOperation = false;
        }

        private async void clearButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() => ClearOutput());
        }

        private void ClearOutput()
        {
            if (outputTextBox.InvokeRequired)
            {
                outputTextBox.Invoke(new Action(ClearOutput));
            }
            else
            {
                outputTextBox.Clear();
            }
        }

        private void CountDown(DateTime timeToEnd, TimeSpan timeSpan)
        {
            if (!ContinuePingOperation)
            {
                return;
            }

            var breakups = timeSpan.TotalHours / 2;

            do
            {
                Thread.Sleep(TimeSpan.FromHours(breakups));
            } while (DateTime.Now < timeToEnd);

            ContinuePingOperation = false;

            var resultSet = new ResultSet()
            {
                DisplayColor = Color.Orange,
                StringValue = "The ping operation has been auto shutdown."
            };
            FormClient.OutputValue(resultSet, outputTextBox);
        }

        private void automaticEndTimeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.automaticEndTimeCheckbox.Checked)
            {
               return; 
            }
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.Pink,
                StringValue = @"Please enter number of hours you want this to ping operation to go for; in format hh:MM example 01:30 -> this will then run for one hour and thrity mins.",
                IsSuccessful = false,
                NonRecoverableException = true
            };

            FormClient.OutputValue(resultSet, outputTextBox);
        }
    }
}
