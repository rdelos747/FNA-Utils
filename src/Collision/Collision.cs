using Microsoft.Xna.Framework;
using System;

namespace Utils
{
  public enum PointSectors
  {
    Center = 0,
    Top = 1,
    Bottom = 2,
    TopLeft = 9,
    TopRight = 5,
    Left = 8,
    Right = 4,
    BottomLeft = 10,
    BottomRight = 6
  };


  public static class Collision
  {

    public static bool RectangleRectangle(Vector2 r1TopLeftPosition, Size r1Size, Vector2 r2TopLeftPosition, Size r2Size)
    {
      return r1TopLeftPosition.X + r1Size.Width > r2TopLeftPosition.X && r1TopLeftPosition.X < r2TopLeftPosition.X + r2Size.Width && r1TopLeftPosition.Y + r1Size.Height > r2TopLeftPosition.Y && r1TopLeftPosition.Y < r2TopLeftPosition.Y + r2Size.Height;
    }

    public static bool RectangleCircle(Vector2 rTopLeftPosition, Size rSize, Vector2 cCenterPosition, float cRadius)
    {
      return new Vector2(
        cCenterPosition.X - Math.Max(rTopLeftPosition.X, Math.Min(cCenterPosition.X, rTopLeftPosition.X + rSize.Width)),
        cCenterPosition.Y - Math.Max(rTopLeftPosition.Y, Math.Min(cCenterPosition.Y, rTopLeftPosition.Y + rSize.Height))
      ).Length() < cRadius;
    }

    public static bool RectangleLine(float rX, float rY, float rW, float rH, Vector2 lineFrom, Vector2 lineTo)
    {
      //throw new Exception("rectangle-line collision not implemented.");
      PointSectors fromSector = Collision.GetSector(rX, rY, rW, rH, lineFrom);
      PointSectors toSector = Collision.GetSector(rX, rY, rW, rH, lineTo);

      if (fromSector == PointSectors.Center || toSector == PointSectors.Center)
        return true;
      else if ((fromSector & toSector) != 0)
        return false;
      else
      {
        PointSectors both = fromSector | toSector;

        //Do line checks against the edges
        Vector2 edgeFrom;
        Vector2 edgeTo;

        if ((both & PointSectors.Top) != 0)
        {
          edgeFrom = new Vector2(rX, rY);
          edgeTo = new Vector2(rX + rW, rY);
          if (Collision.LineCheck(edgeFrom, edgeTo, lineFrom, lineTo))
            return true;
        }

        if ((both & PointSectors.Bottom) != 0)
        {
          edgeFrom = new Vector2(rX, rY + rH);
          edgeTo = new Vector2(rX + rW, rY + rH);
          if (Collision.LineCheck(edgeFrom, edgeTo, lineFrom, lineTo))
            return true;
        }

        if ((both & PointSectors.Left) != 0)
        {
          edgeFrom = new Vector2(rX, rY);
          edgeTo = new Vector2(rX, rY + rH);
          if (Collision.LineCheck(edgeFrom, edgeTo, lineFrom, lineTo))
            return true;
        }

        if ((both & PointSectors.Right) != 0)
        {
          edgeFrom = new Vector2(rX + rW, rY);
          edgeTo = new Vector2(rX + rW, rY + rH);
          if (Collision.LineCheck(edgeFrom, edgeTo, lineFrom, lineTo))
            return true;
        }
      }

      return false;
    }

    public static bool LineCheck(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
    {
      Vector2 b = a2 - a1;
      Vector2 d = b2 - b1;
      float bDotDPerp = b.X * d.Y - b.Y * d.X;

      // if b dot d == 0, it means the lines are parallel so have infinite intersection points
      if (bDotDPerp == 0)
        return false;

      Vector2 c = b1 - a1;
      float t = (c.X * d.Y - c.Y * d.X) / bDotDPerp;
      if (t < 0 || t > 1)
        return false;

      float u = (c.X * b.Y - c.Y * b.X) / bDotDPerp;
      if (u < 0 || u > 1)
        return false;

      return true;
    }


    public static bool CircleCircle(Vector2 c1CenterPosition, float c1Radius, Vector2 c2CenterPosition, float c2Radius)
    {
      return (c1CenterPosition - c2CenterPosition).Length() < c1Radius + c2Radius;
    }

    public static bool CircleLine(Vector2 cCenterPosition, float cRadius, Vector2 lStartPosition, Vector2 lEndPosition)
    {
      throw new Exception("circle-line collision not implemented.");
    }

    public static bool LineLine(Vector2 l1StartPosition, Vector2 l1EndPosition, Vector2 l2StartPosition, Vector2 l2EndPosition)
    {
      Vector2 s1 = l1EndPosition - l1StartPosition;
      Vector2 s2 = l2EndPosition - l2StartPosition;
      float s = (-s1.Y * (l1StartPosition.X - l2StartPosition.X) + s1.X * (l1StartPosition.Y - l2StartPosition.Y)) / (-s2.X * s1.Y + s1.X * s2.Y);
      float t = (s2.X * (l1StartPosition.Y - l2StartPosition.Y) - s2.Y * (l1StartPosition.X - l2StartPosition.X)) / (-s2.X * s1.Y + s1.X * s2.Y);
      return s >= 0 && s <= 1 && t >= 0 && t <= 1;
    }

    public static bool RectanglePoint(float rX, float rY, float rW, float rH, Vector2 point)
    {
      // return rTopLeftPosition.X < point.X && rTopLeftPosition.X + rSize.Width > point.X && rTopLeftPosition.Y < point.Y && rTopLeftPosition.Y + rSize.Height > point.Y;
      return point.X >= rX && point.Y >= rY && point.X < rX + rW && point.Y < rY + rH;
    }

    public static bool CirclePoint(Vector2 cCenterPosition, float cRadius, Vector2 point)
    {
      return (cCenterPosition - point).Length() < cRadius;
    }

    public static bool LinePoint(Vector2 lStartPosition, Vector2 lEndPosition, Vector2 point)
    {
      throw new Exception("line-point collision not implemented.");
    }

    public static bool PointPoint(Vector2 point1, Vector2 point2)
    {
      throw new Exception("Just use ==");
    }

    /*
    *  Bitflags and helpers for using the Cohen–Sutherland algorithm
    *  http://en.wikipedia.org/wiki/Cohen%E2%80%93Sutherland_algorithm
    *  
    *  Sector bitflags:
    *      1001  1000  1010
    *      0001  0000  0010
    *      0101  0100  0110
    */

    public static PointSectors GetSector(Rectangle rect, Vector2 point)
    {
      PointSectors sector = PointSectors.Center;

      if (point.X < rect.Left)
        sector |= PointSectors.Left;
      else if (point.X >= rect.Right)
        sector |= PointSectors.Right;

      if (point.Y < rect.Top)
        sector |= PointSectors.Top;
      else if (point.Y >= rect.Bottom)
        sector |= PointSectors.Bottom;

      return sector;
    }

    public static PointSectors GetSector(float rX, float rY, float rW, float rH, Vector2 point)
    {
      PointSectors sector = PointSectors.Center;

      if (point.X < rX)
        sector |= PointSectors.Left;
      else if (point.X >= rX + rW)
        sector |= PointSectors.Right;

      if (point.Y < rY)
        sector |= PointSectors.Top;
      else if (point.Y >= rY + rH)
        sector |= PointSectors.Bottom;

      return sector;
    }
  }
}