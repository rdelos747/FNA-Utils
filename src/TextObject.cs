using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace Engine {

  public enum VerticalAlignment {
    NONE,
    TOP,
    CENTER
  }

  public class TextObject : Node {

    private Font _font;
    public Font Font {
      get {
        return _font;
      }
      set {
        _font = value;
        Bounds = new Rectangle(0, 0, 10, _font.lineHeight);

        if (_text != null) {
          SetText(_text);
        }
      }
    }

    private List<(char, Vector2)> Points;

    private string _text;
    public string Text {
      get {
        return _text;
      }
      set {
        SetText(value);
      }
    }

    public VerticalAlignment VerticalAlignment = VerticalAlignment.NONE;

    public Color Color = Color.White;

    public TextObject(Font font = null, int x = 0, int y = 0, string text = null) {
      _font = font;
      if (_font == null) {
        _font = EngineDefaults.SystemFontReg;
      }

      Bounds = new Rectangle(0, 0, 10, _font.lineHeight);

      Points = new List<(char, Vector2)>();

      X = x;
      Y = y;
      if (text != null) {
        SetText(text);
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing == true) {
        // image.Dispose();
        // TODO:
        // finish this at some point
      }
    }

    public override void Draw(SpriteBatch spriteBatch, float lastX, float lastY) {
      Vector2 position = new Vector2(lastX + X, lastY + Y);
      if (IsHidden) return;

      for (int i = 0; i < Points.Count; i++) {
        GlyphData glyph = _font.getGlyph(Points[i].Item1);
        spriteBatch.Draw(
          glyph.texture,
          new Vector2(
            (Points[i].Item2.X + position.X) - Origin.X,
            (Points[i].Item2.Y + position.Y) - Origin.Y
          ),
          new Rectangle(0, 0, glyph.width, glyph.height),
          Color
        );
      }

      base.Draw(spriteBatch, lastX, lastY);
    }

    public void SetText(string t) {
      // modfied version of 
      // https://bitbucket.org/jacobalbano/fnt/src/default/
      _text = t;

      Points.Clear();

      if (_text.Length == 0) {
        return;
      }

      // loop over every char and position its glyph
      int px = 0;
      int py = 0;
      int maxWidth = 0;
      int maxHeight = 0;
      for (int i = 0; i < _text.Length; i++) {
        char c = _text[i];

        // do special char stuff
        switch (c) {
          case ' ':
            px += _font.advanceSpace;
            continue;
          case '\t':
            px += _font.advanceSpace * _font.tabSpaces;
            continue;
          case '\r':  //  ignore
            continue;
          case '\n':
            px = 0;
            py += _font.lineHeight;
            continue;
        }

        GlyphData glyph = _font.getGlyph(c);
        if (glyph == null) {
          throw new Exception($"Invalid character '{c}'");
        }

        int gx = px + glyph.bitmapLeft;
        int gy = py + _font.ascender - glyph.bearingY;

        Points.Add((glyph.c, new Vector2(gx, gy)));
        px += glyph.advanceX;

        maxWidth = Math.Max(maxWidth, px);
        maxHeight = Math.Max(maxHeight, py + glyph.height);
      }

      Bounds = new Rectangle(Bounds.X, Bounds.Y, maxWidth, maxHeight);

      if (VerticalAlignment == VerticalAlignment.CENTER) {
        Origin = new Vector2(0, Bounds.Height / 2);
      }
      else if (VerticalAlignment == VerticalAlignment.TOP) {
        Origin = new Vector2(0, 0);
      }
    }
  }
}