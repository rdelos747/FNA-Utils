# FNA Utils

Library of useful classes and utilies for FNA.

# Installation

1. Clone this repo, preferably somewhere with easy access to your game and FNA.
2. In `utils.csproj` change the FNA include reference to wherever you've placed FNA.

```
<!-- Reference FNA project -->
  <ItemGroup>
    <ProjectReference Include="path.to.FNA" />
  </ItemGroup>
```

3. run the Restore build task
4. email us when you realize we forgot something

# Using this in your game

Within the game's `.csproj` file, add the following:

```
  <ItemGroup>
    <ProjectReference Include="path.to.utils.csproj" />
  </ItemGroup>
```

Then, use `using Utils;` along with the rest of your includes within your .cs files. Below is an exmaple of a simple player class, created from a `GameObject`.

```c#
using System.Collections.Generic;
using Microsoft.Xna.Framework;

using Utils;

namespace MyGame {

  public class Player : GameObject {
    public int speed = 0;

    private SpriteSheet MySheet = new SpriteSheet("path/to/sheet", 16, 16);

    public Player() {
      SetSpriteSheet(MySheet);
      Bounds = new Rectangle(-16, -16, 32, 48);
      ImageOrigin = new Vector2(32, 32);
    }

    public void Update() {
      if (Input.IsKeyDown("right")) {
        speed = 5;
      }
      else if (Input.IsKeyDown("right")) {
        speed = -5;
      }
      else {
        speed = 0;
      }

      X += speed;
    }
  }
}
```

# Notes

We mainly develop via VS Code on OS X, so our setup or instructions may not translate over to Visual Studio or another IDE.

