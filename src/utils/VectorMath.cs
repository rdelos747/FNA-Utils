using Microsoft.Xna.Framework;

namespace Utils {

  public static class VectorMath {

    public static Vector2 Normalize(Vector2 vector) {
      return vector / vector.Length();
    }
  }
}