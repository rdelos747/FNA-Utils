using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Utils {

  public class Glyph {
    public char C;
    public Texture2D Texture;
    public Rectangle Clip;
    public int Width = 0;
    public int Height = 0;

    /*
    TTF Specifics
    */
    public uint Index = 0;
    // Bitmaps left bearing, is this needed?
    public int BitmapLeft = 0;
    // Horizontal distance to increment after rendering glyph
    public int AdvanceX = 0;
    // Horizontal distance from pen position to glyphs left bbox edge
    public int BearingX = 0;
    // Vertical distance from baseline to top of glyphs bbx
    public int BearingY = 0;

    public Glyph() { }
  }

  public abstract class Font {
    public int LineHeight;
    public int Ascender;
    public int Advance;
    public int Tab;
    public Dictionary<char, Glyph> Glyphs = new Dictionary<char, Glyph>();

    public Font() { }

    public abstract Glyph GetGlyph(char c);
  }
}