using Meadow;
using Meadow.Devices;
using Meadow.Units;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using Meadow.Foundation.Sensors.Temperature;
using System;
using System.Threading;

namespace _10_LM35_LED_OnOff
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        AnalogTemperature analogTemperature;
        
        public MeadowApp()
        {
            Initialize();

        }
        
        void Initialize()
        {
            var led = new Led(Device, Device.Pins.D13);

            analogTemperature = new AnalogTemperature(
                Device, Device.Pins.A00, AnalogTemperature.KnownSensorType.LM35
                );

            var consumer = AnalogTemperature.CreateObserver(
                handler: result =>
                {
                    Console.WriteLine($"Temperature above threshold : {result.New.Celsius:N2}C");
                },
                filter: result =>
                {
                    if (result.New.Celsius > 31)
                    {
                        led.IsOn = true;
                        return true;
                    }
                    else
                    {
                        led.IsOn = false;
                        return false;
                    }

                }
                );
            analogTemperature.Subscribe(consumer);

            analogTemperature.TemperatureUpdated += (s, result) =>
            {
                Console.WriteLine($"Current Temperature: {result.New.Celsius:N2}C");
            };

            analogTemperature.StartUpdating(TimeSpan.FromMilliseconds(500));

        }
    }
}
