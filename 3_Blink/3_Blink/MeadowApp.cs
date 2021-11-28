using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;

namespace _2_Blink
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {

        public MeadowApp()
        {
            var pwmLed = new PwmLed(
                    Device,
                    Device.Pins.OnboardLedGreen,
                    TypicalForwardVoltage.Green);

            // blink the LED
            pwmLed.StartBlink(
                onDuration : 1000, 
                offDuration : 1000
                );
        }

    }
}
