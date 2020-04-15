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

    public static bool RectangleRectangle(Vector2 r1TopLeftPosition, Size r1Size, Vector2 r2TopLeftPosition, Size r2Size) {
      return r1TopLeftPosition.X + r1Size.Width > r2TopLeftPosition.X && r1TopLeftPosition.X < r2TopLeftPosition.X + r2Size.Width && r1TopLeftPosition.Y + r1Size.Height > r2TopLeftPosition.Y && r1TopLeftPosition.Y < r2TopLeftPosition.Y + r2Size.Height;
    }

    public static bool RectangleCircle(Vector2 rTopLeftPosition, Size rSize, Vector2 cCenterPosition, float cRadius) {
      return new Vector2(
        cCenterPosition.X - Math.Max(rTopLeftPosition.X, Math.Min(cCenterPosition.X, rTopLeftPosition.X + rSize.Width)),
        cCenterPosition.Y - Math.Max(rTopLeftPosition.Y, Math.Min(cCenterPosition.Y, rTopLeftPosition.Y + rSize.Height))
      ).Length() < cRadius;
    }

    public static bool RectangleLine(Vector2 rTopLeftPosition, Size rSize, Vector2 lStartPosition, Vector2 lEndPosition) {
      throw new Exception("rectangle-line collision not implemented.");
    }

    public static bool CircleCircle(Vector2 c1CenterPosition, float c1Radius, Vector2 c2CenterPosition, float c2Radius) {
      return (c1CenterPosition - c2CenterPosition).Length() < c1Radius + c2Radius;
    }

    public static bool CircleLine(Vector2 cCenterPosition, float cRadius, Vector2 lStartPosition, Vector2 lEndPosition) {
      throw new Exception("circle-line collision not implemented.");
    }

    public static bool LineLine(Vector2 l1StartPosition, Vector2 l1EndPosition, Vector2 l2StartPosition, Vector2 l2EndPosition) {
      Vector2 s1 = l1EndPosition - l1StartPosition;
      Vector2 s2 = l2EndPosition - l2StartPosition;
      float s = (-s1.Y * (l1StartPosition.X - l2StartPosition.X) + s1.X * (l1StartPosition.Y - l2StartPosition.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
      float t = (s2.X * (l1StartPosition.Y - l2StartPosition.Y) - s2.Y * (l1StartPosition.X - l2StartPosition.X)) / (-s2.X * s1.Y + s1.X * s2.Y);
      return s >= 0 && s <= 1 && t >= 0 && t <= 1;
    }

    public static bool RectanglePoint(Vector2 rTopLeftPosition, Size rSize, Vector2 point) {
      return rTopLeftPosition.X < point.X && rTopLeftPosition.X + rSize.Width > point.X && rTopLeftPosition.Y < point.Y && rTopLeftPosition.Y + rSize.Height > point.Y;
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