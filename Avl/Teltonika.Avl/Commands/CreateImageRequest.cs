﻿// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.CreateImageRequest
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

namespace Teltonika.Avl.Commands
{
  public sealed class CreateImageRequest : ImageCommand
  {
    public override int Id => 1;

    public override int ImageId => -1;

    public override string ToString() => "Create image";
  }
}
