// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Tools.HuffmanDecoder
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using System.Collections.Generic;
using System.IO;
using Teltonika.IO;

namespace Teltonika.Avl.Tools
{
  public static class HuffmanDecoder
  {
    public static byte[] Decode(byte[] bytes)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (bytes.Length == 0)
        return new byte[0];
      using (MemoryStream memoryStream = new MemoryStream(bytes))
        return Decode(memoryStream);
    }

    public static byte[] Decode(byte[] bytes, int offset, int count)
    {
      if (bytes == null)
        throw new ArgumentNullException(nameof (bytes));
      if (bytes.Length == 0)
        return new byte[0];
      byte[] bytes1 = new byte[count];
      Buffer.BlockCopy(bytes, offset, bytes1, 0, count);
      return Decode(bytes1);
    }

    public static byte[] Decode(Stream stream)
    {
      short num1 = stream != null ? new EndianBinaryReader(EndianBitConverter.Little, stream).ReadInt16() : throw new ArgumentNullException(nameof (stream));
      List<Symbol> symbolList = new List<Symbol>();
      EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Big, stream);
      byte num2 = 0;
      byte num3 = 0;
      for (; num2 < 8; ++num2)
      {
        byte num4 = endianBinaryReader.ReadByte();
        int num5 = num4 & 15;
        int num6 = (num4 & 240) >> 4;
                Symbol symbol1 = new Symbol();
                Symbol symbol2 = symbol1;
        int num7 = num3;
        byte num8 = (byte) (num7 + 1);
        symbol2.Value = (byte) num7;
        symbol1.Length = (short) num5;
                Symbol symbol3 = symbol1;
        if (symbol3.Length != 0)
          symbolList.Add(symbol3);
                Symbol symbol4 = new Symbol();
                Symbol symbol5 = symbol4;
        int num9 = num8;
        num3 = (byte) (num9 + 1);
        symbol5.Value = (byte) num9;
        symbol4.Length = (short) num6;
                Symbol symbol6 = symbol4;
        if (symbol6.Length != 0)
          symbolList.Add(symbol6);
      }
            Symbol[] array1 = symbolList.ToArray();
      byte[] numArray = Decode(stream, array1, 256);
      symbolList.Clear();
      for (int index = 0; index < numArray.Length; ++index)
      {
        if (numArray[index] != 0)
          symbolList.Add(new Symbol()
          {
            Length = numArray[index],
            Value = (byte) index
          });
      }
            Symbol[] array2 = symbolList.ToArray();
      return Decode(stream, array2, num1);
    }

    private static byte[] Decode(Stream stream, Symbol[] symbols, int length)
    {
      Array.Sort(symbols);
      short num1 = 0;
      short num2 = 0;
      int index1 = 0;
      while (index1 < symbols.Length)
      {
        short length1 = symbols[index1].Length;
        short num3 = (short) (num1 >> num2 - length1);
        symbols[index1].Code = num3;
        num2 = length1;
        ++index1;
        num1 = (short) (num3 + 1);
      }
      for (int index2 = 0; index2 < symbols.Length; ++index2)
        symbols[index2].ReverseCodeBits();
      List<byte> byteList = new List<byte>();
      EndianBinaryReader endianBinaryReader = new EndianBinaryReader(EndianBitConverter.Big, stream);
      byte num4 = 0;
      int num5 = 0;
      short testCode = 0;
      short testLength = 0;
      while (byteList.Count < length)
      {
        if (num5 == 0)
        {
          num4 = endianBinaryReader.ReadByte();
          num5 = 8;
        }
        while (num5 > 0)
        {
          int num3 = num4 & 1;
          testCode <<= 1;
          testCode |= (short) num3;
          num4 >>= 1;
          --num5;
          ++testLength;
                    Symbol symbol = Array.Find(symbols, match => match.IsMatch(Symbol.ReverseBits(testCode, testLength), testLength));
          if (symbol != null)
          {
            testCode = 0;
            testLength = 0;
            byteList.Add(symbol.Value);
          }
          if (testLength == 16)
            return new byte[0];
        }
      }
      return byteList.ToArray();
    }

    private sealed class Symbol : IComparable<Symbol>
    {
      public byte Value { get; set; }

      public short Length { get; set; }

      public short Code { get; set; }

      public void ReverseCodeBits() => Code = ReverseBits(Code, Length);

      public static short ReverseBits(short value, short length)
      {
        short num1 = 0;
        for (short index = length; index > 0; --index)
        {
          int num2 = 1 << index - 1;
          short num3 = (short) ((short)((short)(value & num2) >> index - 1) << length - index);
          num1 |= num3;
        }
        return num1;
      }

      public bool IsMatch(short code, short length)
      {
        if (length == 0 || Length != length)
          return false;
        for (int index = 0; index < Length; ++index)
        {
          int num = 1 << index;
          if ((short)(Code & num) != (short)(code & num))
            return false;
        }
        return true;
      }

      public override string ToString() => "[val=" + Value + ",size=" + Length + ",code=" + Code + "]";

      public int CompareTo(Symbol other)
      {
        if (Length < other.Length)
          return 1;
        if (Length > other.Length || Value < other.Value)
          return -1;
        return Value > other.Value ? 1 : 0;
      }
    }
  }
}
