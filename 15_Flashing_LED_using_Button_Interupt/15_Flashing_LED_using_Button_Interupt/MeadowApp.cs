using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _15_Flashing_LED_using_Button_Interupt
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IDigitalInputPort digitalIn12;
        IDigitalOutputPort digitalOut13;
        public MeadowApp()
        {
            Initialize();
            loop();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");
            
            digitalOut13 = Device.CreateDigitalOutputPort(Device.Pins.D13, initialState: false);
            digitalIn12 = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeRising, ResistorMode.ExternalPullDown, debounceDuration: 500);

            digitalIn12.Changed += (s, result) =>
            {
                Console.WriteLine($"Interupt occured, state {result.New.State}");
                for (var i = 0; i < 20; i++) // flash led ten times.
                {
                    digitalOut13.State = true;
                    Thread.Sleep(100);
                    digitalOut13.State = false;
                    Thread.Sleep(100);
                }
            };
        }

        void loop()
        {
            while (true)
            {
                Console.WriteLine($"Input State : {digitalIn12.State}");
                Thread.Sleep(1000);
            }
        }

    }
}
