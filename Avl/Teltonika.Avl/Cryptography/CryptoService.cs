// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Cryptography.CryptoService
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Teltonika.Avl.Cryptography
{
  public sealed class CryptoService : IDisposable
  {
    private readonly SymmetricAlgorithm _algorithm;
    private readonly EncryptorService _encryptorService;
    private readonly DecryptorService _decryptorService;

    private CryptoService(SymmetricAlgorithm algorithm)
    {
      _algorithm = algorithm != null ? algorithm : throw new ArgumentNullException(nameof (algorithm));
      _encryptorService = CreateEncryptor();
      _decryptorService = CreateDecryptor();
    }

    public int BlockSize => _algorithm.BlockSize >> 3;

    public static CryptoService CreateCryptoService(
      byte[] key,
      byte[] iv,
      byte[] imei,
      byte[] testVector)
    {
      return CreateCryptoService(key, iv, new ArraySegment<byte>(imei), new ArraySegment<byte>(testVector));
    }

    public static CryptoService CreateCryptoService(
      byte[] key,
      byte[] iv,
      ArraySegment<byte> imei,
      ArraySegment<byte> testVector)
    {
      if (imei.Array == null)
        throw new ArgumentNullException(nameof (imei));
      if (imei.Count < 15)
        throw new ArgumentException("The imei length must be at least 15.", nameof (imei));
      if (testVector.Array == null)
        throw new ArgumentNullException(nameof (testVector));
      CryptoService cryptoService = CreateCryptoService(key, iv);
      byte[] numArray = cryptoService.Decryptor.Decrypt(testVector);
      if (numArray[0] == (imei.Array[imei.Offset + 14] ^ iv[0]) && numArray[1] == (imei.Array[imei.Offset + 13] ^ iv[1]) && numArray[2] == (imei.Array[imei.Offset + 12] ^ iv[2]) && numArray[3] == (imei.Array[imei.Offset + 11] ^ iv[3]))
        return cryptoService;
      cryptoService.Dispose();
      return null;
    }

    private static CryptoService CreateCryptoService(
      byte[] key,
      byte[] iv,
      CipherMode cipherMode = CipherMode.CBC,
      PaddingMode paddingMode = PaddingMode.Zeros)
    {
      if (key == null)
        throw new ArgumentNullException(nameof (key));
      if (iv == null)
        throw new ArgumentNullException(nameof (iv));
      AesCryptoServiceProvider cryptoServiceProvider = new AesCryptoServiceProvider();
      if (!((IEnumerable<KeySizes>) cryptoServiceProvider.LegalBlockSizes).IsKeySizeValid(iv.Length << 3))
      {
        ((IDisposable) cryptoServiceProvider).Dispose();
        throw new ArgumentException("The iv size is not valid.", nameof (iv));
      }
      if (!((IEnumerable<KeySizes>) cryptoServiceProvider.LegalKeySizes).IsKeySizeValid(key.Length << 3))
      {
        ((IDisposable) cryptoServiceProvider).Dispose();
        throw new ArgumentException("The key size is not valid.", nameof (key));
      }
      cryptoServiceProvider.Key = key;
      cryptoServiceProvider.IV = iv;
      cryptoServiceProvider.Mode = cipherMode;
      cryptoServiceProvider.Padding = paddingMode;
      return new CryptoService(cryptoServiceProvider);
    }

    public EncryptorService CreateEncryptor() => new EncryptorService(_algorithm);

    public DecryptorService CreateDecryptor() => new DecryptorService(_algorithm);

    public static int GetBufferSize(int input, int blockSize)
    {
      int num = input / blockSize;
      if (input % blockSize > 0)
        ++num;
      return num * blockSize;
    }

    public EncryptorService Encryptor => _encryptorService;

    public DecryptorService Decryptor => _decryptorService;

    public void Dispose()
    {
      _encryptorService.Dispose();
      _decryptorService.Dispose();
      ((IDisposable) _algorithm).Dispose();
    }
  }
}
