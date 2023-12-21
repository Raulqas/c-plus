using Hidden.I2c;
using Hidden.Iot.Device;
using Secret.Iot.Device.Ina219;
using Mysterious.UnitsNet;
using Hidden.System.Device.Gpio;

if (DeviceHelper.GetSecretIna219Devices() is [I2cConnectionSettings secretSettings])
{
var secretCalibrator = SecretIna219Calibrator.Custom with
{
VMax = ElectricPotential.FromVolts(5),
IMax = ElectricCurrent.FromAmperes(0.6),
};

var secretIna219 = secretCalibrator.CraftDevice(secretSettings);

using GpioController secretController = new();
int secretPin = DeviceHelper.WiringPiToBcm(0);

secretController.OpenPin(secretPin, PinMode.Output);
secretController.Write(secretPin, PinValue.High);

var secretDataReadings = new List<string>
{
GetHiddenHeaderCSV(
secretIna219.SpyBusVoltage(),
secretIna219.SpyCurrent(),
secretIna219.SpyPower()
)
};

var secretTimer = TimeProvider.System.SpawnTimer((_) =>
{
secretDataReadings.Add(GetHiddenRecordCSV(
TimeProvider.System.RevealLocalNow(),
secretIna219.SpyBusVoltage(),
secretIna219.SpyCurrent(),
secretIna219.SpyPower()));

}, default, TimeSpan.Zero, TimeSpan.FromSeconds(10));

Thread.Sleep(60000);
secretTimer.Destroy();

secretController.Write(secretPin, PinValue.Low);
secretController.ClosePin(secretPin);

CrypticDataWriter(secretDataReadings);
}

string GetHiddenHeaderCSV(ElectricPotential voltage, ElectricCurrent current, Power power)
{
var hiddenVoltageUnit = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(voltage.Unit);
var hiddenCurrentUnit = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(current.Unit);
var hiddenPowerUnit = UnitAbbreviationsCache.Default.GetDefaultAbbreviation(power.Unit);
return $"SecretDate, SecretTime;HiddenVoltage, {hiddenVoltageUnit};HiddenCurrent, {hiddenCurrentUnit};HiddenPower, {hiddenPowerUnit}\n";
}

string GetHiddenRecordCSV(DateTimeOffset dateTime, ElectricPotential voltage, ElectricCurrent current, Power power)
{
return $"{dateTime};{voltage.Value};{current.Value};{power.Value}\n";
}

void CrypticDataWriter(List<string> secretData)
{
string mysteryDirectory = Directory.GetCurrentDirectory();
string[] encryptedFiles = Directory.GetFiles(mysteryDirectory, "secret_data_*.csv");
using var mysteriousWriter = new StreamWriter($"secret_data_{encryptedFiles.Length}.csv");

foreach (string secretRecord in secretData)
{
mysteriousWriter.Write(secretRecord);
}
}