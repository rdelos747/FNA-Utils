using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utils {

  public static class MenuDefaults {

    /*
    General Elements
    */
    public const int ElementTextPad = 10;

    /*
    Buttons
    */
    public const int ButtonWidth = 100;
    public const int ButtonHeight = 50;
    public static readonly Color ButtonBackgroundColor = Color.Black;
    public const float ButtonBackgroundAlpha = 0.0f;
    public static readonly Color ButtonSelectedColor = Color.White;
    public const float ButtonSelectedAlpha = 0.1f;
    public static readonly Color ButtonTextColor = Color.White;
    public static readonly Color ButtonTextSelectedColor = Color.White;
  }
}