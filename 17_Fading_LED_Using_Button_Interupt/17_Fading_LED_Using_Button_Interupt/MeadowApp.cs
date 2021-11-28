using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _17_Fading_LED_Using_Button_Interupt
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IPwmPort pwm13;
        IDigitalInputPort digitalIn12;

        public MeadowApp()
        {
            Initialize();
            loop();
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            pwm13 = Device.CreatePwmPort(Device.Pins.D13, frequency: 100, dutyCycle: 0.0f);
            
            pwm13.Start();

            digitalIn12 = Device.CreateDigitalInputPort(Device.Pins.D12, InterruptMode.EdgeRising, ResistorMode.InternalPullDown, debounceDuration: 500);

            digitalIn12.Changed += (s, result) =>
            {
                if (result.New.State == true)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        for (var j = 0; j < 100; j++)
                        {
                            pwm13.DutyCycle = (float)(j / 100.0);
                            Thread.Sleep(10);
                        }
                        for (var j = 100; j >= 0; j--)
                        {
                            pwm13.DutyCycle = (float)(j / 100.0);
                            Thread.Sleep(10);
                        }
                    }
                }
            };

        }

        void loop()
        {
            while (true)
            {
                Console.WriteLine($"Current Duty Cycle : {pwm13.DutyCycle}");
                Thread.Sleep(1000);
            }
        }


    }
}
