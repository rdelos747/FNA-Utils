using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public class Atlas : Font {

    public Atlas(SpriteSheet sheet) {
      GenerateBounds(sheet);
      Advance = Glyphs[' '].Width;
    }

    public override Glyph GetGlyph(char c) {
      Glyph glyph;
      if (!Glyphs.TryGetValue(c, out glyph)) {
        glyph = Glyphs['?'];
      }
      return glyph;
    }

    public void GenerateBounds(SpriteSheet sheet) {
      Color[] colors = new Color[sheet.Texture.Width * sheet.Texture.Height];
      sheet.Texture.GetData(colors);
      for (int j = 0; j < sheet.Rows; j++) {
        for (int i = 0; i < sheet.Cols; i++) {
          char c = (char)(((j * sheet.Cols) + i) + 32);
          Glyphs.Add(c, CreateGlyphFromSheet(c, i, j, colors, sheet));
          LineHeight = Math.Max(LineHeight, Glyphs[c].Height);
        }
      }
    }

    public Glyph CreateGlyphFromSheet(char c, int col, int row, Color[] colors, SpriteSheet sheet) {
      int start = (row * sheet.Texture.Width * sheet.Height) + (col * sheet.Width);
      int top = 0;
      int left = 0;
      int right = 0;
      int bottom = 0;

      /*
      Move to top left point
      */
      while (colors[start + left + (top * sheet.Texture.Width)] == Color.Black && top < sheet.Height) {
        left++;
        if (left == sheet.Width) {
          left = 0;
          top += 1;
        }
      }

      /*
      Move to right point
      */
      while (
        colors[start + left + (top * sheet.Texture.Width) + right] != Color.Black &&
        right < sheet.Width - left
      ) {
        right++;
      }

      /*
      Move to bottom point
      */
      while (
        colors[start + left + (top * sheet.Texture.Width) + (bottom * sheet.Texture.Width)] != Color.Black &&
        bottom < sheet.Height - top
      ) {
        bottom++;
      }

      Glyph glyph = new Glyph();
      glyph.Width = right;
      glyph.AdvanceX = right;
      glyph.Height = bottom;
      glyph.C = c;
      glyph.Texture = sheet.Texture;
      glyph.Clip = new Rectangle((col * sheet.Width) + left, (row * sheet.Height) + top, right, bottom);

      return glyph;
    }
  }
}