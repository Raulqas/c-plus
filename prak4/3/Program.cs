using HiddenLibrary;
using Iot.Device.OneWire;

bool flag = true;

Console.CancelKeyPress += (sender, eventArgs) =>
{
    flag = false;
};

while (flag)
{
    foreach (var dev in OneWireThermometerDevice.EnumerateDevices())
    {
        var temp = await dev.ReadTemperatureAsync( );
        Console.WriteLine(temp.DegreesCelsius);
        Task.Delay(1000).GetAwaiter().GetResult();
    }
}
