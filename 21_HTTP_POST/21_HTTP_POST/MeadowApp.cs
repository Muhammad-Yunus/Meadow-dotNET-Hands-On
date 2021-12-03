using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace _21_HTTP_POST
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize().Wait();

            do
            {
                var payload = new SensorData
                {
                    name = "Sensor Node 001",
                    device_no = "150001",
                    temperature = 32.5,
                    pressure = 65
                };
                var stringPayload = JsonConvert.SerializeObject(payload);
                HttpPost("http://192.168.0.126:5000/api/v1/dummy/post", stringPayload).Wait();
                Thread.Sleep(5000);
            }
            while (true);

            Console.WriteLine("Done.");
        }

        async Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            // connected event test.
            Device.WiFiAdapter.WiFiConnected += WiFiAdapter_ConnectionCompleted;

            // connnect to the wifi network.
            Console.WriteLine($"Connecting to WiFi Network {Secrets.WIFI_NAME}");
            var connectionResult = await Device.WiFiAdapter.Connect(Secrets.WIFI_NAME, Secrets.WIFI_PASSWORD);
            if (connectionResult.ConnectionStatus != ConnectionStatus.Success)
            {
                throw new Exception($"Cannot connect to network: {connectionResult.ConnectionStatus}");
            }
        }

        private void WiFiAdapter_ConnectionCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Connection request completed.");
        }

        public async Task HttpPost(string uri, string stringPayload)
        {
            Console.WriteLine($"Requesting {uri}");

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 5, 0);

                var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, httpContent);

                try
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Request time out.");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Request went sideways: {e.Message}");
                }
            }
        }

        public class SensorData
        {
            public string name { get; set; }
            public string device_no { get; set; }
            public double temperature { get; set; }
            public double pressure { get; set; }
        }

    }
}