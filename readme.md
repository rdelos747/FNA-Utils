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



# Engine Lifecycle

_The `Renderer` class manages rendering of `Nodes`. The `Renderer` class extends XNA's `Game` class, and mainly takes care of the boilerplate around loading and drawing objects._

_Every frame, `Renderer` will loop over all `Nodes` added via `addChild()` and call their `draw` method. `Renderer` also provides a virtual `updateObjects()` method, but this does nothing about of the box. `updateObjects()` should be overridden in whatever class is derrived from `Renderer`._

## Initialization

1. `Renderer` constructor intializes `Input.inputMap` and other default game settings, most of which are located in the `/defaults` directory.
2. FNA calls `Renderer.initalize()` and `Renderer.loadContent()`. Both are virtual, but generally `loadContent()` is not needed.
3. `Renderer` is also initialized with a root `Node`. All children added to the Renderer are decendants of this node.

## Game Loop

1. FNA calls `Renderer.Update()`, which is meant to handle behind-the-scenes functions such as refreshing `Input`'s state.
2. Renderer calls its private `defaultUpdate()` method, which will handle any default functionality that the user has not manually overridden. For example, if the default `inputMap` is still present, or if the user defined `inputMap` contains a key `engine_pause` that is triggered, then `defaultUpdate()` will toggle the `engineState` between `RUNNING` and `PAUSED`. Within `defailtUpdate()` is also code for displaying the default pause menu, or whatever pause menu the user attaches to `Renderer.pauseMenu`.
3. If `Renderer.engineState` is set to `EngineState.QUIT`, the game will exit here.
4. If `Renderer.engineState` is set to `EngineState.RUNNING`, `Renderer.updateGame()` is called, which should be overridden in the derrived class. This is the starting point for user-defined game updates.
5. FNA calls `Renderer.Draw()`, which draws every `Node` that has been added so far with `Renderer.addChildToRoot()`. If any of these children have children of their own, it will call their draw methods, and so on.
6. Repeat.




# `Renderer` Examples

## Creating a game with `Renderer`

```c#
// MyGame.cs

using Engine;

public class Car : GameObject {
  int speed = 0;

  public Car() {}

  public void update() {
    speed++;
  }
}

public class Game1 : Renderer {
    Car car;

    public Game1() : base(
      1280,             // window width
      720,              // window height
      Color.Black,      // background color
      false,            // start full screen
      true              // allow resize
    ) { }

    override protected void Initialize() {
      car = new Car(); 
      addChildToRoot(car);
      base.Initialize();
    }

    override protected void updateGame(GameTime gameTime) {
      car.update(gameTime);
    }
  }

// Program.cs
class Program {

    public static void Main(string[] args) {
      
      using (Game game = new Game1()) {
        game.Run();
      }
    }
  }
```

In the example above, note that we also created a `GameObject` called Car, and used `addChildToRoot()` to add it to our `Renderer`'s root node. 



## Using the `inputMap`

The static `Input` helper contains functions for grabbing the current key/ mouse press. For example, you can check if a specific XNA `Key` is down.
```C#
using Microsoft.Xna.Framework.Input;
using Engine;

Input.isKeyDown(Keys.Up);
```

`Input` also contains an `inputMap` dictionary that associates strings to `Key` values. With the `inputMap` set, you can check for key events by passing in a string. This makes it easy to change the actual `Key` mid-game, as the string we use to check never changes.
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

Renderer automatically initializes `Input.inputMap` with default values found in `defaults/EngineDefaults.cs`, but this can be changed at any time in your game.



# The `Node` class
_The `Node` class is the engine's basic drawable object, and is the base class for other objects such as `GameObject` and `TextObject`. A `Node` on its own, however, will not render anything to the screen, but instead can be thought of as a parent for other object. Children added to a node will respect its relative coordiate system and bounds._

## Members

`public float X`

The X coordinate of the node within its parent;

`public float Y`

The Y coordinate of the node within its parent;

`public Vector2 Origin`

