using Meadow;
using Meadow.Devices;
using Meadow.Gateway.WiFi;
using Meadow.Foundation.Sensors.Motion;
using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;

namespace _24_MPU6050_HTTP_API
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Mpu6050 motion_sensor;
        public MeadowApp()
        {
            Initialize().Wait();

            do
            {
                var conditions = motion_sensor.Read();
                var payload = new SensorData
                {
                    name = "Sensor Node 001",
                    device_no = "150001",
                    acceleration = new Space3D
                    {
                        X = (double)conditions.Result.Acceleration3D?.X.MetersPerSecondSquared,
                        Y = (double)conditions.Result.Acceleration3D?.Y.MetersPerSecondSquared,
                        Z = (double)conditions.Result.Acceleration3D?.Z.MetersPerSecondSquared,
                    },
                    angular_velocity = new Space3D
                    {
                        X = (double)conditions.Result.AngularVelocity3D?.X.DegreesPerSecond,
                        Y = (double)conditions.Result.AngularVelocity3D?.Y.DegreesPerSecond,
                        Z = (double)conditions.Result.AngularVelocity3D?.Z.DegreesPerSecond,
                    },
                    temperature = (double)conditions.Result.Temperature?.Celsius
                };
                var stringPayload = JsonConvert.SerializeObject(payload);
                HttpPost("http://192.168.0.126:5000/api/v1/mpu6050/post", stringPayload).Wait();
                Thread.Sleep(5000);
            }
            while (true);

            Console.WriteLine("Done.");
        }

        async Task Initialize()
        {
            Console.WriteLine("Initialize hardware...");
            // configure our sensor on the I2C Bus
            motion_sensor = new Mpu6050(Device.CreateI2cBus());

            motion_sensor.Updated += (s, result) =>
            {
                Console.WriteLine($"\nAccel : [X: {result.New.Acceleration3D?.X.MetersPerSecondSquared:N2}, " +
                                          $"[Y: {result.New.Acceleration3D?.Y.MetersPerSecondSquared:N2}, " +
                                          $"[Z: {result.New.Acceleration3D?.Z.MetersPerSecondSquared:N2}]");

                Console.WriteLine($"Angular Velocity : [X: {result.New.AngularVelocity3D?.X.DegreesPerSecond:N2}, " +
                                          $"[Y: {result.New.AngularVelocity3D?.Y.DegreesPerSecond:N2}, " +
                                          $"[Z: {result.New.AngularVelocity3D?.Z.DegreesPerSecond:N2}]");

                Console.WriteLine($"Temp: {result.New.Temperature?.Celsius:N2}C\n");
            };

            // start updating
            motion_sensor.StartUpdating(TimeSpan.FromMilliseconds(6000));

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
            public Space3D acceleration { get; set; }
            public Space3D angular_velocity { get; set; }
            public double temperature { get; set; }
        }

        public class Space3D
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
        }

    }
}