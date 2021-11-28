using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _14_TurnOn_LED_using_Button
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IDigitalInputPort inputPin12;
        IDigitalOutputPort outputPort13;

        public MeadowApp()
        {
            Initialize();
            loop();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            outputPort13 = Device.CreateDigitalOutputPort(Device.Pins.D13, initialState: false);
            inputPin12 = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeRising, ResistorMode.InternalPullDown, debounceDuration: 500);
        }

        void loop()
        {
            while (true)
            {
                outputPort13.State = inputPin12.State;
                Thread.Sleep(100);
            }
        }

    }
}
