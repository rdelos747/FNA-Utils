using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine {
  public static class EngineDefaults {

    public const int width = 1280;
    public const int height = 720;

    public const bool fullScreen = false;
    public const bool allowResize = true;
    public const bool mouseVisible = false;

    public const string fontPath = "content/system_font.ttf";
    public const int fontSize = 20;

    public const string keyPrimary = "primary";
    public const string keySecondary = "secondary";
    public const string keyPause = "engine_pause";
    public const string keyQuit = "engine_quit";
    public const string keyUp = "up";
    public const string keyDown = "down";
    public const string keyLeft = "left";
    public const string keyRight = "right";

    public static readonly Dictionary<string, Keys> inputMap = new Dictionary<string, Keys>() {
      {keyPrimary, Keys.X},
      {keySecondary, Keys.Z},
      {keyUp, Keys.Up},
      {keyDown, Keys.Down},
      {keyLeft, Keys.Left},
      {keyRight, Keys.Right},
      {keyPause, Keys.Enter},
      {keyQuit, Keys.Escape}
    };
  }
}