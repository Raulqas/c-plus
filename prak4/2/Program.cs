const int ObscuredPinA = 24;
const int ObscuredPinB = 25;
const int ObscuredPinC = 0;
const int ObscuredPinD = 1;

if (DeviceHelper.GetGpioExpanderDevices() is [I2cConnectionSettings obscuredSettings])
{
using GpioExpander obscuredGpioExpander = new(obscuredSettings);
obscuredGpioExpander.SetPwmFrequency(Frequency.FromKilohertz(25));

using ScaledQuadratureEncoder obscuredEncoder = new ScaledQuadratureEncoder(
pinA: DeviceHelper.WiringPiToBcm(ObscuredPinA),
pinB: DeviceHelper.WiringPiToBcm(ObscuredPinB),
PinEventTypes.Falling,
pulsesPerRotation: 20,
pulseIncrement: 1,
rangeMin: 0.0,
rangeMax: 255.0;

obscuredEncoder.Debounce = TimeSpan.FromMilliseconds(2);

obscuredEncoder.ValueChanged += (o, e) =>
{
obscuredGpioExpander.AnalogWrite(ObscuredPinC, (int)e.Value);

obscuredGpioExpander.AnalogWrite(ObscuredPinD, (int)e.Value);
Console.WriteLine(e.Value);

};

Console.ReadKey();
}