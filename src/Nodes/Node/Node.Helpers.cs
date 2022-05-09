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

    public void DrawBox(float x, float y, Size size, Color color)
    {
      DrawBox(new Vector2(x, y), size, color);
    }

    public void DrawBox(Vector2 pos, Size size, Color color)
    {
      Engine.DrawBox(pos + (WorldPosition - Calc.Floor(Origin)), size, color);
    }

    public void DrawOutlineBox(Vector2 pos, Size size, Color outlineColor, int thickness = 1)
    {
      Engine.DrawOutlineBox(
        pos: pos + (WorldPosition - Calc.Floor(Origin)),
        size: size,
        outlineColor: outlineColor,
        thickness: thickness
      );
    }

    public void DrawLine(int x1, int y1, int x2, int y2, Color color, int thickness = 1)
    {
      Engine.DrawLine(
        new Vector2(x1, y1) + WorldPosition,
        new Vector2(x2, y2) + WorldPosition,
        color,
        thickness
      );
    }

    public void DrawLine(Vector2 p1, Vector2 p2, Color color, int thickness = 1)
    {
      Engine.DrawLine(
        p1 + WorldPosition,
        p2 + WorldPosition,
        color,
        thickness
      );
    }

    public void DrawCircle(Vector2 pos, float rad, Color color, int res)
    {
      Engine.DrawCircle(
        pos + WorldPosition,
        rad,
        color,
        res
      );
    }
  }
}