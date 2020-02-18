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

_Every frame, `Renderer` will loop over all `Nodes` added via `addChildToRoot()`, as well as all of those `Nodes'` children added via `AddChild()`, and call their `draw` method. `Renderer` also provides a virtual `updateGame()` method, but this does nothing about of the box. `updateGame()` should be overridden in whatever class is derrived from `Renderer`._

## Initialization

1. When `Renderer's` constructor is called, a few important XNA attributes are initialized, including the `GraphicsDeviceManager` and the `Content` directory location. 
2. FNA calls `Renderer.initalize()`. This is virtual, and at the very least, the derrived version must call `base.Initialize()` and pass an instance of `EngineSettings`. Generally, this is also where we do custom game setup, such as declaring our initial game objects. See below for an example.
3. FNA calls `Renderer.loadContent()`. This is also virtual are virtual, but generally `loadContent()` is not needed in the derrived class space.

## Game Loop

1. FNA calls `Renderer.Update()`, which is meant to handle behind-the-scenes functions such as refreshing `Input`'s state.
2. `Renderer.updateGame()` is called, which should be overridden in the derrived class. This is the starting point for user-defined game updates.
3. If `Renderer.engineState` is set to `EngineState.QUIT`, the game will exit here.
4. `base.Update` is called, returning control to FNA.
5. FNA calls `Renderer.Draw()`, which draws every `Node` that is a descendent of the root `Node`.
6. Repeat.




# `Renderer` Examples

