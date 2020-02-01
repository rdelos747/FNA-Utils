using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  public sealed class Button : Element {

    public Action OnClick;

    // public Color BackgroundColor = EngineDefaults.ButtonBackgroundColor;
    // public float BackgroundAlpha = EngineDefaults.ButtonBackgroundAlpha;
    // public Color SelectedColor = EngineDefaults.ButtonSelectedColor;
    // public float SelectedAlpha = EngineDefaults.ButtonSelectedAlpha;
    // public Color TextColor = EngineDefaults.ButtonTextColor;
    // public Color TextSelectedColor = EngineDefaults.ButtonTextSelectedColor;

    // public TextObject Text;

    public Button(int x = 0, int y = 0, int w = EngineDefaults.ButtonWidth, int h = EngineDefaults.ButtonHeight, Font font = null) : base(x, y, w, h) {
      ShowBounds = true;
      IsSelectable = true;

      // Text = new TextObject(font);
      // Text.X = 10;
      // Text.Y = Bounds.Height / 2;
      // Text.VerticalAlignment = VerticalAlignment.CENTER;
      // AddChild(Text);
    }

    public override void SetSelected() {
      Selected = true;
      BoundsColor = SelectedColor;
      BoundsAlpha = SelectedAlpha;
      //Text.Color = TextSelectedColor;
      Label.Color = TextSelectedColor;
    }

    public override void SetUnselected() {
      Selected = false;
      BoundsColor = BackgroundColor;
      BoundsAlpha = BackgroundAlpha;
      //Text.Color = TextColor;
      Label.Color = TextColor;
    }

    public override void Update(float mouseX, float mouseY) {
      if (OnClick != null) {
        if (Input.mouseLeftClicked() && pointInBounds(mouseX, mouseY)) {
          OnClick();
        }

        if (Input.keyPressed(EngineDefaults.keyPrimary) && Selected) {
          OnClick();
        }
      }

      base.Update(mouseX, mouseY);
    }
  }
}
