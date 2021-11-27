using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;

namespace _3_Pulse
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        public MeadowApp()
        {
            var pwmLed = new PwmLed(Device, Device.Pins.D13, TypicalForwardVoltage.Blue);

            // pulse tehe LED
            pwmLed.StartPulse(pulseDuration: 1000, lowBrightness:0);
        }
    }
}
