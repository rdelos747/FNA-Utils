using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine {
  public static class EngineDefaults {

    public const int Width = 1280;
    public const int Height = 720;

    public const bool FullScreen = false;
    public const bool AllowResize = true;
    public const bool MouseVisible = false;

    public static readonly Color BackgroundColor = Color.Black;

    public static readonly Color PauseMenuBackground = Color.Black;
    public const float PauseMenuAlpha = 0.5f;

    public const int FontSizeReg = 20;
    public const int FontSizeLarge = 50;

    public const int ElementTextPad = 10;

    public const int ButtonWidth = 100;
    public const int ButtonHeight = 50;
    public static readonly Color ButtonBackgroundColor = Color.Black;
    public const float ButtonBackgroundAlpha = 0.0f;
    public static readonly Color ButtonSelectedColor = Color.White;
    public const float ButtonSelectedAlpha = 0.1f;
    public static readonly Color ButtonTextColor = Color.White;
    public static readonly Color ButtonTextSelectedColor = Color.White;

    public const int MenuTop = 200;
    public const int MenuLeft = 50;

    public const string KeyPrimary = "primary";
    public const string KeySecondary = "secondary";
    public const string KeyPause = "pause";
    public const string KeyQuit = "quit";
    public const string KeyUp = "up";
    public const string KeyDown = "down";
    public const string KeyLeft = "left";
    public const string KeyRight = "right";

    public static readonly Dictionary<string, Keys> InputMap = new Dictionary<string, Keys>() {
      {KeyPrimary, Keys.X},
      {KeySecondary, Keys.Z},
      {KeyUp, Keys.Up},
      {KeyDown, Keys.Down},
      {KeyLeft, Keys.Left},
      {KeyRight, Keys.Right},
      {KeyPause, Keys.Enter},
      {KeyQuit, Keys.Escape}
    };
  }
}