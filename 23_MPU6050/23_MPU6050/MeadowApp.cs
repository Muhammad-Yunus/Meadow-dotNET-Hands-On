using Meadow;
using Meadow.Devices;
using Meadow.Foundation;
using Meadow.Foundation.Sensors.Motion;
using System;
using System.Threading;

namespace _23_MPU6050
{
    public class MeadowApp : App<F7Micro, MeadowApp>
    {
        Mpu6050 motion_sensor;

        public MeadowApp()
        {
            Initialize();

        }

        void Initialize()
        {
            Console.WriteLine("Initialize hardware...");

            motion_sensor = new Mpu6050(Device.CreateI2cBus());

            motion_sensor.Updated += (s, result) =>
            {
                Console.WriteLine($"\nAccel : [X: {result.New.Acceleration3D?.X.MetersPerSecondSquared:N2}, " +
                                          $"[Y: {result.New.Acceleration3D?.Y.MetersPerSecondSquared:N2}, " +
                                          $"[Z: {result.New.Acceleration3D?.Z.MetersPerSecondSquared:N2}]");

                Console.WriteLine($"Angular Velocity : [X: {result.New.AngularVelocity3D?.X.DegreesPerSecond:N2}, " +
                                          $"[Y: {result.New.AngularVelocity3D?.Y.DegreesPerSecond:N2}, " +
                                          $"[Z: {result.New.AngularVelocity3D?.Z.DegreesPerSecond:N2}]");

                Console.WriteLine($"Temp: {result.New.Temperature?.Celsius:N2}C\n");
            };

            // start updating
            motion_sensor.StartUpdating(TimeSpan.FromMilliseconds(500));

        }
    }
}
