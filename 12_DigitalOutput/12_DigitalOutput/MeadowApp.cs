using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _12_DigitalOutput
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            Initialize();

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            var d13 = Device.CreateDigitalOutputPort(Device.Pins.D13, initialState: false);

            while (true)
            {
                d13.State = true;
                Thread.Sleep(500);
                d13.State = false;
                Thread.Sleep(500);
            }
        }
    }
}
