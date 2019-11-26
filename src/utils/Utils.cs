using System;

namespace Engine {
  public static class Utils {
    private static Random random = new Random();

    public static int rand(int n, int m) {
      return random.Next(n, m + 1);
    }

    public static bool chance(int n) {
      return rand(0, 100) < n;
    }
  }
}