using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public sealed class SpriteSheet {
    public Texture2D Texture { get; private set; }
    public int Cols { get; private set; }
    public int Rows { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }

    public SpriteSheet(string path, int cols, int rows) {
      Texture = TextureLoader.Load(path);
      Cols = cols;
      Rows = rows;
      Width = (int)(Texture.Width / Cols);
      Height = (int)(Texture.Height / Rows);
    }

    public int ColOffset(int idx) {
      return (idx % Cols) * Width;
    }

    public int RowOffset(int idx) {
      return (idx / Cols) * Height;
    }
  }
}