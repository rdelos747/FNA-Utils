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
    protected int spriteWidth { get; private set; }
    protected int spriteHeight { get; private set; }
    protected Rectangle spriteClip;
    protected Vector2 center = Vector2.Zero;
    public float spriteRotation = 0f;
    public float spriteScale = 1f;
    public bool isHidden = false;

    // sprite sheet animation
    protected SpritesheetAnimation spriteSheetAnimation;
    protected int currentFrame = -1;

    // position vars
    public Vector2 position;
    public float direction = 0f;

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

    public virtual void update() { }

    public virtual void draw(SpriteBatch spriteBatch) {
      if (image == null || isHidden) return;
      updateSpriteDetails();

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
    }

    // lifecyle methods
    private void updateSpriteDetails() {
      // if the animation is set, update currentFrame and spriteClip to be in sync
      if (spriteSheetAnimation != null) {
        spriteSheetAnimation.run();
        currentFrame = spriteSheetAnimation.getFrame();

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
        throw new System.NullReferenceException("Cannot kill GameObject - renderParent is null");
      }
      renderParent.removeObject(this);
      renderParent = null;
    }
  }
}