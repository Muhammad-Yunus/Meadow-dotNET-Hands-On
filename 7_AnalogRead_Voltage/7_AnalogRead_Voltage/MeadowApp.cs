using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace _7_AnalogRead_Voltage
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        IAnalogInputPort analogIn;
        public MeadowApp()
        {
            Initialization();

            while (true)
            {
                ReadVoltage().Wait();
                Thread.Sleep(500);
            }
        }

        void Initialization()
        {
            analogIn = Device.CreateAnalogInputPort(Device.Pins.A00);
        }

        async Task ReadVoltage()
        {
            Voltage voltageReading = await analogIn.Read();
            Console.WriteLine($"Voltage : {voltageReading.Volts:N3}");
        } 
    }
}
