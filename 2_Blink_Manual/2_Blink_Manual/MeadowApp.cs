using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;

namespace _10_LM35_LED_OnOff
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {

        public MeadowApp()
        {
            Initialize();

        }

        void Initialize()
        {
            var led = new Led(Device, Device.Pins.D13);

            while (true)
            {
                led.IsOn = true;
                Thread.Sleep(1000);
                led.IsOn = false;
                Thread.Sleep(1000);
            }
        }
    }
}
