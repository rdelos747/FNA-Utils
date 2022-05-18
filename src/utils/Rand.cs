using System;

namespace Utils
{
  public class Rand
  {
    public Random Random { get; private set; }

    public Rand()
    {
      Random = new Random();
    }

    public Rand(int seed)
    {
      Random = new Random(seed);
    }

    public int RandInt(int n)
    {
      return Random.Next(n);
    }

    public int RandRange(int n, int m)
    {
      return Random.Next(n, m + 1);
    }

    public float RandRange(float n, float m)
    {
      return (float)Random.NextDouble() * (m - n) + n;
    }

    public bool Chance(int n)
    {
      return RandRange(0, 100) < n;
    }
  }
}