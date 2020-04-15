using Microsoft.Xna.Framework;
using System;

namespace Utils {

  public static class VectorMath {

    public static Vector2 Normalize(Vector2 vector) {
      return vector / vector.Length();
    }

    public static Vector2 Rotate(Vector2 vector, float radians) {
      return Vector2.Transform(vector, Matrix.CreateRotationZ(radians));
    }

    public static float Rotation(Vector2 vector) {
      return (float)Math.Atan2(vector.Y, vector.X);
    }
  }
}