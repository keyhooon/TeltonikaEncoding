// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Configuration.ConfigurationEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Teltonika.IO;

namespace Teltonika.Avl.Configuration
{
  public sealed class ConfigurationEncoding
  {
    private static readonly ConfigurationEncoding _instance = new ConfigurationEncoding();

    public static ConfigurationEncoding Instance => _instance;

    private ConfigurationEncoding()
    {
    }

    public void Encode(IEnumerable<ConfigurationParameter> configuration, Stream stream)
    {
      short int16_1 = Convert.ToInt16(configuration.Count());
      byte ticks = (byte) DateTime.Now.Ticks;
      EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
      suitableBinaryWriter.Write((short) 0);
      suitableBinaryWriter.Write(ticks);
      suitableBinaryWriter.Write(int16_1);
      foreach (ConfigurationParameter configurationParameter in configuration)
      {
        if (configurationParameter.Value == null)
          throw new ArgumentNullException("ParamValue");
                Encode(configurationParameter, suitableBinaryWriter);
      }
      stream.CreateSuitableBinaryWriter();
      short int16_2 = Convert.ToInt16(stream.Length - 2L);
      stream.Position = 0L;
      stream.CreateSuitableBinaryWriter().Write(int16_2);
    }

    public void EncodeFm3(IEnumerable<ConfigurationParameter> configuration, Stream stream)
    {
      byte num = Convert.ToByte(configuration.Count());
      byte ticks = (byte) DateTime.Now.Ticks;
      EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
      suitableBinaryWriter.Write((short) 0);
      suitableBinaryWriter.Write(ticks);
      suitableBinaryWriter.Write(num);
      foreach (ConfigurationParameter configurationParameter in configuration)
      {
        if (configurationParameter.Value == null)
          throw new ArgumentNullException("ParamValue");
                Encode(configurationParameter, suitableBinaryWriter);
      }
      stream.CreateSuitableBinaryWriter();
      short int16 = Convert.ToInt16(stream.Length - 2L);
      stream.Position = 0L;
      stream.CreateSuitableBinaryWriter().Write(int16);
    }

    public byte[] Encode(IEnumerable<ConfigurationParameter> configuration)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        Encode(configuration, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }

    public byte[] EncodeFm3(IEnumerable<ConfigurationParameter> configuration)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        EncodeFm3(configuration, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }

    private static void Encode(ConfigurationParameter param, EndianBinaryWriter w)
    {
      byte[] tcpBytes = param.GetTcpBytes();
      w.Write(param.Id);
      w.Write((short) tcpBytes.Length);
      w.Write(tcpBytes);
    }
  }
}
