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

[The Node Class](#the-node-class)

[Input](#input)

[Resolution](#resolution)

[GameObjects](#gameobjects)

[TextureLoader](#textureloader)

[TextObjects](#textobjects)

[Fonts](#fonts)

[Animations](#animations)

[Random](#rand)

[Menu Helpers](#menu-helpers)

[The Element Class](#the-element-class)

[Button Elements](#button-elements)

[KeySwitcher Elements](#keyswitcher-elements)

[List Elements](#List-elements)

# Node Class

A basic drawable component, handling simple data such as X & Y position and boundaries. Nodes can be used as containers for other Nodes, and serve as the base class for other drawable components such as [GameObjects](#gameobjects) and [TextObjects](#textobjects).

## Creating a Node

###### constructor

`public Node()`

## Position and Bounds

###### property

`public float X`

The x coordinate of a Node within its parent.

###### property

`public float Y`

The y coordinate of a Node within its parent.

## Adding Children

## Removing a Node

## Drawing a Node

##

# Input

# Resolution

# GameObjects

# TextureLoader

# TextObjects

# Fonts

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
