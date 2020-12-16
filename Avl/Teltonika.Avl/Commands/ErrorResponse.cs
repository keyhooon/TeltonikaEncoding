// Decompiled with JetBrains decompiler
// Type: Teltonika.Avl.Commands.ErrorResponse
// Assembly: Teltonika.Avl, Version=1.4.4.0, Culture=neutral, PublicKeyToken=null
// MVID: 74AE4D6A-12C9-4B78-84AB-F581C6CFF6B5
// Assembly location: F:\FLEET\Tavl\Teltonika.Avl.dll

using System;
using Teltonika.IO;

namespace Teltonika.Avl.Commands
{
  public sealed class ErrorResponse : ImageCommand
  {
    private readonly int _id;
    private readonly int _commandId;
    private readonly int _error;

    public ErrorResponse(int id, int commandId, int error)
    {
      _id = id == 238 || id == 239 ? id : throw new ArgumentOutOfRangeException(nameof (id));
      _commandId = commandId;
      _error = error;
    }

    public override int Id => _id;

    public override int ImageId => -1;

    public int CommandId => _commandId;

    public CameraImageError Error => (CameraImageError) _error;

    public static ErrorResponse Decode(byte id, IBinaryReader reader)
    {
      byte num1 = reader.ReadByte();
      byte num2 = reader.ReadByte();
      return new ErrorResponse(id, num1, num2);
    }

    public override string ToString() => string.Format("Error {0} {1} {1:X}", CommandId, Error);
  }
}