The offset within a `Node`'s bounds to place `X` and `Y`.

When `Origin` is set, `Bounds.X` is automatically set to `-(Origin.X/2)`, and `Bounds.Y` is set to `-(Origin.Y/2)`. 

`public Rectangle Bounds`

The bounding box around a `Node`'s `X` and `Y`. `Bounds.X` and `Bounds.Y` represent the distance that the `Bounds`'s top left corner is from `X` and `Y`.

In most cases, it is sufficient to set _either_ `Bounds` or `Origin`, but not both. 

`public bool ShowBounds = false`

If `true`, the bounding box of a `Node` will be filled in with the color/ alpha specified by `BoundsColor` and `BoundsAlpha`.

`public Color BoundsColor = Color.Blue`

`public float BoundsAlpha = 0.5f`

`public bool IsHidden = false`

## Methods

`public void AddChild(Node n)`

Adds `Node` _n_ to the current `Node` as a child, iff _n_ does not yet have a parent.

`public void RemoveFromParent(bool isMe = true)`

Removes the calling `Node` from its parent, and recurrsively removes all children nodes as well. 

## Node Examples

```c#
public class Tile : GameObject {
  // Tile game logic here 
}

public class Level : Node {
  private List<Tile> tiles;
  private int Tile_W = 20;
  private int Tile_H = 10;

  public Level() {
    // add a bunch of children to our node
    for (int j = 0; j < 10; j++) {
      for (int i = 0; i < 10; i++) {
        Tile t = new Tile(i * Tile_w, j * Tile_H);
        AddChild(t);
      }
    }

    // if we change the parent's coordiates, all children will be affected
    X = 20;
  }
}
```




# The `GameObject` class

`Public GameObject : Node`

_The `GameObject` class displays a sprite on screen. The image provided to a `GameObject` can function as the whole sprite, or as a sprite sheet. Bounds and collision detection are also available._

As noted above, we use `Node.addChild(obj)` to add a `GameObject` to an existing `Node` as a child. 

## Sprite Dimension vars:

`protected readonly int imageWidth`

`protected readonly int imageHeight`

`public readonly int spriteWidth`

`public readonly int spriteHeight`

_`imageWidth` and `imageHeight` specify the dimensions of the image provided to the `GameObject`. If the image is supplied as a spritesheet (see `setSpriteSheet()` below), `spriteWidth` and `spriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions._


`public float direction`

`public int colisionLayer`

`public float spriteRotation`

`public float spriteScale`

`public Vector2 center`

`public bool showBounds`

## Other vars:

`public float layerDepth`

`public Color drawColor`

`public bool isHidden`

`public Color boundsColor = Color.Blue`;

`public float boundsAlpha = 0.5f`;

## Creating a `GameObject` with an image
```c#
using Engine

public class Player: GameObject {

  public Player() {}

  public override void load(ContentManager content) {
    setImage(TextureLoader.Load("myimage.png"))
  }
}
```

Sets the `GameObject`'s image as a single sprite. Drawing the `GameObject` will display the entire image, unless a `spriteClip` is specified. Changing the values of `GameObject.animation` or `GameObject.currentFrame` will have no effect.

## Creating a `GameObject` with a sprite sheet
```C#
using Engine

public class Player: GameObject {

  public player() {}

