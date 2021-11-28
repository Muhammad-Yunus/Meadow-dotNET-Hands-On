using Meadow;
using Meadow.Devices;
using Meadow.Units;
using Meadow.Foundation.Sensors.Temperature;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _4_AnalogRead_LM35
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        AnalogTemperature analogTemperature;

        public MeadowApp()
        {
            analogTemperature = new AnalogTemperature(
                Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35
               );
            while (true)
            {
                ReadTemp().Wait();
                Thread.Sleep(3000);
            }
        }

        async Task ReadTemp()
        {
            Temperature temperature = await analogTemperature.Read();
            Console.WriteLine($"Temperature : {temperature.Celsius:N2}C");
        }

    }
}
