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

            var nullOrEmptyResult = ValidateInput.IsNullOrEmpty(inputTextBox.Text, "The field 'Input Website/DNS to ping:' cannot be null or emptly, please enter a vaild site or ip to continue.");
            if (nullOrEmptyResult)
            {
                return;
            }

            await StartPingingProcess();
            await StopPingingProcess();
        }

        private void saveSessionButton_Click(object sender, EventArgs e)
        {
            if (ContinuePingOperation)
            {
                MessageBox.Show(
                    @"Cannot save session while the ping operation is in progress, Please stop for the ping operation");
            }
            if (string.IsNullOrEmpty(this.outputTextBox.Text))
            {
                MessageBox.Show(@"Nothing to export, please run the Ping first");
                return;
            }

            var path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(path);
            if (directoryName == null)
            {
                MessageBox.Show(@"The working directory was null cannot continue.");
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
                MessageBox.Show($"The write operation failed; aborting save.");
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
            OutputValue(resultSet);
        }

        private async Task StopPingingProcess()
        {
            var resultSet = new ResultSet()
            {
                DisplayColor = Color.Pink,
                IsSuccessful = true,
                StringValue = "Ping Operation Complete!"
            };

            await Task.Run(() => OutputValue(resultSet));
        }

        private async Task StartPingingProcess()
        {
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
                var isNumericResult = ValidateInput.IsNumeric(autoReconnectFailureCountInputTextBox.Text, "Only integers values are allowed in the field 'Auto Reconnect Failure Count'; " +
                                                                                                          "please re-enter the value to continue or leave it blank and a default " +
                                                                                                          "value of 4 would be used.");
                if (!isNumericResult)
                {
                    return true;
                }
                FailureCountForAutoRetry = int.Parse(autoReconnectFailureCountInputTextBox.Text);
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
            OutputValue(resultSet);

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
            OutputValue(resultSet);
            WifiConnectionClient.DisconnectWifi();

            resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = "Getting a list of access points."
            };
            OutputValue(resultSet);
            var accessPoint = WifiConnectionClient.GetAccessPoints().First(x => string.Equals(x.Name, AccessPointName, StringComparison.InvariantCultureIgnoreCase));

            resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = $"Trying to reconnect to access point: {AccessPointName}"
            };
            WifiConnectionClient.ReConnectWifi(accessPoint);
            OutputValue(resultSet);

            resultSet = new ResultSet()
            {
                DisplayColor = Color.AliceBlue,
                IsSuccessful = true,
                StringValue = $"Reconnection to {AccessPointName} complete, please try ping operation again."
            };
            OutputValue(resultSet);
        }

        private void CarryOutPingOperation()
        {
            Thread.Sleep(SleepTimeSpan);
            var pingResult = WebClient.CarryOutPings(inputTextBox.Text);
            if (pingResult.NonRecoverableException)
            {
                ContinuePingOperation = false;
            }
            if (!pingResult.IsSuccessful)
            {
                _failureQueue.Add(pingResult.IsSuccessful);
            }
            else
            {
                _failureQueue.Clear();
            }
            OutputValue(pingResult);
        }

        private void OutputValue(ResultSet resultSet)
        {
            if (this.outputTextBox.InvokeRequired)
            {
                this.outputTextBox.Invoke(new Action<ResultSet>(OutputValue), resultSet);
            }
            else
            {
                outputTextBox.SelectionBackColor = resultSet.DisplayColor;
                outputTextBox.AppendText($"{DateTime.Now.ToString("yy-mm-dd:hh:MM:ss")} --> {resultSet.StringValue};\n");
                outputTextBox.SelectionStart = outputTextBox.Text.Length;
                outputTextBox.ScrollToCaret();
            }
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
    }
}