  public override void load(ContentManager content) {
    setSpriteSheet(TextureLoader.Load("mysheet.png"), 4, 4);
  }
}
```

Sets the `GameObject`'s image as a sprite sheet, cut into a grid specified by cols and rows (in this case, 4 x 4). Choosing this allows for the use of `animation` and `currentFrame` to pick the sprite to display, as well as `spriteClip`.

## Using a `GameObject`'s sprite sheet

_If a GameObject's image is supplied as a sprite sheet, there are a few ways to specify which frame or section of the sheet to show. The following are specified in order of precidence (if one is set, the ones below are automatically updated to match)._

1. `protected Animation animation`
2. `protected int currentFrame`
3. `protected Rectangle spriteClip`

## Using a `GameObject`'s `animation`

GameObject will automatically run the supplied `animation`, and will display the sprite based on the animation's current frame. `GameObject.currentFrame` will be updated to this value, and `GameObject.spriteClip` will represent the bounds of the sprite on the sheet. User setting either `GameObject.currentFrame` or `GameObject.spriteClip` in this case will have no effect, as these values are computed every frame.

`Animation` is used so that the user can define custom lengths for each frame, rathen than being forced into a linear frame time. Internally, every frame the engine will call the supplied `animation`'s `update(GameTime gameTime)` method, and casts the return value to an int to be used as the `currentFrame`. The `animation`'s `animationType` should be set to `AnimationType.STEP` for the easiest results. 

Below is an example of setting a custom looping frame animation that animates through a sprite sheet's frames 5 through 10.
```c#
public class MyClass : GameObject {
  Animation runAnimation = new Animation(true, AnimationType.STEP);

  public MyClass() {}

  public override void load(ContentManager content){
    setSpriteSheet(TextureLoader.Load("mysheet.png", content), 4, 4);

    /*
      The sheet added can be though of as a box cut into equal smaller boxes, labeled as follows:
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

    animation = runAnimation;
  }
}
``` 

When `Renderer`'s `Draw` method loops over each `GameObject`, it automatically handles the progression of its animation. As mentioned above, the value of `currentFrame` will be updated during each game loop to reflect the animation's current position. The same applys to `spriteClip`.

## Using a `GameObject`'s `currentFrame`

For situations where you don't necessarily want to _animate_ changes between different sprite sheet frames, but still want control over which frame to display, use `GameObject.spriteClip`.

```c#
public class MyClass : GameObject {
  int hp = 10;
  int healthyFrame = 10;
  int lowHealthFrame = 11;
  int veryLowHealthFrame = 12;

  public MyClass() {}

  public override void load(ContentManager content){
    setSpriteSheet(TextureLoader.Load("mysheet.png", content), 4, 4);

    currentFrame = healthyFrame;
  }

  public void takeDamage() {
    hp--;

    if (hp < 5) {
      currentFrame = lowHealthFrame;
    }
    else if (hp < 2) {
      curretFrame = veryLowHealthFrame;
    }
  }
}
```

## Using a `GameObject`'s `spriteClip`

It is certainly possible to use a spritesheet that contains cells of different sizes. For this, use `spriteClip` to specify a rectangle on the sheet to display.

## `GameObject` collision

TODO

# The `FontLib` class

`public FontLib(string fontPath, GraphicsDevice graphics)`

_Used to create fonts for a specific font file. For now, ttf files are sourced from the client game folder's `content` directory._

```c#
FontLib FontLibrary = new FontLib("path/to/my/font", GraphicsDevice);
Font FontReg = FontLibrary.CreateFont(20);
Font FontLarge = FontLibrary.CreateFont(50);
```

# The `Font` class

TLDR: To draw text with a `TextObject`, you must supply a `Font`, which is made from a `FontLib` and a specific point size.

`Font`s are used as a cache for textures created from a `FontLib` at a particular point size. The first time we need a character, the `Font` will generate a texture for it, and use metrics from the `FontLib` to specify how it should be placed when drawing to the screen. 

__

# The `TextObject` class

`Public TextObject : Node`

_`TextObject`s are special nodes use for drawing text to the screen. When drawn, the text is rendered_

## Vars

`public Font Font`

The `Font` property just calls `SetText` when set.

`public string Text`

`public VerticalAlignment VerticalAlignment = VerticalAlignment.NONE`

Uses the `VerticalAlignment` enum:

```c#
public enum VerticalAlignment {
    NONE,
    TOP,
    CENTER
  }
```

`public Color Color = Color.White`

## Methods

`public TextObject(Font font = null, int x = 0, int y = 0, string text = null)`

If the provided `Font` is null, the default system font will be used.

`public void SetText(string t)`

# `Animation` Examples

TODO
