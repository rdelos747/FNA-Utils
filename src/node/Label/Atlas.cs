using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public class Atlas : Font {
    public Texture2D Texture { get; private set; }
    private int Rows;
    private int Cols;
    private int CellWidth;
    private int CellHeight;

    public Atlas(string path, int cols, int rows) {
      // Texture = TextureLoader.Load(path);
      Texture = Engine.LoadTexture(path);
      Rows = rows;
      Cols = cols;
      CellWidth = (int)(Texture.Width / Cols);
      CellHeight = (int)(Texture.Height / Rows);

      GenerateBounds();
      Advance = Glyphs[' '].Width;
    }

    public override Glyph GetGlyph(char c) {
      Glyph glyph;
      if (!Glyphs.TryGetValue(c, out glyph)) {
        glyph = Glyphs['?'];
      }
      return glyph;
    }

    public void GenerateBounds() {
      Color[] colors = new Color[Texture.Width * Texture.Height];
      Texture.GetData(colors);
      for (int j = 0; j < Rows; j++) {
        for (int i = 0; i < Cols; i++) {
          char c = (char)(((j * Cols) + i) + 32);
          Glyphs.Add(c, CreateGlyphFromSheet(c, i, j, colors));
          LineHeight = Math.Max(LineHeight, Glyphs[c].Height);
        }
      }
    }

    public Glyph CreateGlyphFromSheet(char c, int col, int row, Color[] colors) {
      int start = (row * Texture.Width * CellHeight) + (col * CellWidth);
      int top = 0;
      int left = 0;
      int right = 0;
      int bottom = 0;

      /*
      Move to top left point
      */
      while (colors[start + left + (top * Texture.Width)] == Color.Black && top < CellHeight) {
        left++;
        if (left == CellWidth) {
          left = 0;
          top += 1;
        }
      }

      /*
      Move to right point
      */
      while (
        colors[start + left + (top * Texture.Width) + right] != Color.Black &&
        right < CellWidth - left
      ) {
        right++;
      }

      /*
      Move to bottom point
      */
      while (
        colors[start + left + (top * Texture.Width) + (bottom * Texture.Width)] != Color.Black &&
        bottom < CellHeight - top
      ) {
        bottom++;
      }

      Glyph glyph = new Glyph();
      glyph.Width = right;
      glyph.AdvanceX = right;
      glyph.Height = bottom;
      glyph.C = c;
      glyph.Texture = Texture;
      glyph.Clip = new Rectangle((col * CellWidth) + left, (row * CellHeight) + top, right, bottom);

      return glyph;
    }
  }
}