using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public enum CollisionType {
    Box,
    Circle
  }

  public partial class Node : IDisposable {

    public Rectangle Bounds = new Rectangle();
    public CollisionType CollisionType = CollisionType.Box;
    // public Radius Rad = new Radius
    // public Line Line = new Line

    public bool Collides(Node other, float x, float y) {
      return Collides(other, new Vector2(x, y));
    }

    public bool Collides(Node other, Vector2 offset) {
      if (other == null) {
        // should we do this, or should we throw an error??
        return false;
      }

      if (CollisionType == CollisionType.Box && other.CollisionType == CollisionType.Box) {

        Rectangle r1 = new Rectangle(
          (int)((Position.X + Bounds.X) + offset.X),
          (int)((Position.Y + Bounds.Y) + offset.Y),
          Bounds.Width,
          Bounds.Height);

        Rectangle r2 = new Rectangle(
          (int)((other.Position.X + other.Bounds.X)),
          (int)((other.Position.Y + other.Bounds.Y)),
          other.Bounds.Width,
          other.Bounds.Height);

        return Collision.Box(r1, r2);
      }
      else if (CollisionType == CollisionType.Circle && other.CollisionType == CollisionType.Circle) {
        // return Collision.Circle(Position, Radius, other.Position, other.Radius)
        return false;
      }

      return false;
    }

    public bool PointInBounds(Vector2 p) {
      if (Bounds.IsEmpty) {
        return false;
      }

      float cornerX = Position.X - Bounds.X;
      float cornerY = Position.Y - Bounds.Y;

      return p.X >= cornerX && p.X <= cornerX + Bounds.Width && p.Y >= cornerY && p.Y <= cornerY + Bounds.Height;
    }
  }





  public sealed class BoundingBox {
    /*
    Static texture used to draw all bounds.
    Should be initialized in Game constructor in
    user space.
    */
    //public static Texture2D Texture;


    // public bool IsHidden = true;
    // public Color Color = Color.Blue;
    // public float Alpha = 0.5f;
    // public float DrawDepth = 0.0f;


    public Rectangle Rect = new Rectangle();
    // public Radius Rad = new Radius
    // public Line Line = new Line

    public BoundingBox() { }



    // public void Draw(SpriteBatch spriteBatch, float x, float y) {
    //   if (!Rect.IsEmpty && !IsHidden) {
    //     spriteBatch.Draw(
    //       Texture,
    //       new Rectangle((int)(x + Rect.X), (int)(y + Rect.Y), Rect.Width, Rect.Height),
    //       null,
    //       Color * Alpha,
    //       0.0f,
    //       new Vector2(0, 0),
    //       SpriteEffects.None,
    //       DrawDepth
    //     );
    //   }
    // }
  }
}