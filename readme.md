# FNA Engine

Library of useful classes and utilies for FNA.

# Installation

1. Clone this repo, preferably somewhere with easy access to your game and FNA.
2. In `engine.csproj` change the FNA include reference to wherever you've placed FNA.

```
<!-- Reference FNA project -->
  <ItemGroup>
    <ProjectReference Include="path.to.FNA" />
  </ItemGroup>
```

3. run the Restore build task
4. email us when you realize we forgot something

# Using this in your game

Within your games `.csproj` file, add the following:

```
  <ItemGroup>
    <ProjectReference Include="path.to.this.repo.engine.csproj" />
  </ItemGroup>
```

Then, use `using Engine;` along with the rest of your includes within your .cs files.

Here is an [example](https://github.com/rdelos747/FNA-Test-Game).

# Notes

We mainly develop via VS Code on OS X, so our setup or instructions may not translate over to Visual Studio or another IDE.

This project was initially bootstrapped with [this template](https://github.com/TheSpydog/fna_vscode_template), which gave us the initial .vscode functions. We don't author any of it, but we may fork it soon?

## Also Notes

We very new to FNA, monogame, and C# in general.

# Code Reference

See below for different classes and utilites within the engine :)


Note: the term `readonly` is used loosely, and is meant to specifiy the property `{ get; private set; }`

# Renderer

`public Renderer()`

- Manages rendering of GameObjects. This class extends XNA's `Game` class, and mainly takes care of the boilerplate around loading and drawing objects. 

- While `Renderer` contains a private update method, it does not call the `update()` methods of GameObjects added with `addObject()`. Instead, `Renderer`'s virtual method `updateObjects` is meant be overriden in the derived class, and the user is responsible for grouping/ looping over objects there.

- The user does not, however, need to manually call GameObject's draw methods - `Renderer` does this automatically. 

### Public vars:

`readonly int count`

- Number of `GameObject`s the `Renderer` currently holds.

### Virtual Methods:

`protected void Initialize()`

`protected void updateObjects()`

### Public Methods:

`public int addObject(GameObject obj)`

- Adds a `GameObject` to be rendered.

`public void removeObject(GameObject obj)`

- Removes a `GameObjects` from the `Renderer`s list of GameObjects. This is public but should be avoided from outside the engine. `GameObject.removeFromRenderer()` is preferred.

# GameObject

`public GameObject()`

- Displays a sprite on screen. The image provided can function as the whole sprite, or as a sprite sheet.

### Position vars:

`rublic Vector2 position`

`rublic float direction`

### Sprite Dimension vars:

`readonly imageWidth`

`readonly imageHeight`

`readonly spriteWidth`

`readonly spriteHeight`

- `imageWidth` and `imageHeight` specify the dimensions of the image provided to the `GameObject`. If the image is supplied as a spritesheet (see `setSpriteSheet()` below), `spriteWidth` and `spriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions.

`public float spriteRotation`

`public float spriteScale`

### Sprite sheet manipulation vars:

If a GameObject's image is supplied as a sprite sheet, there are a few ways to specify which frame or section of the sheet to show. The following are specified in order of precidence (if one is set, the ones below are automatically updated to match).

`protected Animation spriteSheetAnimation`

- GameObject will automatically run the supplied `SpritesheetAnimation` (but it can still be paused/ restarted in the base class), and will display the sprite based on the animation's current frame. `GameObject.currentFrame` will be updated to this value, and `GameObject.spriteClip` will represent the bounds of the sprite on the sheet. User setting either in this case will have no effect, as these values are computed every frame.

`protected int currentFrame`

- Sets the sprite to the current frame on the sheet. `GameObject.spriteClip` will automatically represent these bounds on the sheet.

`protected Rectangle spriteClip`

- The area of the spritesheet to draw, initialized to the top left frame on the sheet. If `spriteSheetAnimation` or `currentFrame` have not been set, this can be used to display any rectangular section of the sheet. This is useful when a spritesheet contains sprites of different sizes.

### Other vars:

`public float layerDepth`

`public Color drawColor`

`public bool isHidden`

### Virtual Methods:

`public virtual void init()`

- Init runs once before the main loop of the engine. This is a good place to do any major setup within the derived objects.

`public virtual void load(ContentManager content)`

- Load is similar to Init, except it has access to the games content manager. Load should be used for setting graphical information of the object, like calling `setImage()` or `setSpriteSheet()`.

`public virtual void update()`

- Called every frame. Place main object logic here.

`public virtual void draw(SpriteBatch spriteBatch)`

- Similar to update, but requires a `SpriteBatch` to render the object's image to the view.

### Image/ Sprite Initializers:

Below are image intializers meant to set a GameObject's sprite, meant to be called within the derived class's load method. Only one should be used per GameObject, as a GameObject only has one internal image member. Once an image is set, it cannot be removed or reset.

`protected void setImage(Texture2D newImage)`

- Sets the GameObject's image as a single sprite. Drawing the GameObject will display the entire image, unless a `spriteClip` is specified. Using `spriteSheetAnimation` or `currentFrame` will have no effect.

`protected void setSpriteSheet(Texture2D newImage, int cols, int rows)`

- Sets the GameObject's image as a sprite sheet, cut into a grid specified by `cols` and `rows`. Choosing this allows for the use of `spriteSheetAnimation` and `currentFrame` to pick the sprite to display, as well as `spriteClip`.

### Public Methods:

`public void removeFromRenderer()`

- Removes an object from the `Renderer`, if it was added via `Renderer.addObject()`. 

# Utils

Static helper class.

`public static int rand(int n, int m)`

- Returns a random int between `n` and `m` (inclusive)

`public static bool chance(int n)`

- Returns if a random int between 0 and 100 is less than `n`. Eg, a 30% chance can be represented by `Utils.chance(30)`.

# Animation

Helper class for animating a value over time along a smooth curve made of keyframes. `Animation` essentially wraps XNA's `Curve` class. 

`public Animation(float initialValue = 0)`

- An `Animation` is initialized with a single keyframe at time = 0;

`public void reset()`

- Resets the animation back to the start.

`public void addKeyframe(int time, float value)` 

- Adds a point to the curve at time `time` (in milliseconds), with value `value`.

`public float update(GameTime gameTime, ref float value)`

- Updates the animation's current time based on passed in `GameTime`, and set ref `value` to the current position on the animation's curve. Returns the delta between the value passed in the updated value. If for some reason the value along the curve is `NaN`, `value` is then set to 0.