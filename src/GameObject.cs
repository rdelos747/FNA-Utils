using System;
//using System.Drawing;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  public class GameObject {
    private Renderer renderParent;
    // sprite / graphical vars
    private Texture2D image; // used as a single sprite, or a spritesheet
    private int spriteSheetCols = 1;
    private int spriteSheetRows = 1;

    protected int imageWidth { get; private set; }
    protected int imageHeight { get; private set; }
    public int spriteWidth { get; private set; }
    public int spriteHeight { get; private set; }

    protected Rectangle spriteClip;
    protected Vector2 center = Vector2.Zero;
    public float spriteRotation = 0f;
    public float spriteScale = 1f;

    public bool isHidden = false;

    // sprite sheet animation
    //protected SpritesheetAnimation spriteSheetAnimation;
    protected Animation animation;
    protected int currentFrame = -1;

    // position vars
    public float x;
    public float y;
    public float direction = 0f;
    public Rectangle bounds;
    public bool showBounds = false;
    protected int collisionLayer = 0;

    // other vars
    public float layerDepth = 0.5f;
    public Color drawColor = Color.White;


    public GameObject() { }

    // virtual methods
    public virtual void init(Renderer r) {
      renderParent = r;
    }

    public virtual void load(ContentManager content) {
      initializeSpriteDimensions();
    }

    public virtual void draw(SpriteBatch spriteBatch, GameTime gameTime) {
      if (image == null || isHidden) return;
      updateSpriteDetails(gameTime);

      Vector2 position = new Vector2(x, y);

      spriteBatch.Draw(
        image,
        position,
        spriteClip,
        drawColor,
        spriteRotation,
        center,
        spriteScale,
        SpriteEffects.None,
        layerDepth
      );
    }

    // image initializers
    protected void setImage(Texture2D newImage) {
      if (image != null) return; // if image already set, bounce
      image = newImage;
    }

    protected void setSpriteSheet(Texture2D newImage, int cols, int rows) {
      if (image != null) return; // if image already set, bounce
      image = newImage;
      spriteSheetCols = cols;
      spriteSheetRows = rows;
    }

    private void initializeSpriteDimensions() {
      if (image == null) return;

      imageWidth = image.Width;
      imageHeight = image.Height;
      spriteWidth = imageWidth / spriteSheetCols;
      spriteHeight = imageHeight / spriteSheetRows;
      spriteClip = new Rectangle(0, 0, spriteWidth, spriteHeight);

      bounds = new Rectangle(0, 0, spriteWidth, spriteHeight);
    }

    // lifecyle methods
    private void updateSpriteDetails(GameTime gameTime) {
      // if the animation is set, update currentFrame and spriteClip to be in sync
      //if (spriteSheetAnimation != null) {
      if (animation != null) {
        //spriteSheetAnimation.run();
        //currentFrame = spriteSheetAnimation.getFrame();
        currentFrame = (int)animation.update(gameTime);

        int clipX = (currentFrame % spriteSheetCols) * spriteWidth;
        int clipY = (currentFrame / spriteSheetCols) * spriteHeight;
        spriteClip = new Rectangle(clipX, clipY, spriteWidth, spriteHeight);
      }
      // if current frame is set, update spriteClip to be in sync
      else if (currentFrame >= 0) {
        int clipX = (currentFrame % spriteSheetCols) * spriteWidth;
        int clipY = (currentFrame / spriteSheetCols) * spriteHeight;
        spriteClip = new Rectangle(clipX, clipY, spriteWidth, spriteHeight);
      }
      // otherwise, nothing is set. User is free to use spriteClip in this case
    }

    // public methods
    public void removeFromRenderer() {
      if (renderParent == null) {
        throw new System.NullReferenceException("Cannot kill GameObject - renderParent is null. GameObject was probably never added via addObject()");
      }
      renderParent.removeObject(this);
      renderParent = null;
    }

    public bool pointInBounds(float pX, float pY, float offX = 0, float offY = 0) {
      if (bounds == null) {
        return false;
      }

      float cornerX = (x - bounds.X) + offX;
      float cornerY = (y - bounds.Y) + offY;

      return pX >= cornerX && pX <= cornerX + bounds.Width && pY >= cornerY && pY <= cornerY + bounds.Height;
    }

    public bool objectInBounds(GameObject obj, float offX = 0, float offY = 0, int cl = 0) {
      if (bounds == null || obj == null || obj.bounds == null || isHidden == true || obj.collisionLayer != cl) {
        return false;
      }

      Rectangle r1 = new Rectangle(
        (int)((x + bounds.X) + offX),
        (int)((y + bounds.Y) + offY),
        bounds.Width,
        bounds.Height);
      Rectangle r2 = new Rectangle(
        (int)((obj.x + obj.bounds.X)),
        (int)((obj.y + obj.bounds.Y)),
        obj.bounds.Width,
        obj.bounds.Height);

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