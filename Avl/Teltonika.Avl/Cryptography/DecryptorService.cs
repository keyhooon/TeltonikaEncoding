// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Cryptography.DecryptorService
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Security.Cryptography;
using Teltonika.Logging;

namespace Teltonika.Avl.Cryptography
{
  public sealed class DecryptorService : IDisposable
  {
    private static readonly ILog Log = LogManager.GetLogger<DecryptorService>();
    private readonly int _blockSize;
    private readonly ICryptoTransform _decryptor;

    public DecryptorService(SymmetricAlgorithm algorithm)
    {
      _decryptor = algorithm != null ? algorithm.CreateDecryptor() : throw new ArgumentNullException(nameof (algorithm));
      _blockSize = algorithm.BlockSize >> 3;
    }

    public byte[] Decrypt(byte[] input) => Decrypt(new ArraySegment<byte>(input));

    public byte[] Decrypt(byte[] input, int offset, int count) => Decrypt(new ArraySegment<byte>(input, offset, count));

    public byte[] Decrypt(ArraySegment<byte> input)
    {
      byte[] numArray = input.Array != null ? new byte[input.Count] : throw new ArgumentException("The input must have a valid array.", nameof (input));
      int outputOffset = 0;
      int num1 = input.Offset + input.Count;
      for (int offset = input.Offset; offset < num1; offset += _blockSize)
      {
        int num2 = _decryptor.TransformBlock(input.Array, offset, _blockSize, numArray, outputOffset);
        outputOffset += num2;
      }
      if (Log.IsDebugEnabled)
                Log.DebugFormat("Decrypt [{0}] => [{1}]", input.ToHexString(), numArray.ToHexString());
      return numArray;
    }

    public void Dispose() => _decryptor.Dispose();
  }
}
