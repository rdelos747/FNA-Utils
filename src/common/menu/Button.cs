using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Engine {

  public sealed class Button : Element {

    public Action OnClick;

    public Button(Font font = null) : base(font) {
      ShowBounds = true;
      IsSelectable = true;

      Bounds = new Rectangle(0, -(EngineDefaults.ButtonHeight / 2), EngineDefaults.ButtonWidth, EngineDefaults.ButtonHeight);

      ShowCenter = true;
    }

    public override void SetSelected() {
      Selected = true;
      BoundsColor = SelectedColor;
      BoundsAlpha = SelectedAlpha;
      Label.Color = TextSelectedColor;
    }

    public override void SetUnselected() {
      Selected = false;
      BoundsColor = BackgroundColor;
      BoundsAlpha = BackgroundAlpha;
      Label.Color = TextColor;
    }

    public override void Update(float mouseX, float mouseY) {
      if (OnClick != null) {
        if (Input.MouseLeftClicked() && pointInBounds(mouseX, mouseY)) {
          OnClick();
        }

        if (Input.KeyPressed(EngineDefaults.KeyPrimary) && Selected) {
          OnClick();
        }
      }

      base.Update(mouseX, mouseY);
    }
  }
}
