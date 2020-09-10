using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public abstract class Collider {
    public Size Size = new Size(0, 0);
    public Vector2 Position = new Vector2(0, 0);
    public Vector2 Origin = new Vector2(0, 0);
    public Color Color = Color.Red;
    public float Alpha = 0.5f;
    private Node Node;


    public Collider(Node node = null) {
      Node = node;
    }

    public abstract bool Collides(Hitbox other, Vector2 offset = new Vector2());
    public abstract void Render();
    // public abstract bool Collides(Circlebox circlebox);

    public abstract float Left { get; }
    public abstract float Right { get; }
    public abstract float Top { get; }
    public abstract float Bottom { get; }

    public bool Collides(Collider other, Vector2 offset = new Vector2()) {
      if (other is Hitbox) {
        return Collides(other as Hitbox, offset);
      }
      else {
        throw new Exception("Collisions against the collider type are not implemented!");
      }
    }

    public void CenterOrigin() {
      Origin = new Vector2(Size.Width / 2, Size.Height / 2);
    }

    public Vector2 WorldPosition {
      get {
        if (Node != null) {
          return Node.Position + Position;
        }
        else {
          return Position;
        }
      }
    }

    public float WorldLeft {
      get {
        if (Node != null) {
          return Node.Position.X + Left;
        }
        else {
          return Left;
        }
      }
    }

    public float WorldRight {
      get {
        if (Node != null) {
          return Node.Position.X + Right;
        }
        else {
          return Right;
        }
      }
    }

    public float WorldTop {
      get {
        if (Node != null) {
          return Node.Position.Y + Top;
        }
        else {
          return Top;
        }
      }
    }

    public float WorldBottom {
      get {
        if (Node != null) {
          return Node.Position.Y + Bottom;
        }
        else {
          return Bottom;
        }
      }
    }
  }
}