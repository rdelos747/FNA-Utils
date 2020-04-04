using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace Utils {

  public enum VerticalAlignment {
    Top,
    Center
  }

  public enum HorizontalAlignment {
    Left,
    Center,
    Right
  }

  public class Label : Node {

    public static Font BaseFont;

    private Font _font;
    public Font Font {
      get {
        return _font;
      }
      set {
        _font = value;
        Size = new Vector2(10, _font.lineHeight);

        if (_text != null) {
          SetText(_text);
        }
      }
    }

    protected List<(char, Vector2)> Points;

    private string _text;
    public string Text {
      get {
        return _text;
      }
      set {
        SetText(value);
      }
    }

    public Vector2 TextOrigin = new Vector2();
    private VerticalAlignment _verticalAlignment;
    public VerticalAlignment VerticalAlignment {
      get {
        return _verticalAlignment;
      }
      set {
        _verticalAlignment = value;

        Size = new Vector2(10, _font.lineHeight);

        if (_text != null) {
          SetText(_text);
        }
      }
    }
    private HorizontalAlignment _horizontalAlignment;
    public HorizontalAlignment HorizontalAlignment {
      get {
        return _horizontalAlignment;
      }
      set {
        _horizontalAlignment = value;

        Size = new Vector2(10, _font.lineHeight);

        if (_text != null) {
          SetText(_text);
        }
      }
    }

    public Color Color = Color.White;
    public float DrawDepth = 0;

    public Label(Font font = null) : this(null, 0, 0, HorizontalAlignment.Left, VerticalAlignment.Top, font) { }

    public Label(string text, int x, int y) : this(text, x, y, HorizontalAlignment.Left, VerticalAlignment.Top, null) { }

    public Label(string text, int x, int y, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, Font font = null) {
      _font = font;
      if (_font == null) {
        _font = BaseFont;
      }

      Size = new Vector2(10, _font.lineHeight);

      Points = new List<(char, Vector2)>();

      Position.X = x;
      Position.Y = y;
      _horizontalAlignment = horizontalAlignment;
      _verticalAlignment = verticalAlignment;
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
      if (IsHidden) return;

      Vector2 position = new Vector2(lastX + Position.X, lastY + Position.Y);

      for (int i = 0; i < Points.Count; i++) {
        GlyphData glyph = _font.getGlyph(Points[i].Item1);
        // spriteBatch.Draw(
        //   glyph.texture,
        //   new Vector2(
        //     (Points[i].Item2.X + position.X) - TextOrigin.X,
        //     (Points[i].Item2.Y + position.Y) - TextOrigin.Y
        //   ),
        //   new Rectangle(0, 0, glyph.width, glyph.height),
        //   Color
        // );
        spriteBatch.Draw(
          // glyph.texture,
          //   new Vector2(
          //   (Points[i].Item2.X + position.X) - TextOrigin.X,
          //   (Points[i].Item2.Y + position.Y) - TextOrigin.Y
          // ),
          glyph.texture,
            new Vector2(
            (Points[i].Item2.X + position.X),
            (Points[i].Item2.Y + position.Y)
          ),
          null,
          Color,
          0.0f,
          TextOrigin,
          1,
          SpriteEffects.None,
          DrawDepth
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

      Size = new Vector2(maxWidth, maxHeight);

      TextOrigin = Vector2.Zero;
      if (_horizontalAlignment == HorizontalAlignment.Center) {
        TextOrigin.X = Size.X / 2;
      }
      else if (_horizontalAlignment == HorizontalAlignment.Right) {
        TextOrigin.X = Size.X;
      }

      if (_verticalAlignment == VerticalAlignment.Center) {
        TextOrigin.Y = Size.Y / 2;
      }
    }
  }
}