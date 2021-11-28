using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _13_DigitalInput
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {

        IDigitalInputPort digitalIn12;
        public MeadowApp()
        {
            Initialize();
            loop();
        }

        void Initialize()
        {
            digitalIn12 = Device.CreateDigitalInputPort(Device.Pins.D12, 
                                                        InterruptMode.EdgeRising, 
                                                        ResistorMode.InternalPullDown,
                                                        debounceDuration: 500);

            digitalIn12.Changed += (s, result) =>
            {
                Console.WriteLine($"Interupt occured, new state : {result.New.State}, old state {result.Old?.State}");
            };

        }

        void loop()
        {
            while (true)
            {
                Console.WriteLine($"pin 12 state : {digitalIn12.State}");
                Thread.Sleep(1000);
            }
        }

        
    }
}
