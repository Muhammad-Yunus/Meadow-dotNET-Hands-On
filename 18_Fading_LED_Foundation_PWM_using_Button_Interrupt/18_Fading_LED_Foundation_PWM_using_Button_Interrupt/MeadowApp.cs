using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Foundation;
using Meadow.Foundation.Leds;
using System;
using System.Threading;

namespace _18_Fading_LED_Foundation_PWM_using_Button_Interrupt
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        PwmLed pwm13;
        IDigitalInputPort digitalIn12;

        public MeadowApp()
        {
            Initialize();
            loop();

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            pwm13 = new PwmLed(Device, Device.Pins.D13, TypicalForwardVoltage.Green);

            digitalIn12 = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeRising, ResistorMode.InternalPullDown, debounceDuration: 500);

            digitalIn12.Changed += (s, result) =>
            {
                Console.WriteLine($"Interrupt occured, state : {result.New.State}, start fading LED!");
                pwm13.StartPulse(pulseDuration: 1000);
                Thread.Sleep(3000);
                pwm13.Stop();
            };
        }

        void loop()
        {
            while (true)
            {
                Console.WriteLine($"Input state : {digitalIn12.State}");
                Thread.Sleep(1000);
            }
        }
    }
}
