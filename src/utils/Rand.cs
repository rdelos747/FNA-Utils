using System;

namespace Utils {
  public static class Rand {
    private static Random Random = new Random();

    public static int RandInt(int n) {
      return Random.Next(n);
    }

    public static int RandRange(int n, int m) {
      return Random.Next(n, m + 1);
    }

    public static float RandRange(float n, float m) {
      return (float)Random.NextDouble() * (m - n) + n;
    }

    public static bool Chance(int n) {
      return RandRange(0, 100) < n;
    }
  }
}