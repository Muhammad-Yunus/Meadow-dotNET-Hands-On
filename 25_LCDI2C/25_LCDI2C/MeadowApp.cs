using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Displays.Lcd;
using Meadow.Hardware;
using System;
using System.Threading;

namespace _25_LCDI2C
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        CharacterDisplay display;

        public MeadowApp()
        {
            Initialize();
            do
            {
                TestCharacterDisplay();
            }
            while (true);
        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            display = new CharacterDisplay(
                i2cBus: Device.CreateI2cBus(I2cBusSpeed.Standard),
                address: (byte)I2cCharacterDisplay.Addresses.Default,
                rows: 4, columns: 20
                );
        }

        void TestCharacterDisplay()
        {
            Console.WriteLine("TestCharacterDisplay...");

            display.WriteLine("Hello", 0);

            display.WriteLine("Display", 1);

            Thread.Sleep(1000);
            display.WriteLine("Will delete in", 0);

            int count = 5;
            while (count > 0)
            {
                display.WriteLine($"{count--}", 1);
                Thread.Sleep(500);
            }

            display.ClearLines();
            Thread.Sleep(2000);

            display.WriteLine("Cursor test", 0);

            for (int i = 0; i < display.DisplayConfig.Width; i++)
            {
                display.SetCursorPosition((byte)i, 1);
                display.Write("*");
                Thread.Sleep(100);
                display.SetCursorPosition((byte)i, 1);
                display.Write(" ");
            }

            display.ClearLines();
            display.WriteLine("Complete!", 0);
        }
    }
}
