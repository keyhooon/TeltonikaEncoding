// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.At2000.At2000Io
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Data.At2000
{
  public enum At2000Io
  {
    Ain1 = 1,
    Ain2 = 2,
    ExternalPower = 3,
    BatteryLevel = 4,
    CurrentProfile = 5,
    Temperature = 6,
    GsmOperator = 10, // 0x0000000A
    GsmLevel = 11, // 0x0000000B
    CellId = 12, // 0x0000000C
    [BinaryIo(IoType.Io, 0)] RoamingMode = 13, // 0x0000000D
    [BinaryIo(IoType.Io, 1)] TemperatureThresholdExceeded = 14, // 0x0000000E
    [BinaryIo(IoType.Io, 2)] AccelerometerOperation = 15, // 0x0000000F
    [BinaryIo(IoType.Io, 3)] ExternalPowerConnected = 16, // 0x00000010
    [BinaryIo(IoType.Io, 4)] AnalogInput1ThresholdExceeded = 17, // 0x00000011
    [BinaryIo(IoType.Io, 5)] AnalogInput2ThresholdExceeded = 18, // 0x00000012
    [BinaryIo(IoType.Io, 6)] DigitalInput1 = 19, // 0x00000013
    [BinaryIo(IoType.Io, 7)] DigitalInput2 = 20, // 0x00000014
    [BinaryIo(IoType.Io, 9)] BatteryLevelCritical = 21, // 0x00000015
    [BinaryIo(IoType.Io, 8)] BatteryLevelLow = 22, // 0x00000016
    [BinaryIo(IoType.Io, 10)] MagneticSensorStatus = 23, // 0x00000017
    [BinaryIo(IoType.Io, 11)] CoordinateReliability = 24, // 0x00000018
    [BinaryIo(IoType.Io, 12)] DistanceThresholdExceeded = 25, // 0x00000019
    [BinaryIo(IoType.Io, 13)] CourseThresholdExceeded = 26, // 0x0000001A
    [BinaryIo(IoType.Geofencing, 0)] Geozone01 = 27, // 0x0000001B
    [BinaryIo(IoType.Geofencing, 1)] Geozone02 = 28, // 0x0000001C
    [BinaryIo(IoType.Geofencing, 2)] Geozone03 = 29, // 0x0000001D
    [BinaryIo(IoType.Geofencing, 3)] Geozone04 = 30, // 0x0000001E
    [BinaryIo(IoType.Geofencing, 4)] Geozone05 = 31, // 0x0000001F
    [BinaryIo(IoType.Geofencing, 5)] Geozone06 = 32, // 0x00000020
    [BinaryIo(IoType.Geofencing, 6)] Geozone07 = 33, // 0x00000021
    [BinaryIo(IoType.Geofencing, 7)] Geozone08 = 34, // 0x00000022
    [BinaryIo(IoType.Geofencing, 8)] Geozone09 = 35, // 0x00000023
    [BinaryIo(IoType.Geofencing, 9)] Geozone10 = 36, // 0x00000024
    [BinaryIo(IoType.Geofencing, 10)] Geozone11 = 37, // 0x00000025
    [BinaryIo(IoType.Geofencing, 11)] Geozone12 = 38, // 0x00000026
    [BinaryIo(IoType.Geofencing, 12)] Geozone13 = 39, // 0x00000027
    [BinaryIo(IoType.Geofencing, 13)] Geozone14 = 40, // 0x00000028
    [BinaryIo(IoType.Geofencing, 14)] Geozone15 = 41, // 0x00000029
    [BinaryIo(IoType.Geofencing, 15)] Geozone16 = 42, // 0x0000002A
    [BinaryIo(IoType.Geofencing, 16)] Geozone17 = 43, // 0x0000002B
    [BinaryIo(IoType.Geofencing, 17)] Geozone18 = 44, // 0x0000002C
    [BinaryIo(IoType.Geofencing, 18)] Geozone19 = 45, // 0x0000002D
    [BinaryIo(IoType.Geofencing, 19)] Geozone20 = 46, // 0x0000002E
    [BinaryIo(IoType.IoEvent, 0)] RoamingModeEvent = 47, // 0x0000002F
    [BinaryIo(IoType.IoEvent, 1)] TemperatureThresholdExceededEvent = 48, // 0x00000030
    [BinaryIo(IoType.IoEvent, 2)] AccelerometerOperationEvent = 49, // 0x00000031
    [BinaryIo(IoType.IoEvent, 3)] ExternalPowerConnectedEvent = 50, // 0x00000032
    [BinaryIo(IoType.IoEvent, 4)] AnalogInput1ThresholdExceededEvent = 51, // 0x00000033
    [BinaryIo(IoType.IoEvent, 5)] AnalogInput2ThresholdExceededEvent = 52, // 0x00000034
    [BinaryIo(IoType.IoEvent, 6)] DigitalInput1Event = 53, // 0x00000035
    [BinaryIo(IoType.IoEvent, 7)] DigitalInput2Event = 54, // 0x00000036
    [BinaryIo(IoType.IoEvent, 9)] BatteryLevelCriticalEvent = 55, // 0x00000037
    [BinaryIo(IoType.IoEvent, 8)] BatteryLevelLowEvent = 56, // 0x00000038
    [BinaryIo(IoType.IoEvent, 10)] MagneticSensorStatusEvent = 57, // 0x00000039
    [BinaryIo(IoType.IoEvent, 11)] CoordinateReliabilityEvent = 58, // 0x0000003A
    [BinaryIo(IoType.IoEvent, 12)] DistanceThresholdExceededEvent = 59, // 0x0000003B
    [BinaryIo(IoType.IoEvent, 13)] CourseThresholdExceededEvent = 60, // 0x0000003C
    [BinaryIo(IoType.GeofencingEvent, 0)] GeozoneEvent01 = 61, // 0x0000003D
    [BinaryIo(IoType.GeofencingEvent, 1)] GeozoneEvent02 = 62, // 0x0000003E
    [BinaryIo(IoType.GeofencingEvent, 2)] GeozoneEvent03 = 63, // 0x0000003F
    [BinaryIo(IoType.GeofencingEvent, 3)] GeozoneEvent04 = 64, // 0x00000040
    [BinaryIo(IoType.GeofencingEvent, 4)] GeozoneEvent05 = 65, // 0x00000041
    [BinaryIo(IoType.GeofencingEvent, 5)] GeozoneEvent06 = 66, // 0x00000042
    [BinaryIo(IoType.GeofencingEvent, 6)] GeozoneEvent07 = 67, // 0x00000043
    [BinaryIo(IoType.GeofencingEvent, 7)] GeozoneEvent08 = 68, // 0x00000044
    [BinaryIo(IoType.GeofencingEvent, 8)] GeozoneEvent09 = 69, // 0x00000045
    [BinaryIo(IoType.GeofencingEvent, 9)] GeozoneEvent10 = 70, // 0x00000046
    [BinaryIo(IoType.GeofencingEvent, 10)] GeozoneEvent11 = 71, // 0x00000047
    [BinaryIo(IoType.GeofencingEvent, 11)] GeozoneEvent12 = 72, // 0x00000048
    [BinaryIo(IoType.GeofencingEvent, 12)] GeozoneEvent13 = 73, // 0x00000049
    [BinaryIo(IoType.GeofencingEvent, 13)] GeozoneEvent14 = 74, // 0x0000004A
    [BinaryIo(IoType.GeofencingEvent, 14)] GeozoneEvent15 = 75, // 0x0000004B
    [BinaryIo(IoType.GeofencingEvent, 15)] GeozoneEvent16 = 76, // 0x0000004C
    [BinaryIo(IoType.GeofencingEvent, 16)] GeozoneEvent17 = 77, // 0x0000004D
    [BinaryIo(IoType.GeofencingEvent, 17)] GeozoneEvent18 = 78, // 0x0000004E
    [BinaryIo(IoType.GeofencingEvent, 18)] GeozoneEvent19 = 79, // 0x0000004F
    [BinaryIo(IoType.GeofencingEvent, 19)] GeozoneEvent20 = 80, // 0x00000050
  }
}
