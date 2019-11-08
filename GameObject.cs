using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  public class GameObject {

    // sprite vars
    private Texture2D image; // used as a single sprite, or a spritesheet
    protected int imageWidth { get; private set; }
    protected int imageHeight { get; private set; }
    protected int spriteWidth { get; private set; }
    protected int spriteHeight { get; private set; }
    protected Rectangle spriteClip;

    public float spriteRotation = 0f;
    public float spriteScale = 1f;

    protected Animation spriteSheetAnimation;
    private int spriteSheetCols = 1;
    private int spriteSheetRows = 1;

    // position vars
    public Vector2 position;
    public float direction = 0f;
    protected Vector2 center = Vector2.Zero;

    // other vars
    public float layerDepth = 0.5f;
    public Color drawColor = Color.White;

    public GameObject() { }

    public virtual void init() { }

    public virtual void load(ContentManager content) {
      initializeSpriteDimensions();
    }

    public virtual void update() {
      if (spriteSheetAnimation != null) spriteSheetAnimation.run();
    }

    public virtual void draw(SpriteBatch spriteBatch) {
      if (image == null) return;

      spriteBatch.Draw(
        image,
        position,
        getClip(),
        drawColor,
        spriteRotation,
        center,
        spriteScale,
        SpriteEffects.None,
        layerDepth
      );
    }

    protected void setImage(Texture2D newImage) {
      if (image != null) return;
      image = newImage;
    }

    protected void setSpriteSheet(Texture2D newImage, int cols, int rows) {
      if (image != null) return;
      image = newImage;
      spriteSheetCols = cols;
      spriteSheetRows = rows;
    }

    void initializeSpriteDimensions() {
      imageWidth = image.Width;
      imageHeight = image.Height;
      spriteWidth = imageWidth / spriteSheetCols;
      spriteHeight = imageHeight / spriteSheetRows;
      spriteClip = new Rectangle(0, 0, spriteWidth, spriteHeight);
    }

    Rectangle getClip() {
      if (spriteSheetAnimation == null) return spriteClip;

      int frame = spriteSheetAnimation.getFrame();
      int clipX = (frame % spriteSheetCols) * spriteWidth;
      int clipY = (frame / spriteSheetCols) * spriteHeight;
      return new Rectangle(clipX, clipY, spriteWidth, spriteHeight);
    }
  }
}