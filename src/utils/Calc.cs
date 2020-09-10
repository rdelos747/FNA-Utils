using System;

namespace Utils {

  public static class Calc {

    public static float Approach(float val, float target, float maxMove) {
      return val > target ? Math.Max(val - maxMove, target) : Math.Min(val + maxMove, target);
    }
  }
}