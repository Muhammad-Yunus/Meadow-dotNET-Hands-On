using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _8_AnalogInput_Automatic_Pooling
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

            analogIn.Updated += (s, result) =>
            {
                Console.WriteLine($"Analog event, new voltage : {result.New.Volts:N3}V, old : {result.Old?.Volts:N3}V");
            };

            analogIn.StartUpdating(TimeSpan.FromMilliseconds(500));
        }

    }
}
