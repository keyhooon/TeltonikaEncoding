// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Data.Bofan.BofanDataBodyEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Teltonika.Avl.Data.Bofan
{
  public sealed class BofanDataBodyEncoding
  {
    public static readonly BofanDataBodyEncoding Instance = new BofanDataBodyEncoding();
    private static readonly Regex Regex = new Regex("\\$POS,(?<imei>\\d+),(?<time>\\d{6}\\.\\d{3}),(?<fixFlag>[VA]),(?<latDegrees>\\d{2})(?<latMinutes>\\d{2}\\.\\d{4}),(?<latMark>[NS]),(?<longDegrees>\\d{3})(?<longMinutes>\\d{2}\\.\\d{4}),(?<longMark>[EW]),(?<speed>(\\d+\\.\\d+)?),(?<angleString>\\d{1,3}\\.\\d+),(?<date>\\d{6}),,,([^/]*/){5}.{2}(?<satellites>[0-9a-f])/.*", RegexOptions.Compiled);

    public BofanAvlData[] Decode(byte[] bytes) => bytes != null ? ((IEnumerable<string>) System.Text.Encoding.ASCII.GetString(((IEnumerable<byte>) bytes).ToArray()).Split(new string[2]
    {
      "\r\n",
      "#\n"
    }, StringSplitOptions.RemoveEmptyEntries)).Select(new Func<string, BofanAvlData>(ParseRecord)).ToArray() : throw new ArgumentNullException(nameof (bytes));

    private BofanAvlData ParseRecord(string record)
    {
      Match match = Regex.Match(record);
      if (!match.Success)
        throw new ApplicationException("Could not parse record [" + record + "].");
      return new BofanAvlData(match.Groups["imei"].Value, new AvlData(AvlDataPriority.Low, ParseTimestamp(match.Groups["date"].Value, match.Groups["time"].Value), ParseGpsElement(match.Groups["latDegrees"].Value, match.Groups["latMinutes"].Value, match.Groups["latMark"].Value, match.Groups["longDegrees"].Value, match.Groups["longMinutes"].Value, match.Groups["longMark"].Value, match.Groups["speed"].Value, match.Groups["angleString"].Value, match.Groups["satellites"].Value)));
    }

    private GpsElement ParseGpsElement(
      string latDegreesString,
      string latMinutesString,
      string latMarkString,
      string longDegreesString,
      string longMinutesString,
      string longMarkString,
      string speedString,
      string angleString,
      string satellitesString)
    {
      int num1 = latMarkString.Equals("N") ? 1 : -1;
      int y = ParseCoordinate(latDegreesString, latMinutesString) * num1;
      int num2 = longMarkString.Equals("E") ? 1 : -1;
      int x = ParseCoordinate(longDegreesString, longMinutesString) * num2;
      double result;
      short speed = double.TryParse(speedString, NumberStyles.Float, CultureInfo.InvariantCulture, out result) ? (short) Math.Round(result * 1.852) : byte.MaxValue;
      short angle = (short) Math.Round(float.Parse(angleString, CultureInfo.InvariantCulture));
      byte satellites = byte.Parse(satellitesString, NumberStyles.HexNumber);
      return new GpsElement(x, y, 0, speed, angle, satellites);
    }

    private DateTime ParseTimestamp(string date, string time) => DateTime.SpecifyKind(DateTime.ParseExact(date + " " + time, "ddMMyy HHmmss.fff", CultureInfo.InvariantCulture), DateTimeKind.Utc);

    private int ParseCoordinate(string degreesString, string minutesString) => (int) Math.Round((int.Parse(degreesString) + double.Parse(minutesString, CultureInfo.InvariantCulture) / 60.0) * 10000000.0);
  }
}
