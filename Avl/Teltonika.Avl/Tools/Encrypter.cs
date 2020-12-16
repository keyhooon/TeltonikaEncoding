// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Tools.Encrypter
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.Logging;

namespace Teltonika.Avl.Tools
{
  public class Encrypter
  {
    private const uint SecretKey = 827153418;
    private static readonly ILog log = LogManager.GetLogger(typeof (Encrypter));

    public byte[] Encrypt(uint key, byte[] stream) => Encrypt(key, stream, (uint) stream.Length);

    public byte[] Encrypt(uint key, byte[] stream, uint len)
    {
      try
      {
        uint num = 827153418;
        byte[] m = new byte[4]
        {
           0,
           0,
           0,
          (byte) (num >> 24 &  byte.MaxValue)
        };
        m[2] = (byte) (num >> 16 & byte.MaxValue);
        m[1] = (byte) (num >> 8 & byte.MaxValue);
        m[0] = (byte) (num & byte.MaxValue);
        _Encode(key, m, 4U);
        _Encode((uint) (((ulong) (m[3] << 24) & 4278190080UL) + (ulong) (m[2] << 16 & 16711680) + (ulong) (m[1] << 8 & 65280) + (ulong) (m[0] & byte.MaxValue)), stream, len);
        _Encode(key, stream, len);
        return stream;
      }
      catch (Exception ex)
      {
                log.ErrorFormat("Encode failure. {0}", (object) ex);
        return null;
      }
    }

    public byte[] Decrypt(uint key, byte[] stream) => Decrypt(key, stream, (uint) stream.Length);

    public byte[] Decrypt(uint key, byte[] stream, uint len)
    {
      try
      {
        uint num = 827153418;
        byte[] m = new byte[4]
        {
           0,
           0,
           0,
          (byte) (num >> 24 &  byte.MaxValue)
        };
        m[2] = (byte) (num >> 16 & byte.MaxValue);
        m[1] = (byte) (num >> 8 & byte.MaxValue);
        m[0] = (byte) (num & byte.MaxValue);
        _Encode(key, m, 4U);
        _Decode(key, stream, len);
        _Decode((uint) (((ulong) (m[3] << 24) & 4278190080UL) + (ulong) (m[2] << 16 & 16711680) + (ulong) (m[1] << 8 & 65280) + (ulong) (m[0] & byte.MaxValue)), stream, len);
        return stream;
      }
      catch (Exception ex)
      {
                log.ErrorFormat("Decode failure. {0}", (object) ex);
        return null;
      }
    }

    private void _Encode(uint Key, byte[] m, uint Len)
    {
      uint num1 = Len;
      ushort num2 = (ushort) ((int) Key & ushort.MaxValue ^ 54650);
      byte num3 = (byte) ((int) ((Key & 1044480U) >> 12) ^ (byte)Len ^ 86);
      ushort num4 = (ushort) ((ushort)((int)((Key & 4294901760U) >> 16) & ushort.MaxValue & ushort.MaxValue) ^ (uint) (ushort) (num3 << 8 & ushort.MaxValue) ^ (ushort)(num2 * 3 & ushort.MaxValue));
      while (num1 > 0U)
      {
        --num1;
        byte num5 = m[num1];
        ushort num6 = (ushort) ((ushort)((ushort)((ushort)(m[num1] << (num3 % 5 & ushort.MaxValue) & ushort.MaxValue) ^ (uint)(ushort)(num2 << (num3 % 3 & ushort.MaxValue) & ushort.MaxValue)) ^ (uint)(ushort)(num4 >> (num3 % 7 & ushort.MaxValue) & ushort.MaxValue)) >> (num3 % 5 & ushort.MaxValue) & ushort.MaxValue);
        m[num1] = (byte) (num6 & (uint) byte.MaxValue);
        num2 ^= (ushort) (num6 ^ 42361U ^ num5);
        num3 ^= (byte) ((num2 ^ num6 >> 8 & ushort.MaxValue) & byte.MaxValue);
        num4 ^= (ushort) (num2 >> 2 ^ num3 << 8);
      }
    }

    private void _Decode(uint key, byte[] stream, uint myLen)
    {
      uint num1 = myLen;
      ushort num2 = (ushort) (((int) key & ushort.MaxValue & ushort.MaxValue ^ 54650) & ushort.MaxValue);
      byte num3 = (byte) (((int) ((key & 1044480U) >> 12) ^ (byte)(((int)num1 ^ 86) & byte.MaxValue)) & byte.MaxValue);
      ushort num4 = (ushort) (((int) ((key & 4294901760U) >> 16) & ushort.MaxValue ^ (ushort)(num3 << 8 & ushort.MaxValue) ^ (ushort)(num2 * 3 & ushort.MaxValue)) & ushort.MaxValue);
      while (num1 > 0U)
      {
        --num1;
        byte num5 = stream[num1];
        byte num6 = (byte) ((ushort)((ushort)((ushort)((ushort)(stream[num1] << num3 % 5 & ushort.MaxValue) ^ (uint)(ushort)(num2 << num3 % 3 & ushort.MaxValue)) ^ (uint)(ushort)(num4 >> num3 % 7 & ushort.MaxValue)) >> num3 % 5 & ushort.MaxValue) & (uint) byte.MaxValue);
        stream[num1] = num6;
        ushort num7 = (ushort) ((ushort)((ushort)((ushort)(num6 << num3 % 5 & ushort.MaxValue) ^ (uint)(ushort)(num2 << num3 % 3 & ushort.MaxValue)) ^ (uint)(ushort)(num4 >> num3 % 7 & ushort.MaxValue)) >> num3 % 5 & ushort.MaxValue);
        num2 ^= (ushort) ((num7 ^ 42361 ^ num6) & ushort.MaxValue);
        num3 ^= (byte) ((num2 ^ num7 >> 8) & byte.MaxValue);
        num4 ^= (ushort) ((num2 >> 2 ^ num3 << 8) & ushort.MaxValue);
      }
    }
  }
}
