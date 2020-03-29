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
      setSpriteSheet(MySheet);
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

[GameObjects](#gameobjects)

[TextureLoader](#textureloader)

[SpriteSheet](#spritesheet)

[TextObjects](#textobjects)

[FontLibrary](#fontlibrary)

[Fonts](#fonts)

[BoundingBox](#boundingbox)

[KeyFrames](#keyframes)

[CurveType](#curvetype)

[Animation](#animation)

[Random](#rand)

[Menu Helpers](#menu-helpers)

[The Element Class](#the-element-class)

[Button Elements](#button-elements)

[KeySwitcher Elements](#keyswitcher-elements)

[List Elements](#List-elements)

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

And finally, `void Main()` kicks off the whole process:

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

A basic drawable component, handling simple data such as X & Y position and boundaries. Nodes can be used as containers for other Nodes, and serve as the base class for other drawable components such as [GameObjects](#gameobjects) and [TextObjects](#textobjects).

## Creating a Node

###### constructor

`public Node()`

## Position

###### property

`public float X`

The x coordinate of a Node within its parent.

###### property

`public float Y`

The y coordinate of a Node within its parent.

## Bounds

###### property

`public BoundingBox Bounds = new BoundingBox()`

The bounding box around a Node's X and Y. Bounds.X and Bounds.Y represent the distance that the Bounds top left corner is placed from X and Y.

```c#
// For example, if a Node of size 64 * 64 wants its X & Y coordinates to
// be placed in the center of its bounds, we can achieve that like so:

Bounds.Rect = new Rectangle(-32, -32, 64, 64);
```

The `BoundingBox` class is a nested class within `Node`, and is essentially a wrapper around XNA's `Rectangle` class, along with a few other helpful properties related to drawing the rectangle. See [BoundingBox](#boundingbox).

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

Removes the calling Node from its parent, severing itself and its children from the engine.

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
    SpriteBatch.Begin( /*  your game settings */ );

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

# GameObjects

`Public GameObject : Node`

Node subclass that handles drawing images to the screen. The image provided to a `GameObject` can function as the whole sprite, or as a sprite sheet. Collision detection is also available.

As noted above, we use `Node.addChild(obj)` to add a `GameObject` to an existing `Node` as a child.

## Creating a GameObject

###### constructor

`public GameObject()`

## General purpose GameObject properties

###### property

`public float SpriteRotation = 0f`

###### property

`public float SpriteScale = 1f`

###### property

`public float Direction = 0f`

###### property

`protected int CollisionLayer = 0`

###### property

`public int LayerDepth = 0`

###### property

`public Color DrawColor = Color.White`

## Setting a GameObject's image

`protected void setImage(Texture2D newImage)`

Sets the `GameObject's` image as a single sprite. Drawing the `GameObject` will display the entire image, unless a `spriteClip` is specified. Changing the values of `GameObject.Animation` or `GameObject.CurrentFrame` will have no effect.

```c#
using Engine

public class Player: GameObject {

  public Player() {
     setImage(TextureLoader.Load("myimage.png"))
  }
}
```

## Setting GameObject's sprite sheet

`protected void setSpriteSheet(SpriteSheet sheet)`

Sets the GameObject's image as a sprite sheet, cut into a grid specified by cols and rows (in the example below, 4 x 4). Choosing this allows for the use of `Animation` and `CurrentFrame` to pick the sprite to display, as well as `SpriteClip`.

```c#
using Engine

public class Player: GameObject {

  SpriteSheet Sheet = new SpriteSheet("mysheet.png", 4, 4)

  public Player() {
    setSpriteSheet(Sheet);
  }
}
```

If a GameObject's image is supplied as a sprite sheet, there are a few ways to specify which frame or section of the sheet to show. The following are specified in order of precidence (if one is set, the ones below are automatically updated to match).

1. `protected Animation animation`
2. `protected int currentFrame`
3. `protected Rectangle spriteClip`

## Using a GameObject's Animation

###### property

`protected Animation Animation`

###### method

`protected void Animate(GameTime gameTime)`

Updates the GameObject's current Animation. `GameObject.CurrentFrame` will be updated to this value, and `GameObject.SpriteClip` will represent the bounds of the sprite on the sheet. User setting either `GameObject.CurrentFrame` or `GameObject.SpriteClip` in this case will have no effect, as these values are recomputed every time `Animate()` is called.

`Animation` is used so that the user can define custom lengths for each frame, rathen than being forced into a constant frame time. Internally, every frame the engine will call the supplied `Animation.Update(GameTime gameTime)` method, and casts the return value to an int to be used as the `currentFrame`. The `Animation.AnimationType` should be set to `AnimationType.STEP` for the easiest results.

Below is an example of setting a custom looping frame animation that animates through a sprite sheet's frames 5 through 10.

```c#
public class MyClass : GameObject {
  Animation runAnimation = new Animation(true, AnimationType.STEP);

  public MyClass() {
    SetSpriteSheet(MySheet);

     /*
      The sheet added can be thought of as a box cut into equal smaller boxes, labeled as follows:
       0,  1,  2,  3,
       4,  5,  6,  7,
       8,  9, 10, 11,
      12, 13, 14, 15
    */

    runAnimation.addKeyframe(0, 5);
    runAnimation.addKeyframe(100, 6);
    runAnimation.addKeyframe(300, 7);
    runAnimation.addKeyframe(800, 8);
    runAnimation.addKeyframe(1500, 9);
    runAnimation.addKeyframe(1580, 10);

    Animation = runAnimation;
  }

  public void CustomUpdate(GameTime gameTime) {
    Animate(gameTime);
  }
}
```

## Using a GameObject's `CurrentFrame`

###### property

`protected int currentFrame = -1`

For situations where we don't necessarily want to _animate_ changes between different sprite sheet frames, but still want control over which frame to display, we use `GameObject.SpriteClip`.

```c#
public class MyClass : GameObject {
  int hp = 100;
  int healthyFrame = 10;
  int lowHealthFrame = 11;
  int veryLowHealthFrame = 12;

  public MyClass() {
    setSpriteSheet(MySheet);
    CurrentFrame = healthyFrame;
  }

  public void takeDamage() {
    hp--;

    if (hp < 50) {
      CurrentFrame = lowHealthFrame;
    }
    else if (hp < 20) {
      CurretFrame = veryLowHealthFrame;
    }
  }
}
```

## Using a `GameObject`'s `spriteClip`

###### property

`protected Rectangle SpriteClip`

It is certainly possible to use a spritesheet that contains cells of different sizes. For this, we use `SpriteClip` to specify a rectangle on the sheet to display.

## Sprite Dimensions:

_`ImageWidth` and `ImageHeight` specify the dimensions of the image provided to the `GameObject`. If the image is supplied as a spritesheet (see `GameObject.SetSpriteSheet()`), `SpriteWidth` and `SpriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions._

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

- The offset within a GameObject's image to place `X` and `Y`.

- For example, if a `GameObject` is using an image as its sprite that is 64 \* 64 pixles, we can place `X` and `Y` in the center of the sprite by doing the following:

```c#
ImageOrigin = new Vector2(32, 32);
```

_Note: The position of `ImageOrigin` is different than the position of a Node's `Bounds.Rect`. `ImageOrigin` can be thought of as the offset from the image's top left corner where we want to place `X` and `Y`. `Bounds.Rect.X` and `Bounds.Rect.Y` are then the Bounds's offset away from `X` and `Y`. See below for a slightly complicated example:_

```c#
public class Player : GameObject {

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
    Bounds.Rect = new Rectangle(-16, -16, 32, 48);
  }
}
```

## Game Object Collision

todo

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

# TextObjects

`Public TextObject : Node`

## Creating a TextObject

###### constructor

`public TextObject(Font font = null, int x = 0, int y = 0, string text = null)`

## Setting a font

###### property

`public Font Font`

When setting the `Font`, the base (Node) class's `Bounds.Rect` is initialized to `Rectangle(0, 0, 10, _font.lineHeight)`. If `Text` is already set, `SetText()` will be called automatically as well.

## Setting text

###### property

`public Color Color = Color.White`

The color of the text.

###### method

`public void SetText(string t)`

Calling `SetText` automatically updated `Bounds.Rect` based on the new text to render, and uses these bounds to also update the `TextOrigin`, based on whatever type of `VerticalAlignment` is currently being used.

###### property

`public string Text`

Changing the `Text` property directly will trigger `SetText`, so either approach is valid.

## Text Origin

###### property

`protected Vector2 TextOrigin = new Vector2()`

Offset from the TextObject's position to draw the gyphs. For example, if a TextObject's bounds are 100 \* 64, we can vertically center the text within the bounds by:

```c#
TextOrigin = new Vector2(0, 32);

// or, more generally
TextOrigin = new Vector2(0, Bounds.Rect.Height / 2);
```

Setting the `VerticalAlignment` does this for us automatically.

## Vertical Alignment

###### property

`public VerticalAlignment VerticalAlignment`

Uses the `VerticalAlignment` enum:

```c#
public enum VerticalAlignment {
  TOP,
  CENTER
}
```

## Setting the BaseFont

###### static property

`public static Font BaseFont`

TextObjects, when created with `font = null`, will attempt to use the static `BaseFont`, meaning it should be set before any TextObjects are created.

```c#
public class GameController : Game {

  public static FontLib MyFontLib;

  public GameController() {
    MyFontLib = new FontLib("font/path.ttf", GraphicsDevice);
  }

  override protected void LoadContent() {
    TextObject.BaseFont = MyFontLib.CreateFont(Constants.FontSizeReg);
    base.LoadContent();
  }
}
```

# FontLibrary

`public sealed class FontLib`

###### Constructor

`public FontLib(string fontPath, GraphicsDevice graphics)`

Used to create fonts for a specific font file. Unlike textures, `.ttf` files do not need to be in the `content` directory.

```c#
FontLib FontLibrary = new FontLib("path/to/my/font.ttf", GraphicsDevice);
Font FontReg = FontLibrary.CreateFont(20);
Font FontLarge = FontLibrary.CreateFont(50);
```

# Fonts

`public class Font`

TLDR: To draw text with a `TextObject`, you must supply a `Font`, which is made from a `FontLib` and a specific point size.

`Fonts` are used as a cache for textures created from a `FontLib` at a particular point size. The first time we need a character, the `Font` will generate a texture for it, and use metrics from the `FontLib` to specify how it should be placed when drawing to the screen.

# BoundingBox

Nested class within `Node` that provides an easy way to manipulate boundaries around a Node.

###### constructor

`public BoundingBox()`

###### static property

`public static Texture2D Texture;`

Shared texture for drawing the rectangle. This should be initialized in the `Game.LoadContent` method in your game:

```c#
Node.BoundingBox.Texture = new Texture2D(GraphicsDevice, 1, 1);
Node.BoundingBox.Texture.SetData(new Color[] { Color.White });
```

###### property

`public Rectangle Rect = new Rectangle()`

###### property

`public bool IsHidden = true`

###### property

`public Color Color = Color.Blue`

###### property

`public float Alpha = 0.5f`

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
public class MyObject : GameObject {
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

# Menu Helpers

[The Element Class](#the-element-class)

[Button Elements](#button-elements)

[KeySwitcher Elements](#keyswitcher-elements)

[List Elements](#List-elements)

The `Element` class and its related utilities attempt to handle UI operations commonly associated with in-game menus. This includes simple tasks such as hover effects when moving the mouse over a menu, to more complex interactions like scroll views that dynamically show and hide content.

Example pause menu:

```c#
public sealed class PauseMenu : Element {
  private Game GC;

  public PauseMenu(Game gc, FontLib fontLib) {
    // Use the default menu controller
    MC = new MenuController();

    // Attach our Game. XNA's Game class contains many
    // settings that we might want to change from within
    // a pause menu, so it is helpful to keep a reference.
    GC = gc;

    // Set up some fonts
    FontReg = fontLib.CreateFont(Constants.FontSizeReg);
    FontLarge = fontLib.CreateFont(Constants.FontSizeLarge);

    // Set our bounds, and draw them to give the
    // element a background. These come from the
    // Node base class.
    Bounds.Rect = new Rectangle(0, 0, Parent.Bounds.Rect.Width, Parent.Bounds.Rect.Height);
    Bounds.IsHidden = false;
    Bounds.Color = Constants.PauseMenuBackground;
    Bounds.Alpha = Constants.PauseMenuAlpha;

    // Position our menu
    X = Constants.MenuLeft;
    Y = Constants.MenuTop;

    // Set up this elements label. In this case it will
    // serve as the title of our menu
    Label.Font = FontLarge;
    Label.Text = "Menu";

    // Add some buttons.
    Button backButton = new Button();
    backButton.OnClick = () => { CloseMenu() };
    backButton.Label.Text = "Return To Game";
    AddChildAsElement(backButton, 0, 20);

    Button closeButton = new Button();
    closeButton.OnClick = () => { ExitGame() };
    closeButton.Label.Text = "Exit Game";
    AddChildAsElement(closeButton, 0, 50);

    // Initialization is done, lets let the Game class know
    // that the menu is open.
    OnOpenMenu();
  }

  // Define some funtions that we can call to manipulate
  // Game settings, such as if the mouse is visible, or if
  // the game is running.
  public void OnOpenMenu() {
    GC.IsMouseVisible = true;
  }

  public void CloseMenu() {
    GC.IsMouseVisible = false;
    GC.GameState = GameState.RUNNING;
    RemoveFromParent();
  }

  public void ExitGame() {
    GC.GameState = GameState.QUIT;
  }
}

```

# The Element Class

`public class Element : GameObject`

Basic building blocks of menu. Elements are similar to Nodes in that they are the base class that define much of the functionality for subclasses like Buttons and Lists.

###### constructor

`public Element(Font font = null)`

## Setting element text

###### Property

`public TextObject Label`

All elements contain one `TextObject` for rendering basic text.

## Element display properties

###### property

`public Color BackgroundColor = MenuDefaults.ButtonBackgroundColor`

###### property

`public float BackgroundAlpha = MenuDefaults.ButtonBackgroundAlpha`

###### property

`public Color SelectedColor = MenuDefaults.ButtonSelectedColor`

###### property

`public float SelectedAlpha = MenuDefaults.ButtonSelectedAlpha`

###### property

`public Color TextColor = MenuDefaults.ButtonTextColor`

###### property

`public Color TextSelectedColor = MenuDefaults.ButtonTextSelectedColor`

## Adding child elements

###### property

`public int TopOffset = 0`

The distance from the Element's `Y` to place the next added child Element's `Y`. When calling `AddChildAsElement(Element el, int left, int top)`, the parameter `top` will increment `TopOffset` _before_ placing the child element.

###### property

`public int LeftOffset = 0`

The distance from the Element's `X` to place the next added child Element's `X`. When calling `AddChildAsElement(Element el, int left, int top)`, the parameter `left` will increment `LeftOffset` _before_ placing the child element.

_Note: `TopOffset` and `LeftOffset` can be changed any time. This is useful for creating columns, where we would, for example, call `AddChildAsElement` on a column of elements, then reset `TopOffset` back to 0 and increment `LeftOffset` into the next column, and repeat the process again._

###### method

`public void AddChildAsElement(Element el, int left, int top)`

Adds Element `el` to parent's list of Nodes (since Elements are just Nodes at the end of the day), and also manages setting values like `el.SelectIndex` and `NumSelectableChildren`. Also passes down `MC` (the parent's MenuController) if it is not `null`.

The arguments `left` and `top` specify Element `el`s distance from the previously added Element.

```c#
public PauseMenuControls() {
  List list = new List(1);
  AddChildAsElement(list, 0, 0);

  list.AddChildAsElement(cancelButton, 0, 0);

  foreach (string key in Input.InputMap.Keys) {
    KeySwitcher k = new KeySwitcher(key);

    list.AddChildAsElement(k, 0, BTN_MARGIN);
  }
}
```

## Selecting Elements

###### property

`public bool Selected = false`

Used by subclasses to determine if Element has been selected or not.

###### property

`public bool IsSelectable = false`

Determines whether or not the Element can be selected. This should be set _before_ adding via `AddChildAsElement`.

###### property

`public int CurrentSelectedChildIndex { get; protected set; } = 0`

The index of the current select child within a parent element.

###### method

`public virtual void SetSelected()`

Determines behavior of an Element when it becomes selected. In base the `Element` class, this does nothing.

###### method

`public virtual void SetUnselected()`

Determines behavior of an Element when it becomes unselected. In base the `Element` class, this does nothing.

_Note: Methods such as `Element.Update()` call `SetSelected()` and `SetUnselected()` automatically on child Elements if they are eligable. In most situations, we do not need to invoke these ourselves._

## Updating child elements

###### method

`public virtual void Update(float mouseX, float mouseY)`

Similar to `Node.Draw()`, `Element.Update()` loops over all child Elements and recursively calls their Update methods. The base `Element.Update()` is also in charge of reading key presses and the mouse position for determining things like the `CurrentSelectedChildIndex`, as well as selecting/ unselecting child Elements. For this reason, any override of the base `Update()` should include a call to `Base.Update()`.

# Menu Controller

`public sealed class MenuController`

Defines an Element's key press behavior when used in a menu-like context. It defines the following default properties:

```c#
public sealed class MenuController {

  public string KeySelect = "primary";
  public string KeyBack = "secondary";
  public string KeyUp = "up";
  public string KeyDown = "down";
  public string KeyLeft = "left";
  public string KeyRight = "right";

  public MenuController() { }
}
```

Recall that the static `Input` class is capable of taking string identifiers to check for key presses, rather than actual XNA Key inputs. The `MenuController` class allows us to define a set of keys unique to that menu, in the case where these inputs differ from the ones we use to move through the actual game.

# Button Elements

# KeySwitcher Elements

# List Elements

#

#

#

#

#

#

<!--  <<< Template >>> -->

## Readme API Template

# The `Class` Class

Description

_Other Info_

## Vars

`public int VarOne`

- Description about `VarOne`

`public int VarOne`

`protected int VarThree`

## Methods

`public Class(int something)`

- Notes (if any) about the constructor

`protected virtual OverrideMe()`

## Examples

Here is how to do something

```c#
class Class {
  // yes
}
```
