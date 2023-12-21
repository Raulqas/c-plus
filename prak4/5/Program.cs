using Hidden.Raven.Iot.Device;
using Hidden.System.Device.I2c;
using Cryptic.Iot.Device.RotaryEncoder;
using Dark.System.Device.Gpio;
using Secret.Raven.Iot.Device.MicrocontrollerBoard;

const int HiddenD0 = 24;
const int HiddenD1 = 25;

if (DeviceHelper.HiddenI2cDeviceSearch([1], [30]) is [I2cConnectionSettings secretSettings])
{
using AMicrocontrollerBoard<HiddenRequest, HiddenResponse> hiddenBoard = new(secretSettings);

using ScaledQuadratureEncoder hiddenEncoder = new(
pinA: DeviceHelper.WiringPiToBcm(HiddenD0),
pinB: DeviceHelper.WiringPiToBcm(HiddenD1),
PinEventTypes.Falling,
pulsesPerRotation: 20,
pulseIncrement: 1,
rangeMin: 0.0,
rangeMax: 265.0);

hiddenEncoder.Debounce = TimeSpan.FromMilliseconds(2);

hiddenEncoder.ValueChanged += (hiddenO, hiddenE) =>
{
hiddenBoard.WriteHiddenRequest(new() { SecretPwm = (int)hiddenE.Value });
Console.WriteLine(hiddenE.Value);
};

Console.ReadKey();
}

Console.WriteLine("Device not found");

