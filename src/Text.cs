using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace Engine {

  public class Text : Node {
    private string _text;
    private Font font;
    private List<(char, Vector2)> points;

    public string text {
      get {
        return _text;
      }
      set {
        setText(value);
      }
    }

    public Text(Font font) {
      this.font = font;
      points = new List<(char, Vector2)>();
    }

    protected override void Dispose(bool disposing) {
      if (disposing == true) {
        // image.Dispose();
        // TODO:
        // finish this at some point
      }
    }

    public void draw(SpriteBatch spriteBatch, int x, int y, Color c) {
      for (int i = 0; i < points.Count; i++) {
        GlyphData glyph = font.getGlyph(points[i].Item1);
        spriteBatch.Draw(
          glyph.texture,
          new Vector2(points[i].Item2.X + x, points[i].Item2.Y + y),
          new Rectangle(0, 0, glyph.width, glyph.height),
          c
        );
      }
    }

    public void setText(string t) {
      // modfied version of 
      // https://bitbucket.org/jacobalbano/fnt/src/default/
      _text = t;

      points.Clear();

      if (_text.Length == 0) {
        return;
      }

      // loop over every char and position its glyph
      int px = 0;
      int py = 0;
      //float underrun = 0;
      //float overrun = 0;
      for (int i = 0; i < _text.Length; i++) {
        char c = _text[i];

        // do special char stuff
        switch (c) {
          case ' ':
            px += font.advanceSpace;
            continue;
          case '\t':
            px += font.advanceSpace * font.tabSpaces;
            continue;
          case '\r':  //  ignore
            continue;
          case '\n':
            //firstInLine = true;
            //underrun = overrun = 0;

            px = 0;
            py += font.lineHeight;

            //maxWidth = Math.Max(maxWidth, currLineWidth);
            //maxHeight = Math.Max(maxHeight, currLineHeight);
            continue;
        }

        GlyphData glyph = font.getGlyph(c);
        if (glyph == null) {
          throw new Exception($"Invalid character '{c}'");
        }

        int gx = px + glyph.bitmapLeft;
        int gy = py + font.ascender - glyph.bearingY;
        points.Add((glyph.c, new Vector2(gx, gy)));
        px += glyph.advanceX;

        // //kerning for next character
        // if (!doKern) {
        //   continue;
        // }
        // if (i < _text.Length - 1) {
        //   GlyphData nextGlyph = font.getGlyph(_text[i + 1]);
        //   if (nextGlyph == null) {
        //     continue;
        //   }
        //   int kern = getKerning(glyph, nextGlyph);
        //   // sanity check for some fonts that have kern way out of whack
        //   if (kern > glyph.advanceX * 5 || kern < -(glyph.advanceX * 5)) {
        //     kern = 0;
        //   }
        //   px += kern;
        // }
      }
    }

    // private int getKerning(GlyphData gl, GlyphData gr) {
    //   if (!font.hasKerning) {
    //     return 0;
    //   }

    //   if (!font.kerning.TryGetValue(gl.c, out var subKerning))
    //     font.kerning[gl.c] = subKerning = new Dictionary<char, int>();

    //   if (!subKerning.TryGetValue(gr.c, out var kern))
    //     subKerning[gr.c] = kern = (int)font.getKerning(gl.index, gr.index).X;

    //   return kern;
    // }
  }
}