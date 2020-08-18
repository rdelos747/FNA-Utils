using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public class Sprite : Node {
    public static Texture2D SystemRect;

    // sprite / graphical vars
    public Texture2D Image; // used as a single sprite, or a spritesheet

    private int SheetCols = 1;
    private int SheetRows = 1;

    protected int ImageWidth { get; private set; }
    protected int ImageHeight { get; private set; }
    public Vector2 ImageOrigin = new Vector2();
    public int SpriteWidth;
    public int SpriteHeight;

    protected Rectangle ImageClip;
    public float Rotation = 0f;
    public float Scale = 1f;
    public float Direction = 0f;
    //public float DrawDepth = 0;
    public Color Color = Color.White;
    public float Alpha = 1;

    // sprite sheet animation
    public Animation Animation;
    private int _currentFrame = -1;
    public int CurrentFrame {
      get => _currentFrame;
      set {
        _currentFrame = value;
        if (_currentFrame >= 0) {
          int clipX = (_currentFrame % SheetCols) * SpriteWidth;
          int clipY = (_currentFrame / SheetCols) * SpriteHeight;
          ImageClip = new Rectangle(clipX, clipY, SpriteWidth, SpriteHeight);
        }
      }
    }

    public Sprite() { }

    protected override void Dispose(bool disposing) {
      if (disposing == true) {
        // Do not dispose of images or other data
        // that might be managed by the ContentManager,
        // as it keeps a cache of textures and other
        // stuff. Eg, dont do:
        // image.Dispose(); 
      }
    }

    public override void Draw(SpriteBatch spriteBatch, float lastX, float lastY) {
      if (IsHidden) return;

      Vector2 position = new Vector2(lastX + Position.X, lastY + Position.Y);

      if (Image != null) {
        spriteBatch.Draw(
          Image,
          position,
          ImageClip,
          Color,
          Rotation,
          ImageOrigin,
          Scale,
          SpriteEffects.None,
          0
        //DrawDepth
        );
      }
      else {
        Vector2 offset = position - ImageOrigin;

        spriteBatch.Draw(
          SystemRect,
          new Rectangle((int)(offset.X), (int)(offset.Y), SpriteWidth, SpriteHeight),
          null,
          Color * Alpha,
          Rotation,
          new Vector2(0, 0),
          SpriteEffects.None,
          0
       //DrawDepth
       );
      }

      base.Draw(spriteBatch, lastX, lastY);
    }

    // image initializers
    public void SetImage(Texture2D newImage) {
      if (Image != null) return; // if image already set, bounce
      Image = newImage;
      InitializeSpriteDimensions();
    }

    public void SetSpriteSheet(SpriteSheet sheet) {
      if (Image != null) return; // if image already set, bounce
      Image = sheet.Texture;
      SheetCols = sheet.Cols;
      SheetRows = sheet.Rows;
      InitializeSpriteDimensions();
    }

    private void InitializeSpriteDimensions() {
      if (Image == null) return;

      ImageWidth = Image.Width;
      ImageHeight = Image.Height;
      SpriteWidth = ImageWidth / SheetCols;
      SpriteHeight = ImageHeight / SheetRows;
      ImageClip = new Rectangle(0, 0, SpriteWidth, SpriteHeight);

      if (Size == null || (Size.Width == 0 && Size.Height == 0)) {
        Size = new Size(SpriteWidth, SpriteHeight);
      }
    }

    public void Animate(GameTime gameTime) {
      // if the animation is set, update currentFrame and spriteClip to be in sync
      if (Animation != null) {
        CurrentFrame = (int)Animation.Update(gameTime);
      }
    }
  }
}