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

_Note: the term `readonly` is used loosely, and is ~usually~ meant to specifiy the property `{ get; private set; }`_




# Renderer

_Manages rendering of GameObjects. This class extends XNA's `Game` class, and mainly takes care of the boilerplate around loading and drawing objects._

Every frame, `Renderer` will loop over all `GameObjects` added via `addObject()` and call their `draw` method. `Renderer` also provides a virtual `updateObjects()` method, but this does nothing about of the box. `updateObjects()` should be overridden in whatever class is derrived from `Renderer`. 

`public Renderer(int resolutionWidth, int resolutionHeight, Color backgroundColor, bool start fullscreen, bool allowResize, Dictionary<string, Keys> inputMap)`

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

`public void setInputMapKey(string inputKey, Keys value)`

- Sets the `Renderer`'s `inputMap` for a given key to a new value. The supplied key must already exist on the `inputMap`.

### The System Rectangle:

We aren't sure of the best way to simply draw a rectangle to the screen, so for now we have simply exposed a global _readonly_ `systemRect`. 

- Example:

```
spriteBatch.Draw(Renderer.systemRect, new Rectangle(10, 10, 10, 10, Color.Blue));
```




# Engine Defaults

_Static class containing constant values meant for initializing the `Renderer`. It is very possible that these will not be used in whatever object inherents `Renderer`, as custom values could better fit the game's use case._

`public static readonly Dictionary<string, Keys> inputMap`

_defines a basic input map with the following values:_
- `"primary": Keys.X`
- `"secondary": Keys.Z`
- `"up": Keys.Up`
- `"down": Keys.Down`
- `"left": Keys.Left`
- `"right": Keys.Right`




# GameObject

_Displays a sprite on screen. The image provided can function as the whole sprite, or as a sprite sheet._

`public GameObject()`

### Position vars:

`public Vector2 position`

`public float direction`

`public Rectangle bounds`

- Initialized to (0, 0), the top left of the image or sprite cell. 

`public int colisionLayer`

- Box around `GameObject` used for collision detection. Initialized to `(0, 0, spriteWidth, spriteHeight)`. 

### Sprite Dimension vars:

`protected readonly int imageWidth`

`protected readonly int imageHeight`

`public readonly int spriteWidth`

`public readonly int spriteHeight`

- `imageWidth` and `imageHeight` specify the dimensions of the image provided to the `GameObject`. If the image is supplied as a spritesheet (see `setSpriteSheet()` below), `spriteWidth` and `spriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions.

`public float spriteRotation`

`public float spriteScale`

`public Vector2 center`

`public bool showBounds`

### Sprite sheet manipulation vars:

_If a GameObject's image is supplied as a sprite sheet, there are a few ways to specify which frame or section of the sheet to show. The following are specified in order of precidence (if one is set, the ones below are automatically updated to match)._

`protected Animation animation`

- GameObject will automatically run the supplied `animation`, and will display the sprite based on the animation's current frame. `GameObject.currentFrame` will be updated to this value, and `GameObject.spriteClip` will represent the bounds of the sprite on the sheet. User setting either `GameObject.currentFrame` or `GameObject.spriteClip` in this case will have no effect, as these values are computed every frame.

- `Animation` is used so that the user can define custom lengths for each frame, rathen than being forced into a linear frame time. Internally, every frame the engine will call the supplied `animation`'s `update(GameTime gameTime)` method, and casts the return value to an int to be used as the `currentFrame`. The `animation`'s `animationType` should be set to `AnimationType.STEP` for the easiest results. 

- Below is an example of setting a custom looping frame animation that animates through a sprite sheet's frames 5 through 10.
```c#
public class MyClass : GameObject {
  Animation runAnimation = new Animation(true, AnimationType.STEP);

  public MyClass() {}

