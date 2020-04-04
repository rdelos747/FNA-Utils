using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public enum CollisionType {
    Rectangle,
    Circle,
    Line
  }

  public partial class Node : IDisposable {

    public CollisionType CollisionType = CollisionType.Rectangle;
    public Vector2 BoundsOffset;
    public Vector2 Size;
    public float Radius;
    public Vector2 End;

    public bool Collides(Node other) {
      return Collides(other, new Vector2(0, 0));
    }

    public bool Collides(Node other, float x, float y) {
      return Collides(other, new Vector2(x, y));
    }

    public bool Collides(Node other, Vector2 offset) {
      if (other == null) {
        throw new Exception("Nodes passed to Node.Collides() must not be null.");
      }

      // TODO: Lines - how do Position/End interact with BoundsOffset and offset, and how do we draw lines
      switch (CollisionType) {
        case CollisionType.Rectangle:
          switch (other.CollisionType) {
            case CollisionType.Rectangle:
              return Collision.RectangleRectangle(Position + BoundsOffset + offset, Size, other.Position + other.BoundsOffset, other.Size);
            case CollisionType.Circle:
              return Collision.RectangleCircle(Position + BoundsOffset + offset, Size, other.Position + other.BoundsOffset + new Vector2(other.Radius), other.Radius);
            case CollisionType.Line:
              return Collision.RectangleLine(Position + BoundsOffset + offset, Size, other.Position, other.End);
          }
          return false;
        case CollisionType.Circle:
          switch (other.CollisionType) {
            case CollisionType.Rectangle:
              return Collision.RectangleCircle(other.Position + other.BoundsOffset, other.Size, Position + BoundsOffset + new Vector2(Radius) + offset, Radius);
            case CollisionType.Circle:
              return Collision.CircleCircle(Position + BoundsOffset + new Vector2(Radius) + offset, Radius, other.Position + other.BoundsOffset + new Vector2(other.Radius), other.Radius);
            case CollisionType.Line:
              return Collision.CircleLine(Position + BoundsOffset + new Vector2(Radius) + offset, Radius, other.Position, other.End);
          }
          return false;
        case CollisionType.Line:
          switch (other.CollisionType) {
            case CollisionType.Rectangle:
              return Collision.RectangleLine(other.Position + other.BoundsOffset, other.Size, Position, End);
            case CollisionType.Circle:
              return Collision.CircleLine(other.Position + other.BoundsOffset + new Vector2(other.Radius), other.Radius, Position, End);
            case CollisionType.Line:
              return Collision.LineLine(Position, End, other.Position, other.End);
          }
          return false;
      }

      return false;
    }

    public bool PointInBounds(Vector2 p) {
      switch (CollisionType) {
        case CollisionType.Rectangle:
          return Collision.RectanglePoint(Position + BoundsOffset, Size, p);
        case CollisionType.Circle:
          return Collision.CirclePoint(Position + BoundsOffset + new Vector2(Radius), Radius, p);
        case CollisionType.Line:
          return Collision.LinePoint(Position, End, p);
      }

      return false;
    }
  }
}