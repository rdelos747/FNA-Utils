using Microsoft.Xna.Framework;
using System;

namespace Utils {
  /*
  Helper class that handles the math behind complex collisions. 
  
  All Collision methods should take primitve shapes (rectangles, circles, points, etc) and return boolean values.
  The decision of which collision function to use, as well as how to construct these primitives,
  should be up to the calling object, eg in Node.Collide();

  */
  public static class Collision {

    public static bool RectangleRectangle(Vector2 r1TopLeftPosition, Vector2 r1Size, Vector2 r2TopLeftPosition, Vector2 r2Size) {
      return r1TopLeftPosition.X + r1Size.X > r2TopLeftPosition.X && r1TopLeftPosition.X < r2TopLeftPosition.X + r2Size.X && r1TopLeftPosition.Y + r1Size.Y > r2TopLeftPosition.Y && r1TopLeftPosition.Y < r2TopLeftPosition.Y + r2Size.Y;
    }

    public static bool RectangleCircle(Vector2 rTopLeftPosition, Vector2 rSize, Vector2 cCenterPosition, float cRadius) {
      return new Vector2(
        cCenterPosition.X - Math.Max(rTopLeftPosition.X, Math.Min(cCenterPosition.X, rTopLeftPosition.X + rSize.X)),
        cCenterPosition.Y - Math.Max(rTopLeftPosition.Y, Math.Min(cCenterPosition.Y, rTopLeftPosition.Y + rSize.Y))
      ).Length() < cRadius;
    }

    public static bool RectangleLine(Vector2 rTopLeftPosition, Vector2 rSize, Vector2 lStartPosition, Vector2 lEndPosition) {
      throw new Exception("rectangle-line collision not implemented.");
    }

    public static bool CircleCircle(Vector2 c1CenterPosition, float c1Radius, Vector2 c2CenterPosition, float c2Radius) {
      return (c1CenterPosition - c2CenterPosition).Length() < c1Radius + c2Radius;
    }

    public static bool CircleLine(Vector2 cCenterPosition, float cRadius, Vector2 lStartPosition, Vector2 lEndPosition) {
      throw new Exception("circle-line collision not implemented.");
    }

    public static bool LineLine(Vector2 l1StartPosition, Vector2 l1EndPosition, Vector2 l2StartPosition, Vector2 l2EndPosition) {
      throw new Exception("line-line collision not implemented.");
    }

    public static bool RectanglePoint(Vector2 rTopLeftPosition, Vector2 rSize, Vector2 point) {
      return rTopLeftPosition.X < point.X && rTopLeftPosition.X + rSize.X > point.X && rTopLeftPosition.Y < point.Y && rTopLeftPosition.Y + rSize.Y > point.Y;
    }

    public static bool CirclePoint(Vector2 cCenterPosition, float cRadius, Vector2 point) {
      return (cCenterPosition - point).Length() < cRadius;
    }

    public static bool LinePoint(Vector2 lStartPosition, Vector2 lEndPosition, Vector2 point) {
      throw new Exception("line-point collision not implemented.");
    }

    public static bool PointPoint(Vector2 point1, Vector2 point2) {
      throw new Exception("Just use ==");
    }
  }
}