  public override void load(ContentManager content){
    setSpriteSheet(TextureLoader.Load("mysheet.png", content), 4, 4);

    runAnimation.addKeyframe(0, 5);
    runAnimation.addKeyframe(100, 6);
    runAnimation.addKeyframe(300, 7);
    runAnimation.addKeyframe(800, 8);
    runAnimation.addKeyframe(1500, 9);
    runAnimation.addKeyframe(1580, 10);

    animation = runAnimation;
  }
}
``` 

`protected int currentFrame`

- Sets the sprite to the current frame on the sheet. `GameObject.spriteClip` will automatically represent these bounds on the sheet.

`protected Rectangle spriteClip`

- The area of the spritesheet to draw, initialized to the top left frame on the sheet. If `spriteSheetAnimation` or `currentFrame` have not been set, this can be used to display any rectangular section of the sheet. This is useful when a spritesheet contains sprites of different sizes.

### Other vars:

`public float layerDepth`

`public Color drawColor`

`public bool isHidden`

`public Color boundsColor = Color.Blue`;

`public float boundsAlpha = 0.5f`;

### Virtual Methods:

_Note: `GameObject` does not come with a virtual `update` function - this must be implimented in the derrived class._

`public virtual void init(Renderer renderer)`

- Init runs once before the main loop of the engine. This is a good place to do any major setup within the derived objects. 

- Alternatively, for simple usecases, it is best to not override `init()` and let the `base` version handle initialization.

`public virtual void load(ContentManager content)`

- Load is similar to Init, except it has access to the games content manager. Load should be used for setting graphical information of the object, like calling `setImage()` or `setSpriteSheet()`.

`public virtual void draw(SpriteBatch spriteBatch)`

- Called every frame by `Renderer`. Requires a `SpriteBatch` to render the object's image to the view. For most purposes this does not need to be overriden in the derrived class.

### Image/ Sprite Initializers:

_Below are image intializers meant to set a GameObject's sprite, meant to be called within the derived class's load method. Only one should be used per GameObject, as a GameObject only has one internal image member. Once an image is set, it cannot be removed or reset._

`protected void setImage(Texture2D newImage)`

- Sets the GameObject's image as a single sprite. Drawing the GameObject will display the entire image, unless a `spriteClip` is specified. Using `spriteSheetAnimation` or `currentFrame` will have no effect.

`protected void setSpriteSheet(Texture2D newImage, int cols, int rows)`

- Sets the GameObject's image as a sprite sheet, cut into a grid specified by `cols` and `rows`. Choosing this allows for the use of `spriteSheetAnimation` and `currentFrame` to pick the sprite to display, as well as `spriteClip`.

### Public Methods:

`public void removeFromRenderer()`

- Removes an object from the `Renderer`, if it was added via `Renderer.addObject()`. 

`public bool objectInBounds(GameObject obj, float offX = 0, float offY = 0, int cl = 0)`

- Returns if passed in GameObject `obj`'s bounds overlap with the caling GameObject's bounds. 
- `offX` and `offY` can be used to offset the calling object's bounding box.
- `cl` specifies the collision layer of the object to check. All `GameObject`'s `collisionLayer`s are initialized to 0.

`public bool objectInBounds<T>(List<T> objs, float offX = 0, float offY = 0, int cl = 0)`

- Override for `objectInBounds` that operates over a list of `T` objects, where `T` is derrived from `GameObject`. Returns whether or not a collision occured.

`public bool objectInBounds<T>(List<T> objs, out T value, float offX = 0, float offY = 0, int cl = 0)`

- Override for `objectInBounds` that operates over a list of `T` objects, where `T` is derrived from `GameObject`. Returns whether or not a collision occured. In this override, `T value` must be provided, and will be set to the first object found, or `null` otherwise.




# Utils

_Static helper class._

`public static int rand(int n, int m)`

- Returns a random int between `n` and `m` (inclusive)

`public static bool chance(int n)`

- Returns if a random int between 0 and 100 is less than `n`. Eg, a 30% chance can be represented by `Utils.chance(30)`.




# Animation

_Helper class for animating a value over time along a curve made of keyframes. `Animation` essentially wraps XNA's `Curve` class._

`public Animation(bool setLoop = true, AnimationType type = AnimationType.CURVE)`

- `Animation` constructor.

`public int index`

- Represents

`public void reset()`

- Resets the animation back to the start.

`public void addKeyframe(int time, float value)` 

- Adds a point to the curve at time `time` (in milliseconds), with value `value`.

`public float update(GameTime gameTime, ref float value)`

- Updates the animation's current time based on passed in `GameTime`, and set ref `value` to the current position on the animation's curve. Returns the delta between the value passed in the updated value. If for some reason the value along the curve is `NaN`, `value` is then set to 0.

`public float update(GameTime gameTime)`

- Returns the value of the curve at time `gametime`. If for some reason the value along the curve is `NaN`, value is then set to 0.




# AnimationType

_Static enum specifying how an `Animation`'s curve should be interpeted when calling its `update()` method._

`AnimationType.CURVE`

- The curve will be evauluated along its normal curve. 

`AnimationType.STEP`

-  The curve will be evaluated as if it were a step function. This is meant for sprite animations.

# Input

_Static helper for retreiving keyboard & mouse inputs._

_Holds a mapping of input keys (strings) to their corresponding `Keys` values. This is meant to make switching the key mapping during game time a bit easier, as the strings that map to input keys can remain constant in game code, while the keys themselves can change at any time via `setInputMapKey()` method below._

### Public Vars

`public static float mouseX`

`public static float mouseY`

### Public Methods

`public static bool isKeyDown(Keys input)`

- Returns if the Key `input` is down.

`public static bool isKeyDown(string mapKey)`

- Returns if the internal `inputMap` contains a mapping for the key `mapKey` that is down.

`public static bool isKeyUp(Keys input)`

- Returns if the Key `input` is up.

`public static bool isKeyUp(string mapKey)`

- Returns if the internal `inputMap` contains a mapping for the key `mapKey` that is up.

