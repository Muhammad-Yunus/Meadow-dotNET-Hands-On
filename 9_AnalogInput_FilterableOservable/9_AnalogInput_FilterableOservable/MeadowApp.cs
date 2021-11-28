using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;

namespace _9_AnalogInput_FilterableOservable
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IAnalogInputPort analogIn;
        public MeadowApp()
        {
            Initialize();
        }

        void Initialize()
        {
            analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);

            var consumer = IAnalogInputPort.CreateObserver(
                handler: result =>
                {
                    Console.WriteLine($"Analog observer triggered; new {result.New.Volts:N3}V, old : {result.Old?.Volts:N3}V");
                },
                filter: result =>
                {
                    if (result.Old is { } old)
                    {
                        return (result.New - old).Abs().Volts > 0.1;
                    }
                    else
                    {
                        return false;
                    }
                }
            );

            analogIn.Subscribe(consumer);

            analogIn.StartUpdating(TimeSpan.FromMilliseconds(500));
        }
    }
}
