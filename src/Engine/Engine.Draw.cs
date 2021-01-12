using System;
using System.IO;
using System.Runtime;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Utils
{

  public partial class Engine : Game
  {
    public static void DrawBox(Vector2 position, Size size, Color color)
    {
      DrawBox(
        (int)position.X,
        (int)position.Y,
        (int)size.Width,
        (int)size.Height,
        color
      );
    }

    public static void DrawBox(Vector2 position, int width, int height, Color color)
    {
      DrawBox(
        (int)position.X,
        (int)position.Y,
        width,
        height,
        color
      );
    }

    public static void DrawBox(int x, int y, int width, int height, Color color)
    {
      Engine.SpriteBatch.Draw(
        Engine.SystemRect,
        new Rectangle(x, y, width, height),
        null,
        color,
        0.0f,
        new Vector2(0, 0),
        SpriteEffects.None,
        0.0f
      );
    }

    public static void DrawOutlineBox(Vector2 pos, Size size, Color outlineColor)
    {
      DrawOutlineBox(
        new Rectangle(
          (int)pos.X,
          (int)pos.Y,
          (int)size.Width,
          (int)size.Height
        ),
        outlineColor
      );
    }

    public static void DrawOutlineBox(Rectangle rect, Color outlineColor, int thickness = 1)
    {
      DrawLine(rect.X, rect.Y, rect.Width, rect.Y, outlineColor, thickness);
      DrawLine(rect.Width, rect.Y, rect.Width, rect.Height, outlineColor, thickness);
      DrawLine(rect.Width, rect.Height, rect.X, rect.Height, outlineColor, thickness);
      DrawLine(rect.X, rect.Height, rect.X, rect.Y, outlineColor, thickness);
    }

    public static void DrawLine(Vector2 start, Vector2 end, Color color, int thickness = 1)
    {
      float angle = Calc.Angle(start, end);
      float dist = Vector2.Distance(start, end);

      Engine.SpriteBatch.Draw(
        Engine.SystemRect,
        start,
        null,
        color,
        angle,
        new Vector2(0, 0.5f),
        new Vector2(dist, thickness),
        SpriteEffects.None,
        0.0f
      );
    }

    public static void DrawLine(int x1, int y1, int x2, int y2, Color color, int thickness = 1)
    {
      DrawLine(
        new Vector2(x1, y1),
        new Vector2(x2, y2),
        color,
        thickness
      );
    }

    public static void DrawCircle(Vector2 pos, float radius, Color color, int res)
    {
      Vector2 last = Vector2.UnitX * radius;
      Vector2 lastP = last.Perpendicular();
      for (int i = 1; i <= res; i++)
      {
        Vector2 at = Calc.AngleToVector(i * MathHelper.PiOver2 / res, radius);
        Vector2 atP = at.Perpendicular();

        DrawLine(pos + last, pos + at, color);
        DrawLine(pos - last, pos - at, color);
        DrawLine(pos + lastP, pos + atP, color);
        DrawLine(pos - lastP, pos - atP, color);

        last = at;
        lastP = atP;
      }
    }
  }
}