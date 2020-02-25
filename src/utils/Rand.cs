using System;

namespace Utils {
  public static class Rand {
    private static Random random = new Random();

    public static int RandInt(int n) {
      return random.Next(n);
    }

    public static int RandRange(int n, int m) {
      return random.Next(n, m + 1);
    }

    public static bool Chance(int n) {
      return RandRange(0, 100) < n;
    }
  }
}