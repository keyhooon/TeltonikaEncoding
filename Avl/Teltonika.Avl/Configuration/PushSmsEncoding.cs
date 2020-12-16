// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Configuration.PushSmsEncoding
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl.Configuration
{
  public sealed class PushSmsEncoding
  {
    private static readonly PushSmsEncoding instance = new PushSmsEncoding();

    public static PushSmsEncoding Instance => instance;

    private PushSmsEncoding()
    {
    }

    public void EncodeFM4(PushSms pushSms, Stream stream)
    {
      EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
      if (pushSms.Login == null)
        throw new ArgumentNullException("Login");
      if (pushSms.Password == null)
        throw new ArgumentNullException("Password");
      if (pushSms.ConfigHost == null)
        throw new ArgumentNullException("ConfigHost");
      suitableBinaryWriter.Write((byte) pushSms.Login.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.Login));
      suitableBinaryWriter.Write((byte) pushSms.Password.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.Password));
      suitableBinaryWriter.Write((byte) pushSms.ConfigHost.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.ConfigHost));
      suitableBinaryWriter.Write(pushSms.ConfigPort);
      suitableBinaryWriter.Write((byte) pushSms.Apn.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.Apn));
      suitableBinaryWriter.Write((byte) pushSms.GprsLogin.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.GprsLogin));
      suitableBinaryWriter.Write((byte) pushSms.GprsPassword.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.GprsPassword));
    }

    public byte[] EncodeFM4(PushSms pushSms)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        EncodeFM4(pushSms, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }

    public void EncodeFM3(PushSms pushSms, Stream stream)
    {
      EndianBinaryWriter suitableBinaryWriter = stream.CreateSuitableBinaryWriter();
      if (pushSms.Login == null)
        throw new ArgumentNullException("Login");
      if (pushSms.Password == null)
        throw new ArgumentNullException("Password");
      if (pushSms.ConfigHost == null)
        throw new ArgumentNullException("ConfigHost");
      suitableBinaryWriter.Write((byte) pushSms.Login.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.Login));
      suitableBinaryWriter.Write((byte) pushSms.Password.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.Password));
      suitableBinaryWriter.Write((byte) pushSms.ConfigHost.Length);
      suitableBinaryWriter.Write(Encoding.UTF8.GetBytes(pushSms.ConfigHost));
      suitableBinaryWriter.Write(pushSms.ConfigPort);
    }

    public byte[] EncodeFM3(PushSms pushSms)
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        EncodeFM3(pushSms, memoryStream);
        memoryStream.Position = 0L;
        using (BinaryReader binaryReader = new BinaryReader(memoryStream))
          return binaryReader.ReadBytes((int) memoryStream.Length);
      }
    }
  }
}
