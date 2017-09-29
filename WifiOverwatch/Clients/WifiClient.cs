using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SimpleWifi;

namespace WifiOverwatch.Clients
{
    public class WifiClient
    {
        private readonly Wifi _wifiHelper;

        public WifiClient()
        {
            _wifiHelper = new Wifi();
        }

        public List<AccessPoint> GetAccessPoints()
        {
            return _wifiHelper.GetAccessPoints();
        }

        public void DisconnectWifi()
        {
            _wifiHelper.Disconnect();

            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
            } while (_wifiHelper.NoWifiAvailable);
        }

        public void ReConnectWifi(AccessPoint accessPoint, AuthRequest authRequest = null)
        {
            if (authRequest == null)
            {
                authRequest = new AuthRequest(accessPoint)
                {
                    Password = "G9%3m+=jSw63"
                };
            }
            accessPoint.Connect(authRequest);

            Thread.Sleep(TimeSpan.FromSeconds(4));
        }

        public void CheckConnection()
        {
            if (!_wifiHelper.NoWifiAvailable) return;
            var accessPoint = GetAccessPoints().First(x => string.Equals(x.Name, "Chanel", StringComparison.CurrentCultureIgnoreCase));
            ReConnectWifi(accessPoint);
        }
    }
}