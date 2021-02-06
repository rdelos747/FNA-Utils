using System;
using Microsoft.Xna.Framework;

namespace Utils
{

  public static class Calc
  {

    public const float DegToRad = MathHelper.Pi / 180f;
    public const float RadToDeg = 180f / MathHelper.Pi;

    public static float Approach(float val, float target, float maxMove)
    {
      return val > target ? Math.Max(val - maxMove, target) : Math.Min(val + maxMove, target);
    }

    public static int Clamp(int value, int min, int max)
    {
      return Math.Min(Math.Max(value, min), max);
    }

    public static float Clamp(float value, float min, float max)
    {
      return Math.Min(Math.Max(value, min), max);
    }

    public static bool InRange(float val, float min, float max)
    {
      return val >= min && val <= max;
    }

    public static bool InRange(float val, int min, int max)
    {
      return val >= min && val <= max;
    }

    public static float Angle(Vector2 from, Vector2 to)
    {
      return (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
    }

    public static float Angle(this Vector2 vector)
    {
      return (float)Math.Atan2(vector.Y, vector.X);
    }

    public static Vector2 Perpendicular(this Vector2 vector)
    {
      return new Vector2(-vector.Y, vector.X);
    }

    public static Vector2 AngleToVector(float angleRadians, float length)
    {
      return new Vector2((float)Math.Cos(angleRadians) * length, (float)Math.Sin(angleRadians) * length);
    }

    public static Vector2 Rotate(this Vector2 vec, float angleRadians)
    {
      return AngleToVector(vec.Angle() + angleRadians, vec.Length());
    }

    public static float ToDeg(this float f)
    {
      return f * RadToDeg;
    }

    public static float WrapAngleDeg(float angleDegrees)
    {
      return (((angleDegrees * Math.Sign(angleDegrees) + 180) % 360) - 180) * Math.Sign(angleDegrees);
    }

    public static Rectangle RectFromVects(Vector2 a, Vector2 b)
    {
      int smallestX = (int)Math.Min(a.X, b.X); //Smallest X
      int smallestY = (int)Math.Min(a.Y, b.Y); //Smallest Y
      int largestX = (int)Math.Max(a.X, b.X);  //Largest X
      int largestY = (int)Math.Max(a.Y, b.Y);  //Largest Y

      //calc the width and height
      int width = largestX - smallestX;
      int height = largestY - smallestY;

      //assuming Y is small at the top of screen
      return new Rectangle(smallestX, smallestY, width, height);
    }
  }
}