This project was initially bootstrapped with [this template](https://github.com/TheSpydog/fna_vscode_template), which gave us the initial .vscode functions. We don't author any of it, but we may fork it soon?

Some aspects of `FNA-Utils`, such as the `TextureLoader` class, `Resolution` class, and the initial template of the `Input` class, were also not authored by us, but were provided as parts of the Michael Hicks Toolbox, which we discovered through [this youtube series](https://www.youtube.com/watch?v=WQOebBVIB0I). It was instrumental in helping us understand how to begin developing games with FNA.

## Also Notes

We are also very new to C# in general. All feedback is welcome :)

# API Reference

[Setup](#setup)

[The Node Class](#the-node-class)

[Input](#input)

[Resolution](#resolution)

[Sprite](#sprite)

[TextureLoader](#textureloader)

[SpriteSheet](#spritesheet)

[Labels](#labels)

[Atlas](#atlas)

[Fonts](#fonts)

[FontLibrary](#fontlibrary)

[BoundingBox](#boundingbox)

[KeyFrames](#keyframes)

[CurveType](#curvetype)

[Animation](#animation)

[Random](#rand)

# Setup

One thing `FNA-Utils` does not provide is the basic app structure/ life cycle dictated by XNA. Below is an example `GameController` (derived from XNA's `Game` class) we use to incorporate the utils into XNA.

_Note: Some utilities (especially static helpers) such as `Input` and `Node.BoundingBox.Texture` require setup before they can be used. All required initializations are mentioned in this example._

```c#
// GameController.cs

using Utils;

public partial class GameController : Game {

  protected GraphicsDeviceManager Graphics;
  protected SpriteBatch SpriteBatch;

  public static Texture2D SystemRect { get; private set; }

  public static FontLib SystemFontLib;

  private Node Root;

  public GameController() {
    /*
    Init graphics and content
    */
    Graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    TextureLoader.Content = Content;

    /*
    Init resolution
    */
    Resolution.Init(ref Graphics, Constants.BackgroundColor);
    Resolution.SetVirtualResolution(Constants.GameWidth, Constants.GameHeight);
    Resolution.SetResolution(Constants.GameWidth, Constants.GameHeight, Constants.StartFullScreen);

    /*
    Init the input map
    */
    Input.SetInputMap(Constants.InputMap);

    /*
    Init a basic font
    */
    SystemFontLib = new FontLib(Constants.FontPath, GraphicsDevice);

    /*
    Create the root node. Having a single root node at (0,0) makes drawing child nodes very simple.
    */
    Root = new Node();
    Root.Bounds.Rect = new Rectangle(0, 0, Constants.GameWidth, Constants.GameHeight);
  }

  override protected void LoadContent() {
    SpriteBatch = new SpriteBatch(GraphicsDevice);

    /*
    Set the shared (static) node rectangle, which is used for drawing bounds.
    */
    Node.BoundingBox.Texture = new Texture2D(GraphicsDevice, 1, 1);
    Node.BoundingBox.Texture.SetData(new Color[] { Color.White });

    /*
    TextObjects have a shared (static) BaseFont, which is nice for many games where only one font is necessary
    */
    TextObject.BaseFont = SystemFontLib.CreateFont(Constants.FontSizeReg);

    base.LoadContent();
  }
}
```

And finally, `void Main()` kicks off the whole process. If you use a template or some sort of code generator, you probably already have something similar.

```c#
// program.cs

public static void Main(string[] args) {
  using (GameController game = new GameController()) {
    game.Run();
  }
}
```

# Node Class

`public class Node : IDisposable`

A basic drawable component, handling simple data such as X & Y position and boundaries. Nodes can be used as containers for other Nodes, and serve as the base class for other drawable components such as [Sprites](#sprites) and [Labels](#labels).

## Creating a Node

###### constructor

`public Node()`

## Position

###### property

`public Vector2 Position`

## Bounds

###### property

`public Size Size`

###### property

`public Vector2 BoundsOffset`

## Collision

###### property

`public CollisionType CollisionType = CollisionType.Rectangle`

###### property

`public float Radius`

Used with `CollisionType.Circle`.

###### property

`public Vector2 End`

Used with `CollisionType.Line`

###### Method

`public bool Collides(Node other)`

###### Method

`public bool Collides(Node other, float x, float y)`

###### Method

`public bool Collides(Node other, Vector2 offset)`

###### Method

`public bool PointInBounds(Vector2 p)`

## Adding Children

###### method

`public void AddChild(Node n)`

Adds Node n to the calling Node's internal list of child Nodes, and sets n.parent to the calling Node.

###### property

`public List<Node> Nodes`

List if Node's children, can only be directly manipulated from within the Node class.

## Removing a Node

###### method

`public void RemoveFromParent()`

Removes the calling Node from its parent, severing itself and its children from its parent `Node`.

## Drawing a Node

###### property

`public bool IsHidden = false`

If true, the current Node and all of its children are not drawn.

###### property

`public bool ShowCenter = false`

If true, a small rectangle is drawn at the Node's X,Y position, which is useful when debugging aspects such as the Node's position in the world and its bounds placement.

###### method

`public virtual void Draw(SpriteBatch spriteBatch, float lastX = 0, float lastY = 0)`

Draws the Node, and recursively calls Draw on each of its child Nodes. Since a Node's X and Y coordinates are relative to the parent, world coordinates are calculated during each pass of Draw and passed down as `lastX` and `lastY`.

Since Draw calls itself on all children of the initial Node, it is recommend that all game Nodes are nested under a single parent Node placed at (0,0). This way, in our Game class, we need only call Draw on the root:

```c#
// Game is given to us from FNA :)

public class MyGame : Game {

  public GameController() {
      // ... other initializers ...

      /*
      Create the root node
      */
      Root = new Node();
      Root.Bounds = new Rectangle(0, 0, Constants.GameWidth, Constants.GameHeight);
    }

  override protected void Draw(GameTime gameTime) {

    Resolution.BeginDraw();
    SpriteBatch.Begin( /*  your settings */ );

    /*
    Any children added to Root, as well as their real-world coordinates,
    will be handled by this single Draw call.
    */
    Root.Draw(SpriteBatch);

    SpriteBatch.End();

    base.Draw(gameTime);
  }
}
```

# Input

`public static class Input`

The `Input` helper provides an api to help polling for keyboard, mouse, and gamepad events. To further assist with functionality such as changing controls or input methods controls mid-game, the `Input` class focusses on the abstract concept of actions, which the user defines in their game code. Each action is mapped to a real key or button event.

###### Initialization

```c#
// Create a mapping for the inputs you want to support:
public static readonly Dictionary<string, Keys> KeyboardMap = new Dictionary<string, Keys>() {
  {"Primary", Keys.X},
  {"Secondary", Keys.Z},
  {"Up", Keys.Up},
  {"Down", Keys.Down},
  {"Left", Keys.Left},
  {"Right", Keys.Right},
  {"Pause", Keys.Enter}
};

public static readonly Dictionary<string, Buttons> ButtonMap = new Dictionary<string, Buttons>() {
  {"Primary", Buttons.A},
  {"Secondary", Buttons.B},
  {"Up", Buttons.DPadUp},
  {"Down", Buttons.DPadDown},
  {"Left", Buttons.DPadLeft},
  {"Right", Buttons.DPadRight},
  {"Pause", Buttons.Start}
};


// Then in the Game constructor, pass your mapping to the player ports you want to support. You must first call init().

Input.Init();

Input.SetActionMap(ButtonMap, PlayerIndex.One);
Input.SetActionMap(KeyboardMap, PlayerIndex.One);

// Now, player one supports both the keyboard and gamepad. It is a good idea however to pick just one. Below, we first check if there is an availible game pad on this port. If there is, connect to it, otherwise use the keyboard.

if (Input.IsGamePadConnected(Constants.Ports.P1)) {
  Input.SetPortInput(Input.InputType.GamePad, Constants.Ports.P1);
  Console.WriteLine("player 1 game pad success");
}
else {
  Input.SetPortInput(Input.InputType.Keyboard, Constants.Ports.P1);
  Console.WriteLine("player 1 game pad failed");
}

// Becuase we set player one to have a mapping for *both* keyboard and gamepad, they can change back and forth durng gameplay with ease.

// Finall, we must call update() in the games update loop
Input.Update();
```

## Checking for gamepads

###### static method

`public static bool IsGamePadConnected(PlayerIndex pi)`

## Checking for any event last frame

###### static method

`public static Keys GetRecentKey(PlayerIndex pi)`

###### static method

`public static Buttons GetRecentButton(PlayerIndex pi)`

_Note: `GetRecentKey` and `GetRecentButton` are the only polling functions that operate around actual key/ button values._

## Port input types

###### Enum

```c#
public enum InputType {
  Keyboard,
  GamePad
}
```

###### static method

`public static InputType GetPortInput(PlayerIndex pi)`

###### static method

`public static void SetPortInput(InputType inputType, PlayerIndex pi)`

## Input Actions

###### static method

`public static void SetPortInput(InputType inputType, PlayerIndex pi) {`

###### static method

`public static void SetActionMap(Dictionary<string, Keys> keyboardMap, PlayerIndex pi)`

###### static method

`public static void SetActionMap(Dictionary<string, Buttons> gamePadMap, PlayerIndex pi)`

###### static method

`public static void SetAction(string action, Keys val, PlayerIndex pi)`

###### static method

`public static void SetAction(string action, Buttons val, PlayerIndex pi)`

###### static method

`public static bool IsActionDown(string action, PlayerIndex pi)`

###### static method

`public static bool IsActionUp(string action, PlayerIndex pi)`

###### static method

`public static bool ActionPressed(string action, PlayerIndex pi)`

###### static method

`public static List<string> GetActiveActions(PlayerIndex pi)`

# Resolution

`public static class Resolution`

Static resolution helper, authored by David Amador (http://www.david-amador.com), and taken from a collection of tools compiled by Michael Hicks (http://michaelarts.net/)

This resolution helper allows us to draw within a fixed coordinate system regardless of the actual screen dimensions.

```c#
public partial class GameController : Game {

  public GameController() {
    /*
    Init resolution
    */
    Resolution.Init(ref Graphics, Constants.BackgroundColor);
    Resolution.SetVirtualResolution(Constants.GameWidth, Constants.GameHeight);
    Resolution.SetResolution(Constants.GameWidth, Constants.GameHeight, Constants.StartFullScreen);

    Window.AllowUserResizing = Constants.AllowResize;
    Window.ClientSizeChanged += onResize;
  }

  private void onResize(Object sender, EventArgs e) {
    Rectangle resizedBounds = GraphicsDevice.Viewport.Bounds;
    Resolution.SetResolution(resizedBounds.Width, resizedBounds.Height, Constants.StartFullScreen);
  }

  override protected void Draw(GameTime gameTime) {

    Resolution.BeginDraw();
    SpriteBatch.Begin(
      SpriteSortMode.Deferred,
      BlendState.AlphaBlend,
      SamplerState.LinearClamp,
      DepthStencilState.Default,
      RasterizerState.CullNone,
      null,
      Resolution.getTransformationMatrix()
    );

    SpriteBatch.End();

    base.Draw(gameTime);
  }
}
```

# Sprite

`Public Sprite : Node`

Node subclass that handles drawing images to the screen. The image provided to a `Sprite` can function as the whole sprite, or as a sprite sheet. Collisoin detection is handled by the base `Node` class;

As noted above, we use `Node.addChild(obj)` to add a `Sprite` to an existing `Node` as a child.

## Creating a Sprite

###### constructor

`public Sprite()`

## General purpose Sprite properties

###### property

`public float Rotation = 0f`

###### property

`public float Scale = 1f`

###### property

`public float Direction = 0f`

###### property

`public float DrawDepth = 0.0f`

###### property

`public Color Color = Color.White`

###### property

`public float Alpha = 1.0f`

## Setting a Sprite's image

`protected void SetImage(Texture2D newImage)`

Sets the `Sprite's` image as a single sprite. Drawing the `Sprite` will display the entire image, unless a `ImageClip` is specified. Changing the values of or within `Sprite.Animation` or `Sprite.CurrentFrame` will have no effect.

```c#
using Utils

public class Player: Sprite {

  public Player() {
     setImage(TextureLoader.Load("myimage.png"))
  }
}
```

## Setting Sprites's sprite sheet

`protected void SetSpriteSheet(SpriteSheet sheet)`

Sets the Sprite's image as a sprite sheet, cut into a grid specified by cols and rows (in the example below, 4 x 4). Choosing this allows for the use of `Animation` and `CurrentFrame` to pick the sprite to display, as well as `SpriteClip`.

```c#
using Utils

public class Player: Sprite {

  SpriteSheet Sheet = new SpriteSheet("mysheet.png", 4, 4)

  public Player() {
    setSpriteSheet(Sheet);
  }
}
```

If a Sprite's image is supplied as a sprite sheet, there are a few ways to specify which frame or section of the sheet to show. The following are specified in order of precidence (if one is set, the ones below are automatically updated to match).

1. `protected Animation animation`
2. `protected int CurrentFrame`
3. `protected Rectangle ImageClip`

## Using a Sprite's Animation

###### property

`protected Animation Animation`

###### method

`protected void Animate(GameTime gameTime)`

Updates the Sprite's current Animation. `Sprite.CurrentFrame` will be updated to this value, and `Sprite.ImageClip` will represent the bounds of the sprite on the sheet. User setting either `Sprite.CurrentFrame` or `Sprite.ImageClip` in this case will have no effect, as these values are recomputed every time `Animate()` is called.

`Animation` is used so that the user can define custom lengths for each frame, rathen than being forced into a constant frame time. The `Animation.AnimationType` should be set to `AnimationType.STEP` for the easiest results.

See [Animation](#animation) for specific usage;

## Using a Sprite's `CurrentFrame`

###### property

`protected int CurrentFrame = -1`

For situations where we don't necessarily want to _animate_ changes between different sprite sheet frames, but still want control over which frame to display, we use `Sprite.CurrentFrame`.

```c#
public class MyClass : Sprite {
  int Hp = 100;
  int HealthyFrame = 10;
  int LowHealthFrame = 11;
  int VeryLowHealthFrame = 12;

  public MyClass() {
    SetSpriteSheet(MySheet);
    CurrentFrame = HealthyFrame;
  }

  public void TakeDamage() {
    Hp--;

    if (Hp < 50) {
      CurrentFrame = LowHealthFrame;
    }
    else if (Hp < 20) {
      CurretFrame = VeryLowHealthFrame;
    }
  }
}
```

## Using a Sprite's `ImageClip`

###### property

`protected Rectangle ImageClip`

It is certainly possible to use a spritesheet that contains cells of different sizes. For this, we use `ImageClip` to specify a rectangle on the sheet to display.

## Sprite Dimensions:

_`ImageWidth` and `ImageHeight` specify the dimensions of the image provided to the `Sprite`. If the image is supplied as a spritesheet (see `Sprite.SetSpriteSheet()`), `SpriteWidth` and `SpriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions._

###### property

`protected readonly int ImageWidth`

###### property

`protected readonly int ImageHeight`

###### property

`public readonly int SpriteWidth`

###### property

`public readonly int SpriteHeight`

## Image Origin:

`protected Vector2 ImageOrigin`

- The offset within a Sprite's image to place `X` and `Y`.

- For example, if a `Sprite` is using an image as its sprite that is 64 * 64 pixles, we can place `X` and `Y` in the center of the sprite by doing the following:

```c#
ImageOrigin = new Vector2(32, 32);
```

_Note: The position of `ImageOrigin` is different than the position of a Node's `BoundsOffset`. `ImageOrigin` can be thought of as the offset from the image's top left corner where we want to place `X` and `Y`. `BoundsOffset.X` and `BoundsOffset.Y` are then the Bounds's offset away from `X` and `Y`. See below for a slightly complicated example:_

```c#
public class Player : Sprite {

  public Player() {
    /*
    Suppose we are supplied a 64 * 64 pixel image for our sprite.
    To center our X & Y in the center of the image, we do
    */
    ImageOrigin = new Vector2(32, 32);

    /*
    However, suppose also that the part of the image representing the
    sprite only occupies a 32 * 48 pixel rectangle in the center of
    the image. If we want our bounds to be exact, we must set set the
    bounds separately:
    */
    Size = new Size(32, 48)
    BoundsOffset = new Vector2(-16, -16);
  }
}
```

# TextureLoader

`public static class TextureLoader`

Static helper that assists with loading `Texture2D` instances from the `content` directory.

###### static property

`public static ContentManager Content`

`TextureLoader` also holds the static `ContentManager`, which deals with caching our textures after initial load. We must attach it during the initialization of our `Game`:

```c#
public class GameController : Game {
  public GameController() {
    Content.RootDirectory = "Content";
    TextureLoader.Content = Content;
  }
}
```

###### method

`public static Texture2D Load(string filePath)`

Returns a `Texture2D` from the supplied image file. The Texture2D's alpha is premultiplied atomatically by `TextureLoader.Load`.

`Load` is primarily used to pass textures directly to `GameObject` texture initializers:

```c#
public class Enemy : GameObject {
  public Enemy {
    SetImage(TextureLoader.Load("path/to/sheet"));
  }
}
```

# SpriteSheet

`public class SpriteSheet`

Defines a sprite sheet to be used with GameObjects.

###### constructor

`public SpriteSheet(string path, int cols, int rows)`

###### readonly property

`public Texture2D SheetTexture`

###### readonly property

`public int Cols`

###### readonly property

`public int Rows`

###### readonly property

`public int Width`

The width of a cell in the sheet.

###### readonly property

`public int Height`

The height of a cell in the sheet.

## Example Usage

SpriteSheets are helpful when you have multiple classes that use the same sheet. We only need to define the sheet once, and can pass it to every object that needs it via `GameObject.SetSpriteSheet()`.

```c#
public SpriteSheet Sheet1 = new SpriteSheet("sheet.png", 8, 8);

public class MyThing : GameObject {
  public MyThing () {
    setSpriteSheet(Sheet1);
    frame = 2;
  }
}

public class MyOtherThing : GameObject {
  public MyOtherThing () {
    setSpriteSheet(Sheet1);
    frame = 7;
    // while both objects use the same sheet, they can each display different areas
  }
}
```

# Labels

`Public Label : Node`

## Creating a Label

###### constructors

`public Label(Font font = null)`

`public Label(string text, int x, int y)`

`public Label(string text, int x, int y, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Font font = null)`

## Setting a font

###### property

`public Font Font`

When setting the `Font`, the base (Node) class's `Size` is initialized to `Size(0, Font.lineHeight)`. If `Text` is already set, `SetText()` will be called automatically as well.

## Setting text

###### property

`public Color Color = Color.White`

The color of the text.

###### method

`public void SetText(string t)`

Calling `SetText` automatically updates `Size` based on the new text to render, and uses these bounds to also update the `TextOrigin`, based on whatever type of `VerticalAlignment` and `HorizontalAlignment` is currently being used.

###### property

`public string Text`

Changing the `Text` property directly will trigger `SetText`, so either approach is valid.

## Text Origin

###### property

`protected Vector2 TextOrigin = new Vector2()`

Offset from the Label's position to draw the gyphs. For example, if a Labels's bounds are 100 * 64, we can vertically center the text within the bounds by:

```c#
TextOrigin = new Vector2(0, 32);

// or, more generally
TextOrigin = new Vector2(0, Size.Height / 2);
```

Setting the `VerticalAlignment` and `HorizontalAlignment` does this for us automatically.

## Vertical Alignment

###### property

`public VerticalAlignment VerticalAlignment`

Uses the `VerticalAlignment` enum:

```c#
public enum VerticalAlignment {
  Top,
  Center
}
```
## Horizontal Alignment

###### property

`public HorizontalAlignment HorizontalAlignment`

Uses the `HorizontalAlignment` enum:

```c#
public enum HorizontalAlignment {
  Left,
  Center,
  Right
}
```

## Setting the BaseFont

###### static property

`public static Font BaseFont`

Labels, when created with `font = null`, will attempt to use the static `BaseFont`, meaning it should be set before any Labels are created.

Using TTF fonts:

```c#
public class GameController : Game {

  public static TTFFontLib MyFontLib;

  public GameController() {}

  override protected void LoadContent() {
    MyFontLib = new FontLib("font/path.ttf", GraphicsDevice);

    TTFFont myFont = MyFontLib.CreateFont(Constants.FontSizeReg);
    TextObject.BaseFont = myFont;
    base.LoadContent();
  }
}
```

Using font Atlas:

```c#
public class GameController : Game {

  public static Atlas FontAtlas;
  public static SpriteSheet FontSheet = new SpriteSheet("path/to/sheet", 16, 14);

  public GameController() {}

  override protected void LoadContent() {
    FontAtlas = new Atlas(FontSheet);

    TextObject.BaseFont = FontAtlas;
    base.LoadContent();
  }
}
```

# Fonts

`public abstract class Font`

`public sealed class Atlas : Font`

`public sealed class TTFFont: Font`

TLDR: To draw text with a `Label`, you must supply a `Font`, which is made from either an `Atlas` (for spritesheet based fonts) or a `TTFFont` and a specific point size.

Atlases made from SpriteSheets are preferred for pixel art games, especially those designed around a smaller resolution, as it is easy to predict how the glyphs will scale.

The `TTFFont` class is used as a cache for textures created from a `TTFFontLib` at a particular point size. The first time we need a character, the `TTFFont` will generate a texture for it, and use metrics from the `TTFFontLib` to specify how it should be placed when drawing to the screen.

# KeyFrames

`public sealed class KeyFrames`

Creates a curve for a given set of values that can be evaluated at specific key frames.

_In many games, multiple objects might use the same keyframes for their animations. It is more efficient to store a single list of frames in memory, and let each object have its own index into the frames for animating._

See the [Animation](#animation) class for usage examples.

## Creating KeyFrames and adding values

###### constructor

`public KeyFrames(CurveType ct)`

See the [CurveType](#curvetype) enum.

###### constructor

`public KeyFrames(CurveType ct, (int time, int value)[] frames)`

###### method

`public void AddKeyframe(int time, float value)`

###### method

`public Animation Create(bool loop = true)`

Returns a new `Animation` object set to use this Keyframe's curve.

## Evaluating Keyframes

While it is possible to manually animate something by reading a KeyFrame's curve at specific points, it is recommented to use the [Animation](#animation) class for this, as the `Animation.Evaluate` family of methods already handle reading the following methods and properties of the `KeyFrames` class.

###### method

`public float Evaluate(int time, int index)`

###### readonly property

`public int NumPoints = 0`

###### readonly property

`public int MaxTime = 0`

## Using the curve

###### property

`public CurveType CurveType`

###### readonly property

`public Curve Curve = new Curve();`

###### method

`public void SmoothTangents()`

For KeyFrames that use `CurveType.Curve`, it is recommended to call `SmoothTangents()` after adding all points to the curve before evaluating.

# CurveType

Defines possible curve types for use with the `KeyFrames` class. `CurveType.STEP` should be used when creating key frames for sprite animations.

```c#
public enum CurveType {
  CURVE,
  STEP
}
```

# Animation

`public class Animation`

Animates a value across a given KeyFrame's curve.

## Creating Animations

####### constructor

`public Animation(KeyFrames kf, bool loop = true)`

To create an `Animation`, we must first create a `KeyFrames` and add points to it.

The following example is for use in animating a GameObject.

```c#
// Constants.cs
public static class Constants {
  public static readonly KeyFrames MyFrames = new KeyFrames(CurveType.STEP,
    new (int time, int value)[]{
      (0, 0),
      (1000, 1),
      (1200, 2),
      (1400, 3),
      (1600, 4),
      (1800, 5),
      (2000, 6)
    }
  );
}

// MyObject.cs
public class MyObject : Sprite {
  public MyObject() {
    animation = Constants.MyFrames.Create();
    //or
    animation = new Animation(Constants.MyFrames);
  }
}
```

To actually animate the frames, `MyObject.Animate()` must be called during the game's update loop.

## Updating Animations

###### method

`public bool IsFinished()`

###### method

`public float Update(GameTime gameTime)`

Animations can also be used to animate values other than sprite frames. Then following example animates the scroll position of a level.

```c#
// MyObject.cs
public class MyLevel : Node {
  private KeyFrames ScrollFrames = new KeyFrames(CurveType.CURVE);
  private Animation scrollAnimation;

  public MyLevel() {
    ScrollFrames.AddKeyframe(0, 0);
    ScrollFrames.AddKeyframe(500, (float)(Constants.Screen.GameHeight * 0.25));
    ScrollFrames.AddKeyframe(800, (float)(Constants.Screen.GameHeight * 0.75));
    ScrollFrames.AddKeyframe(1000, (float)(Constants.Screen.GameHeight * 0.95));
    ScrollFrames.AddKeyframe(1200, Constants.Screen.GameHeight);
    ScrollFrames.smoothTangents();
    scrollAnimation = new Animation(ScrollFrames, false);
  }

  public void Update(GameTime gameTime) {
    float delta = scrollAnimation.Update(gameTime, ref scroll);
    // do something with delta
  }
}
```

# Rand

