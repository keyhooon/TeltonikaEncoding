﻿// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.Codec12
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Commands
{
  public sealed class Codec12 : Codec<Command[], CommandArrayEncoding>
  {
    public static readonly Codec12 Instance = new Codec12();

    public override byte Id => 12;
  }
}
