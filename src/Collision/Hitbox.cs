using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public class Hitbox : Collider {

    public Hitbox(Size size, Vector2 position, bool center = false, Node node = null) : base(node) {
      Size = size;
      Position = position;
      if (center) {
        CenterOrigin();
      }
    }

    public Hitbox(Size size, bool center = false, Node node = null) :
    this(size, new Vector2(), center, node) { }

    public Hitbox(float width = 0, float height = 0, bool center = false, Node node = null) :
    this(new Size(width, height), new Vector2(), center, node) { }


    public override float Left {
      get {
        return Position.X - Origin.X;
      }
    }

    public override float Right {
      get {
        return Position.X + (Size.Width - Origin.X);
      }
    }

    public override float Top {
      get {
        return Position.Y - Origin.Y;
      }
    }

    public override float Bottom {
      get {
        return Position.Y + (Size.Height - Origin.Y);
      }
    }

    public override bool Collides(Hitbox other, Vector2 offset = new Vector2()) {
      return (
        // WorldLeft + offset.X < other.WorldRight &&
        // WorldRight + offset.X > other.WorldLeft &&
        // WorldBottom + offset.Y > other.WorldTop &&
        // WorldTop + offset.Y < other.WorldBottom
        WorldLeft < other.WorldRight + offset.X &&
        WorldRight > other.WorldLeft + offset.X &&
        WorldBottom > other.WorldTop + offset.Y &&
        WorldTop < other.WorldBottom + offset.Y
      );
    }

    public override void Render() {
      Engine.SpriteBatch.Draw(
        Engine.SystemRect,
        new Rectangle(
          (int)(WorldLeft),
          (int)(WorldTop),
          (int)Size.Width,
          (int)Size.Height
        ),
        null,
        Color * Alpha,
        0,
        new Vector2(0, 0),
        SpriteEffects.None,
        0
      );
    }
  }
}