using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public sealed class SpriteSheet {
    public Texture2D SheetTexture { get; private set; }
    public int Cols { get; private set; }
    public int Rows { get; private set; }

    public SpriteSheet(string path, int cols, int rows) {
      SheetTexture = TextureLoader.Load(path);
      Cols = cols;
      Rows = rows;
    }
  }
}