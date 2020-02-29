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

    public Player() {
      setSpriteSheet(TextureLoader.Load("path/to/sheet"), 16, 16);
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

## Also Notes

We are very new to FNA, monogame, and C# in general.

# API Reference

[Setup](#setup)

[The Node Class](#the-node-class)

[Input](#input)

[Resolution](#resolution)

[GameObjects](#gameobjects)

[TextureLoader](#textureloader)

[TextObjects](#textobjects)

[Fonts](#fonts)

[BoundingBox](#boundingbox)

[Animations](#animations)

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
//For example, if a Node of size 64 * 64 wants its X & Y coordinates to be placed in the center of its bounds, we can achieve that like so:

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

###### Initialization

```c#
// In Game constructor
Input.SetInputMap(/* pass your input map here*/);
```

The static `Input` helper contains functions for grabbing the current key/ mouse press. For example, you can check if a specific XNA `Key` is down.

```C#
using Microsoft.Xna.Framework.Input;
using Engine;

Input.isKeyDown(Keys.Up);
```

`Input` also contains an `InputMap` dictionary that associates strings to `Key` values. With the InputMap set, you can check for key events by passing in a string. This makes it easy to change the actual `Key` mid-game, as the string we use to check never changes.

```C#
Dictionary<string, Keys> myMap = new Dictionary<string, Keys>() {
  {"primary", Keys.X},
  {"secondary", Keys.Z},
  {"up", Keys.Up},
  {"down", Keys.Down},
  {"left", Keys.Left},
  {"right", Keys.Right}
};

Input.setInputMap(myMap);

Input.hasKey("primary")
// returns true

Input.hasKey("banana")
// returns false

Input.isKeyUp("tomato")
// does not throw an error if the key does not exist, just returns false

Input.isKeyUp("primary");
// will return true if Keys.X is Up

Input.setInputMapKey("primary", Keys.Enter);
// change the key that "primary" points to

Input.isKeyUp("primary");
// will return true if Keys.Enter is up
```

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

`protected void setSpriteSheet(Texture2D newImage, int cols, int rows)`

Sets the GameObject's image as a sprite sheet, cut into a grid specified by cols and rows (in the example below, 4 x 4). Choosing this allows for the use of `Animation` and `CurrentFrame` to pick the sprite to display, as well as `SpriteClip`.

```c#
using Engine

public class Player: GameObject {

  public Player() {
    setSpriteSheet(TextureLoader.Load("mysheet.png"), 4, 4);
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
    SetSpriteSheet(TextureLoader.Load("mysheet.png"), 4, 4);

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
    setSpriteSheet(TextureLoader.Load("mysheet.png"), 4, 4);
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

# TextObjects

# Fonts

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

# Animations

# Rand

# Menu Helpers

[The Element Class](#the-element-class)
[Button Elements](#button-elements)
[KeySwitcher Elements](#keyswitcher-elements)
[List Elements](#List-elements)

# Menu Controller

# The Element Class

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
