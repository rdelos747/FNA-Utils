using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public class GameObject : Node {
    // sprite / graphical vars
    private Texture2D image; // used as a single sprite, or a spritesheet
    private int spriteSheetCols = 1;
    private int spriteSheetRows = 1;

    protected int imageWidth { get; private set; }
    protected int imageHeight { get; private set; }
    protected Vector2 ImageOrigin = new Vector2();
    public int spriteWidth { get; private set; }
    public int spriteHeight { get; private set; }

    protected Rectangle spriteClip;
    public float spriteRotation = 0f;
    public float spriteScale = 1f;

    //public bool isHidden = false;

    // sprite sheet animation
    protected Animation animation;
    private int _currentFrame = -1;
    protected int currentFrame {
      get => _currentFrame;
      set {
        _currentFrame = value;
        if (_currentFrame >= 0) {
          int clipX = (_currentFrame % spriteSheetCols) * spriteWidth;
          int clipY = (_currentFrame / spriteSheetCols) * spriteHeight;
          spriteClip = new Rectangle(clipX, clipY, spriteWidth, spriteHeight);
        }
      }
    }

    // position vars
    public float direction = 0f;
    protected int collisionLayer = 0;

    // other vars
    public float drawDepth = 0;
    public Color drawColor = Color.White;


    public GameObject() { }

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
      Vector2 position = new Vector2(lastX + X, lastY + Y);
      if (IsHidden) return;

      if (image != null) {
        spriteBatch.Draw(
          image,
          position,
          spriteClip,
          drawColor,
          spriteRotation,
          ImageOrigin,
          spriteScale,
          SpriteEffects.None,
          drawDepth
        );
      }

      base.Draw(spriteBatch, lastX, lastY);
    }

    // image initializers
    protected void setImage(Texture2D newImage) {
      if (image != null) return; // if image already set, bounce
      image = newImage;
      initializeSpriteDimensions();
    }

    protected void setSpriteSheet(Texture2D newImage, int cols, int rows) {
      if (image != null) return; // if image already set, bounce
      image = newImage;
      spriteSheetCols = cols;
      spriteSheetRows = rows;
      initializeSpriteDimensions();
    }

    private void initializeSpriteDimensions() {
      if (image == null) return;

      imageWidth = image.Width;
      imageHeight = image.Height;
      spriteWidth = imageWidth / spriteSheetCols;
      spriteHeight = imageHeight / spriteSheetRows;
      spriteClip = new Rectangle(0, 0, spriteWidth, spriteHeight);

      if (Bounds.Rect.IsEmpty) {
        Bounds.Rect = new Rectangle(0, 0, spriteWidth, spriteHeight);
      }
    }

    protected void animate(GameTime gameTime) {
      // if the animation is set, update currentFrame and spriteClip to be in sync
      if (animation != null) {
        currentFrame = (int)animation.update(gameTime);
      }
    }

    public bool pointInBounds(float pX, float pY, float offX = 0, float offY = 0) {
      if (Bounds.Rect.IsEmpty) {
        return false;
      }

      float cornerX = (X - Bounds.Rect.X) + offX;
      float cornerY = (Y - Bounds.Rect.Y) + offY;

      return pX >= cornerX && pX <= cornerX + Bounds.Rect.Width && pY >= cornerY && pY <= cornerY + Bounds.Rect.Height;
    }

    public bool objectInBounds(GameObject obj, float offX = 0, float offY = 0, int cl = 0) {
      if (Bounds.Rect.IsEmpty || obj == null || obj.Bounds == null || IsHidden == true || obj.collisionLayer != cl) {
        return false;
      }

      Rectangle r1 = new Rectangle(
        (int)((X + Bounds.Rect.X) + offX),
        (int)((Y + Bounds.Rect.Y) + offY),
        Bounds.Rect.Width,
        Bounds.Rect.Height);
      Rectangle r2 = new Rectangle(
        (int)((obj.X + obj.Bounds.Rect.X)),
        (int)((obj.Y + obj.Bounds.Rect.Y)),
        obj.Bounds.Rect.Width,
        obj.Bounds.Rect.Height);

      return r1.Intersects(r2);
    }

    public bool objectInBounds<T>(List<T> objs, float offX = 0, float offY = 0, int cl = 0) where T : GameObject, new() {
      for (int i = 0; i < objs.Count; i++) {
        if (objectInBounds(objs[i], offX, offY, cl)) {
          return true;
        }
      }
      return false;
    }

    public bool objectInBounds<T>(List<T> objs, out T value, float offX = 0, float offY = 0, int cl = 0) where T : GameObject, new() {
      for (int i = 0; i < objs.Count; i++) {
        if (objectInBounds(objs[i], offX, offY, cl)) {
          value = objs[i];
          return true;
        }
      }
      value = null;
      return false;
    }
  }
}