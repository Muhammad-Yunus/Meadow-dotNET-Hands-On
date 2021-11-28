using Meadow;
using Meadow.Devices;
using Meadow.Units;
using Meadow.Foundation.Sensors.Temperature;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _5_AnalogRead_LM35_Automatic_Pooling
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {

        AnalogTemperature analogTemperature;

        public MeadowApp()
        {
            analogTemperature = new AnalogTemperature(
                Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35
               );

            analogTemperature.TemperatureUpdated += (s, result) =>
            {
                Console.WriteLine($"Temp Changed, temp: {result.New.Celsius:N2}C, old: {result.Old?.Celsius:N2}C");
            };

            analogTemperature.StartUpdating(TimeSpan.FromSeconds(1));
        }

    }
}
