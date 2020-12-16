// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Cryptography.CryptoServiceExtensions
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Teltonika.Avl.Cryptography
{
  public static class CryptoServiceExtensions
  {
    public static byte[] Encrypt(this CryptoService service, byte[] buffer) => service.Encrypt(new ArraySegment<byte>(buffer, 0, buffer.Length));

    public static byte[] Encrypt(this CryptoService service, byte[] buffer, int offset, int count) => service.Encrypt(new ArraySegment<byte>(buffer, offset, count));

    public static byte[] Encrypt(this CryptoService service, ArraySegment<byte> segment) => service.Encryptor.Encrypt(segment);

    public static byte[] Decrypt(this CryptoService service, byte[] buffer) => service.Decrypt(new ArraySegment<byte>(buffer, 0, buffer.Length));

    public static byte[] Decrypt(this CryptoService service, byte[] buffer, int offset, int count) => service.Decrypt(new ArraySegment<byte>(buffer, offset, count));

    public static byte[] Decrypt(this CryptoService service, ArraySegment<byte> segment) => service.Decryptor.Decrypt(segment);

    public static bool IsKeySizeValid(this KeySizes keySizes, int keySize)
    {
      if (keySizes.SkipSize == 0)
        return keySizes.MinSize == keySize || keySizes.MaxSize == keySize;
      for (int minSize = keySizes.MinSize; minSize <= keySizes.MaxSize; minSize += keySizes.SkipSize)
      {
        if (keySize == minSize)
          return true;
      }
      return false;
    }

    public static bool IsKeySizeValid(this IEnumerable<KeySizes> keySizes, int keySize) => keySizes.Select(x => x.IsKeySizeValid(keySize)).FirstOrDefault(x => x);
  }
}
