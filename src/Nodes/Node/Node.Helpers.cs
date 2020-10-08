using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils
{
  public partial class Node : IDisposable
  {
    public float DistanceTo(Node other)
    {
      return Vector2.Distance(Position, other.Position);
    }

    public float DistanceTo(Vector2 point)
    {
      return Vector2.Distance(Position, point);
    }
  }
}