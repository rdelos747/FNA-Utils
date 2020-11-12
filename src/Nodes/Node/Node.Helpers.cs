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

    public void DrawOutlineBox(Vector2 pos, Size size, Color outlineColor)
    {
      Vector2 p = pos + DrawPosition;
      Size s = size + DrawPosition;
      Engine.DrawOutlineBox(
        new Rectangle(
          (int)p.X,
          (int)p.Y,
          (int)s.Width,
          (int)s.Height
        ),
        outlineColor
      );
    }

    public void DrawLine(int x1, int y1, int x2, int y2, Color color, int thickness = 1)
    {
      Engine.DrawLine(
        new Vector2(x1, y1) + DrawPosition,
        new Vector2(x2, y2) + DrawPosition,
        color,
        thickness
      );
    }
  }
}