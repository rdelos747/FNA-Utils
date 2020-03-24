using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public partial class Node : IDisposable {

    public BoundingBox Bounds = new BoundingBox();

    public sealed class BoundingBox {

      /*
      Static texture used to draw all bounds.
      Should be initialized in Game constructor in
      user space.
      */
      public static Texture2D Texture;

      public Rectangle Rect = new Rectangle();
      public bool IsHidden = true;
      public Color Color = Color.Blue;
      public float Alpha = 0.5f;

      public BoundingBox() { }

      public void Draw(SpriteBatch spriteBatch, float x, float y) {
        if (!Rect.IsEmpty && !IsHidden) {
          spriteBatch.Draw(
            Texture,
            new Rectangle((int)(x + Rect.X), (int)(y + Rect.Y), Rect.Width, Rect.Height),
          Color * Alpha
          );
        }
      }
    }
  }
}