using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _16_PWM
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IPwmPort pwm13;
        public MeadowApp()
        {
            Initialize();
            loop();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            // hardware PWM
            pwm13 = Device.CreatePwmPort(Device.Pins.D13, frequency: 100, dutyCycle: 0.5f, inverted:false);

            pwm13.Start();
        }

        void loop()
        {
            while (true)
            {
                for(var i=0; i < 100; i++)
                {
                    pwm13.DutyCycle = (float)(i/100.0);
                    Console.WriteLine($"Duty Cycle : {pwm13.DutyCycle}");
                    Thread.Sleep(10);
                }
            }
        }
    }
}
