using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine {

  public sealed class EngineSettings {

    public Color BackgroundColor = EngineDefaults.BackgroundColor;

    public Dictionary<string, Keys> InputMap = EngineDefaults.InputMap;
    public string SystemFontPath = "./system-font.ttf";

    public int VirtualWidth = EngineDefaults.Width;
    public int VirtualHeight = EngineDefaults.Height;
    public int ScreenWidth = EngineDefaults.Width;
    public int ScreenHeight = EngineDefaults.Height;

    public bool StartFullscreen = EngineDefaults.FullScreen;

    public bool AllowResize = EngineDefaults.AllowResize;

    public bool StartMouseVisible = EngineDefaults.MouseVisible;

    public EngineSettings() { }
  }
}