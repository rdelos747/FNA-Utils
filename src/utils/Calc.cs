using System;

namespace Utils
{

  public static class Calc
  {

    public static float Approach(float val, float target, float maxMove)
    {
      return val > target ? Math.Max(val - maxMove, target) : Math.Min(val + maxMove, target);
    }

    public static bool InRange(float val, float min, float max)
    {
      return val >= min && val <= max;
    }

    public static bool InRange(float val, int min, int max)
    {
      return val >= min && val <= max;
    }
  }
}