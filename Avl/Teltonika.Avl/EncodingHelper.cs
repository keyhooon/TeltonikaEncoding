// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.EncodingHelper
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Teltonika.IO;

namespace Teltonika.Avl
{
    public static class EncodingHelper
    {
        public static readonly char[] Alphabet = new char[16]
        {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'a',
      'b',
      'c',
      'd',
      'e',
      'f'
        };

        public static EndianBitConverter CreateSuitableBitConverter() => new BigEndianBitConverter();

        public static EndianBinaryWriter CreateSuitableBinaryWriter(this Stream stream) => new EndianBinaryWriter(CreateSuitableBitConverter(), stream);

        public static EndianBinaryReader CreateSuitableBinaryReader(this Stream stream) => new EndianBinaryReader(CreateSuitableBitConverter(), stream);

        public static OrientedBitStream CreateSuitableBitStream(this Stream stream) => new OrientedBitStream(stream, OrientedBitStream.BitOrder.LsbMsb);

        public static OrientedBitStream CreateSuitableInMemoryBitStream() => new MemoryStream().CreateSuitableBitStream();

        public static IBitReader CreateSuitableBitReader(this Stream stream)
        {
            OrientedBitStream suitableBitStream = stream.CreateSuitableBitStream();
            return new OrientedEndianBitReader(CreateSuitableBitConverter(), suitableBitStream);
        }

        public static IBitWriter CreateSuitableBitWriter(this Stream stream)
        {
            OrientedBitStream suitableBitStream = stream.CreateSuitableBitStream();
            return new OrientedEndianBitWriter(CreateSuitableBitConverter(), suitableBitStream);
        }

        public static string ToHexString<T>(T obj, IEncoding<T> encoding)
        {
            using (OrientedBitStream inMemoryBitStream = CreateSuitableInMemoryBitStream())
            {
                IBitWriter suitableBitWriter = inMemoryBitStream.CreateSuitableBitWriter();
                encoding.Encode(obj, suitableBitWriter);
                inMemoryBitStream.Position = 0L;
                byte[] numArray = new byte[inMemoryBitStream.Length / 8L];
                inMemoryBitStream.Read(numArray, 0, numArray.Length);
                return numArray.ToHexString();
            }
        }

        public static string ToHexString(this byte[] bytes)
        {
            int num1 = bytes != null ? bytes.Length : throw new ArgumentNullException(nameof(bytes));
            StringBuilder stringBuilder = new StringBuilder(num1 << 1);
            for (int index = 0; index < num1; ++index)
            {
                byte num2 = bytes[index];
                char ch1 = Alphabet[num2 >> 4];
                char ch2 = Alphabet[num2 & 15];
                stringBuilder.Append(ch1).Append(ch2);
            }
            return stringBuilder.ToString();
        }

        public static string ToHexString(this ArraySegment<byte> bytes)
        {
            int offset = bytes.Offset;
            int num1 = offset + bytes.Count;
            int capacity = bytes.Count << 1;
            StringBuilder stringBuilder = new StringBuilder(capacity)
            {
                Length = capacity
            };
            int index1 = offset;
            int index2 = 0;
            while (index1 < num1)
            {
                byte num2 = bytes.Array[index1];
                char ch1 = Alphabet[num2 >> 4];
                char ch2 = Alphabet[num2 & 15];
                stringBuilder[index2] = ch1;
                stringBuilder[index2 + 1] = ch2;
                ++index1;
                index2 += 2;
            }
            return stringBuilder.ToString();
        }

        public static string ToHexString(this byte[] bytes, int offset, int count) => new ArraySegment<byte>(bytes, offset, count).ToHexString();

        public static string ToHexString(this IEnumerable<byte> bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte num in bytes)
            {
                char ch1 = Alphabet[num >> 4];
                char ch2 = Alphabet[num & 15];
                stringBuilder.Append(ch1).Append(ch2);
            }
            return stringBuilder.ToString();
        }

        public static byte[] ToByteArray(this string hex)
        {
            if (hex == null)
                throw new ArgumentNullException(nameof(hex));
            if (hex.Length % 2 != 0)
                throw new InvalidOperationException("hex length");
            if (hex.Length == 0)
                return new byte[0];
            byte[] numArray = new byte[hex.Length / 2];
            int num1 = 0;
            for (int index1 = 0; index1 < numArray.Length; ++index1, num1 += 2)
            {
                numArray[index1] = Convert.ToByte(new string(new char[] { hex[num1], hex[num1 + 1] }), 16);
            }
            return numArray;
        }

        public static byte[] ToByteArray(this Stream stream) => stream != null ? new BinaryReader(stream).ReadBytes(stream is OrientedBitStream ? (int)(stream.Length / 8L) : (int)stream.Length) : throw new ArgumentNullException(nameof(stream));
    }
}
