using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace WifiOverwatch.Clients
{
    public class PingClient
    {
        public ResultSet CarryOutPings(string pingDestination)
        {
            var ping = new Ping();
            try
            {
                var result = ping.Send(pingDestination);
                return ResultBuilder(result);
            }
            catch (PingException)
            {
                MessageBox.Show(
                    $"Cannot resolve {pingDestination}, please check the internet connection before continuing.");

                var resultSet = new ResultSet()
                {
                    NonRecoverableException = true
                };

                return resultSet;
            }
        }

        private static ResultSet ResultBuilder(PingReply result)
        {
            return new ResultSet()
            {
                StringValue = $"pinging {result.Address} : {result.Status} with time {result.RoundtripTime}ms",
                IsSuccessful = result.Status == IPStatus.Success,
                DisplayColor = result.Status == IPStatus.Success ? Color.Green : Color.Red
            };
        }
    }

    public class ResultSet
    {
        public string StringValue { get; set; }
        public bool IsSuccessful { get; set; }
        public Color DisplayColor { get; set; }
        public bool NonRecoverableException { get; set; }
    }
}