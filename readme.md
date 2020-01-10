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

_The `Renderer` class manages rendering of GameObjects. This class extends XNA's `Game` class, and mainly takes care of the boilerplate around loading and drawing objects._

_Every frame, `Renderer` will loop over all `GameObjects` added via `addObject()` and call their `draw` method. `Renderer` also provides a virtual `updateObjects()` method, but this does nothing about of the box. `updateObjects()` should be overridden in whatever class is derrived from `Renderer`._

## Initialization

1. `Renderer` constructor intializes `Input.inputMap` and other default game settings, most of which are located in the `/defaults` directory.
2. FNA calls `Renderer.initalize()` and `Renderer.loadContent()`. Both are virtual, but generally `loadContent()` is not needed.

## Game Loop

1. FNA calls `Renderer.Update()`, which is meant to handle behind-the-scenes functions such as refreshing `Input`'s state.
2. Renderer calls its private `defaultUpdate()` method, which will handle any default functionality that the user has not manually overridden. For example, if the default `inputMap` is still present, or if the user defined `inputMap` contains a key `engine_pause` that is triggered, then `defaultUpdate()` will toggle the `engineState` between `RUNNING` and `PAUSED`. Within `defailtUpdate()` is also code for displaying the default pause menu, or whatever pause menu the user attaches to `Renderer.pauseMenu`.
3. If `Renderer.engineState` is set to `EngineState.QUIT`, the game will exit here.
4. If `Renderer.engineState` is set to `EngineState.RUNNING`, `Renderer.updateGame()` is called, which should be overridden in the derrived class. This is the starting point for user-defined game updates.
5. FNA calls `Renderer.Draw()`, which draws every `GameObject` that has been added so far with `Renderer.addObject()`.
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
      addObject(car);
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

In the example above, note that we also created a `GameObject` called Car, and used `addObject()` to add it to our `Renderer`. 



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







# `GameObject` examples

_The `GameObject` class displays a sprite on screen, and allows the user to manipulate various attributes such as the position, direction, etc. The image provided to a `GameObject` can function as the whole sprite, or as a sprite sheet. Bounds and collision detection are also available._

As noted above, we use `Renderer.addObject(obj)` to add our `GameObject`s to the `Renderer`. The calling of `addObject(obj)` automaticaly triggers `obj.init()`, so if your derrived `GameObject` overrides `init()`, remember to call `base.init()` in that function, as forgetting to do so will prevent other `GameObject` methods from working properly within the `Renderer.`

## Sprite Dimension vars:

`protected readonly int imageWidth`

`protected readonly int imageHeight`

`public readonly int spriteWidth`

`public readonly int spriteHeight`

_`imageWidth` and `imageHeight` specify the dimensions of the image provided to the `GameObject`. If the image is supplied as a spritesheet (see `setSpriteSheet()` below), `spriteWidth` and `spriteHeight` will be the dimensions of a single cell on the sheet. Otherwise, sprite dimensions will be the same as image dimensions._

## Position vars:

`public Vector2 position`

`public float direction`

`public Rectangle bounds`

- Initialized to (0, 0), the top left of the image or sprite cell. 

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

When `Renderer` loops over each `GameObject`, it automatically handles the progression of each animation. As mentioned above, the value of `currentFrame` will be updated during each game loop to reflect the animation's current position. The same applys to `spriteClip`.

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

## Removing a `GameObject` from the `Renderer`

`public void removeFromRenderer()`

```c#
public class MyClass : GameObject {
  int hp = 10;

  public MyClass() {}

  public override void load(ContentManager content){
    setSpriteSheet(TextureLoader.Load("mysheet.png", content), 4, 4);
  }

  public void takeDamage() {
    hp--;

    if (hp == 0) {
      removeFromRenderer()
    }
  }
}
```


## `GameObject` collision

TODO


# `Animation` Examples

TODO
