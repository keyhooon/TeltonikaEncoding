// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Cryptography.EncryptorService
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Security.Cryptography;
using Teltonika.Logging;

namespace Teltonika.Avl.Cryptography
{
  public sealed class EncryptorService : IDisposable
  {
    private static readonly ILog Log = LogManager.GetLogger<EncryptorService>();
    private readonly ICryptoTransform _encryptor;
    private readonly int _blockSize;

    public EncryptorService(SymmetricAlgorithm algorithm)
    {
      _encryptor = algorithm != null ? algorithm.CreateEncryptor() : throw new ArgumentNullException(nameof (algorithm));
      _blockSize = algorithm.BlockSize >> 3;
    }

    public byte[] Encrypt(byte[] input) => Encrypt(new ArraySegment<byte>(input));

    public byte[] Encrypt(byte[] input, int offset, int count) => Encrypt(new ArraySegment<byte>(input, offset, count));

    public byte[] Encrypt(ArraySegment<byte> input)
    {
      if (input.Array == null)
        throw new ArgumentException("The input must have a valid array.", nameof (input));
      byte[] numArray = new byte[CryptoService.GetBufferSize(input.Count, _blockSize)];
      int outputOffset = 0;
      int num1 = input.Offset + input.Count;
      for (int offset = input.Offset; offset < num1; offset += _blockSize)
      {
        int num2 = _encryptor.TransformBlock(input.Array, offset, _blockSize, numArray, outputOffset);
        outputOffset += num2;
      }
      if (Log.IsDebugEnabled)
                Log.DebugFormat("Encrypt [{0}] => [{1}]", input.ToHexString(), numArray.ToHexString());
      return numArray;
    }

    public void Dispose() => _encryptor.Dispose();
  }
}