`public class Renderer : Game`

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
    Car Car;

    public Game1() : base() { }

    override protected void Initialize() {
      // This is where we do the bulk of our setup

      // First, lets use some custom settings.
      EngineSettings gameSettings = new EngineSettings();
      gameSettings.SystemFontPath = "./Content/system_font.ttf";
      gameSettings.AllowResize = false;

      // Now, before we can add anything to our root, we need to
      // initialize the base with our customSettings.
      base.Initialize(customSettings);

      // Lastly, lets set up our actual game objects.
      Car = new Car(); 
      addChildToRoot(Car);
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


## Using `EngineSettings`

`EngineSettings` is a class that encapsulates all settings a `Renderer` can take during initialization, all of which are initialized to defaults found within the static `EngineDefaults` class.

Below is an example using default values. 
```c#
public class MyGame : Renderer {
  public Game1() : base() { }

  override protected void Initialize() {
    EngineSettings gameSettings = new EngineSettings();
    base.Initialize(gameSettings);
  }
}
```

Below is an example using custom values. Since `EngineSettings` is `sealed`, we cannot extend it to add new members, and thus are limited to only settings that the `Renderer` expects.
```c#
public class MyGame : Renderer {
  public Game1() : base() { }

  override protected void Initialize() {
    EngineSettings gameSettings = new EngineSettings();
    gameSettings.SystemFontPath = "./Content/system_font.ttf";
    gameSettings.AllowResize = false;
    gameSettings.StartMouseVisible = true;

    base.Initialize(gameSettings);
  }
}
```



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

`public class Node : IDisposable`

_The `Node` class is the engine's basic drawable object, and is the base class for other objects such as `GameObject` and `TextObject`. A `Node` on its own, however, will not render anything to the screen, but instead can be thought of as a parent for other object. Children added to a node will respect its relative coordiate system and bounds._

## Vars

`public float X`

- The X coordinate of the node within its parent;

`public float Y`

- The Y coordinate of the node within its parent;

`public Rectangle Bounds`

- The bounding box around a `Node's` `X` and `Y`. `Bounds.X` and `Bounds.Y` represent the distance that the `Bounds` top left corner is placed from `X` and `Y`.

`public bool ShowBounds = false`

- If `true`, the bounding box of a `Node` will be filled in with the color/ alpha specified by `BoundsColor` and `BoundsAlpha`.

`public Color BoundsColor = Color.Blue`

`public float BoundsAlpha = 0.5f`

`public bool IsHidden = false`

## Methods

`public void AddChild(Node n)`

- Adds `Node` _n_ to the current `Node` as a child, iff _n_ does not yet have a parent.

`public void RemoveFromParent(bool isMe = true)`

- Removes the calling `Node` from its parent, and recurrsively removes all children nodes as well. 

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

- _`imageWidth` and `imageHeight` specify the dimensions of the image provided to the `GameObject`. If the image is supplied as a spritesheet (see `setSpriteSheet()` below), `spriteWidth` and `spriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions._

`protected Vector2 ImageOrigin`

- The offset within a `GameObjects` image to place `X` and `Y`.

- For example, if a `GameObject` is using an image as its sprite that is 64 * 64 pixles, we can place `X` and `Y` in the center of the sprite by doing the following:

```c#
Origin = new Vector2(32, 32);
```


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

Sets the `GameObject's` image as a single sprite. Drawing the `GameObject` will display the entire image, unless a `spriteClip` is specified. Changing the values of `GameObject.animation` or `GameObject.currentFrame` will have no effect.

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

Sets the `GameObject's` image as a sprite sheet, cut into a grid specified by cols and rows (in this case, 4 x 4). Choosing this allows for the use of `animation` and `currentFrame` to pick the sprite to display, as well as `spriteClip`.

## Using a `GameObject`'s sprite sheet

_If a `GameObject's` image is supplied as a sprite sheet, there are a few ways to specify which frame or section of the sheet to show. The following are specified in order of precidence (if one is set, the ones below are automatically updated to match)._

1. `protected Animation animation`
2. `protected int currentFrame`
3. `protected Rectangle spriteClip`

## Using a `GameObject`'s `animation`

GameObject will automatically run the supplied `animation`, and will display the sprite based on the animation's current frame. `GameObject.currentFrame` will be updated to this value, and `GameObject.spriteClip` will represent the bounds of the sprite on the sheet. User setting either `GameObject.currentFrame` or `GameObject.spriteClip` in this case will have no effect, as these values are computed every frame.

`Animation` is used so that the user can define custom lengths for each frame, rathen than being forced into a linear frame time. Internally, every frame the engine will call the supplied `animation's` `update(GameTime gameTime)` method, and casts the return value to an int to be used as the `currentFrame`. The `animation's` `animationType` should be set to `AnimationType.STEP` for the easiest results. 

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

When `Renderer's` `Draw` method loops over each `GameObject`, it automatically handles the progression of its animation. As mentioned above, the value of `currentFrame` will be updated during each game loop to reflect the animation's current position. The same applys to `spriteClip`.

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

`public class FontLib`

`public FontLib(string fontPath, GraphicsDevice graphics)`

- _Used to create fonts for a specific font file. For now, ttf files are sourced from the client game folder's `content` directory._

```c#
FontLib FontLibrary = new FontLib("path/to/my/font", GraphicsDevice);
Font FontReg = FontLibrary.CreateFont(20);
Font FontLarge = FontLibrary.CreateFont(50);
```

## Note about the `SystemFontLib`

During the initialization phase, the `Renderer` attempts to create the static `SystemFontLib`, based on the path given by the supplied `EngineSettings` object. For this reason, the `EngineSettings` object must contain a valid `SystemFontPath`, otherwise the `Renderer` will crash.

# The `Font` class

`public class Font`

TLDR: To draw text with a `TextObject`, you must supply a `Font`, which is made from a `FontLib` and a specific point size.

`Fonts` are used as a cache for textures created from a `FontLib` at a particular point size. The first time we need a character, the `Font` will generate a texture for it, and use metrics from the `FontLib` to specify how it should be placed when drawing to the screen. 

__

# The `TextObject` class

`Public TextObject : Node`

_`TextObjects` are special nodes use for drawing text to the screen. When drawn, the text is rendered_

## Vars

`public Font Font`

- The `Font` property just calls `SetText` when set.

`protected Vector2 TextOrigin`

`public string Text`

`public VerticalAlignment VerticalAlignment = VerticalAlignment.NONE`

- Uses the `VerticalAlignment` enum:

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

- If the provided `Font` is null, the default system font will be used.

`public void SetText(string t)`

# The `Element` Class

`public class Element : GameObject`

_`Elements` are special `GameObjects` that assist with building UIs like menus._

## Vars

`public bool Selected = false`
`public bool IsSelectable = false`

`public int TopOffset = 0`

`public int LeftOffset = 0`

`public Color BackgroundColor = EngineDefaults.ButtonBackgroundColor`

`public float BackgroundAlpha = EngineDefaults.ButtonBackgroundAlpha`

`public Color SelectedColor = EngineDefaults.ButtonSelectedColor`

`public float SelectedAlpha = EngineDefaults.ButtonSelectedAlpha`

`public Color TextColor = EngineDefaults.ButtonTextColor`

`public Color TextSelectedColor = EngineDefaults.ButtonTextSelectedColor`

`public TextObject Label`

## Methods

`public Element(Font font = null)`

- All `Elements` contain one `TextObject` called `Label` to display basic text. It will be rendered in the default engine font unless another is specified.

- By Default, `Elements` are not selectable.

- `Elements` and their derrived classes (`Button`, `KeySwitcher`, etc...) are meant to be added with `AddChildAsElement`, which handles placing `Elements` within their parent. It is for this reason that the `Element` constructor does not take parameters for `X`, `Y`, or other placement related attribute. However, Since `Elements` are derrived from `Node`, it is possible to manually set these attributes if `AddChildAsElement` isn't fit for the situation.

`public virtual void Update(float mouseX, float mouseY)`

- Base version of `Update` handles mouse and keyboard interactions for selecting/ unselecting the `Element` and its direct children. It also loops over all child `Nodes` and calling their `Update` methods. 

`public virtual void SetSelected()`

`public virtual void SetUnselected()`

- These do nothing out of the box, but should be overridden to specifiy selection behavior in the derrived class.

`private void UnselectAllChildren()`

- Sets all child `Nodes` to their unselected state, by calling `SetUnselected()` on each.

`public void AddChildAsElement(Element el, int left, int right)`

- Adds the `Element` normally (calls `AddChild()` internally), and also increments the calling `Element's` `LeftOffset` and `RightOffset` values based on the passed in `left` and `right` params. `el.X` and `el.Y` are set to these updated offset values. 


In the example below, we can easily set the location of each button by specifying its offset from the last.

```c#
public sealed class PauseMenuHome : Element {
  const int BTN_MARGIN = 50;

    public PauseMenuHome() {
      Button resumeButton = new Button();
      resumeButton.Label.Text = "Resume Game";
      AddChildAsElement(resumeButton, 0, 0);

      Button settingsButton = new Button();
      settingsButton.Label.Text = "Settings";
      AddChildAsElement(settingsButton, 0, BTN_MARGIN);

      Button aboutButton = new Button();
      aboutButton.Label.Text = "About";
      AddChildAsElement(aboutButton, 0, BTN_MARGIN);

      Button exitButton = new Button();
      exitButton.Label.Text = "Exit Game";
      AddChildAsElement(exitButton, 0, BTN_MARGIN);
    }
}
```

# The `Button` Class

`Public Button : Element`

`Buttons` are special `Elements` that are selectable and call an `OnClick` `Action` when clicked.

## Vars

`public Action OnClick`

- The default `Update` method for a `Button` will trigger its `OnClick` callback when clicked, or when it is selected and the primary key is pressed.

- For example, `OnClick` is attached like so:

```c#
public class PauseMenu : Element {

  public PauseMenu(Renderer renderer) {
    Renderer.AddChildToRoot(this);

    // create the button element
    Button resumeButton = new Button();

    // set the buttons OnClick to our callback
    resumeButton.OnClick = () => { closeMenu(); };

    // give the button some text
    resumeButton.Label.Text = "Resume Game";

    // add our button to our parent element
    AddChildAsElement(resumeButton, 0, 0);
  }

  // our actual method to call, in this case it closes the menu
  public void CloseMenu() {
    Renderer.engineState = EngineState.RUNNING;
    RemoveFromParent();
  }
}
```

## Methods

`public Button(Font font = null) : base(font)`

- The `Button` constructor enables `ShowBounds` and `IsSelectable`.

`public override void SetSelected()`

- Sets `.Selected` to `true`. Sets `.BoundsColor` to `SelectedColor`, as well as `.BoundsAlpha` to `SelectedAlpha`. Also sets `.Label.Color` to `TextSelectedColor`.

`public override void SetUnselected()`

- Sets `.Selected` to `false`, and resets `.BoundsColor` to `BackgroundColor`, `.BoundsAlpha` to `BackgroundAlpha`, and `Label.Color` to `TextColor`.

`public override void Update(float mouseX, float mouseY)`

- If an `OnClick` action has been specified, `Update` will check if either the mouse left click has occured within the `Button's` bounds, or if the `Button` is selected and the primary key is pressed. If so, `OnClick` is called. `Update` concludes by calling the base `Element` `Update` method.

If a `Button` is a child of an `Element`, that `Element` will handle calling the `Button's` lifecycle events (assuming the parent, or some ancestor `Element` has its `.Update` method called). For this reason, you should not need to manually invoke a `Button's` `Update`, `SetSelected`, or `SetUnselected` unless it is used in a situation that `Element` does not support out of the box.

# `Animation` Examples

TODO





#
#
#
#
#
#

<!--  <<< Template >>> -->
## Readme API Template

# The `Class` Class

_Description_

Other info

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