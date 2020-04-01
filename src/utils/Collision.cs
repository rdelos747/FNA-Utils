using Microsoft.Xna.Framework;

namespace Utils {
  /*
  Helper class that handles the math behind complex collisions. 
  
  All Collision methods should take primitve shapes (rectangles, circles, points, etc) and return boolean values.
  The decision of which collision function to use, as well as how to construct these primitives,
  should be up to the calling object, eg in Node.Collide();

  */
  public static class Collision {

    public static bool Box(Rectangle r1, Rectangle r2) {
      return r1.Intersects(r2);
    }

    public static bool Circle(Vector2 p1, Vector2 p2, int r1, int r2) {
      return false;
    }

    public static bool CircleBox(/* whatever */) {
      return false;
    }

  }
}