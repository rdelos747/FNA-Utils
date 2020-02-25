using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public sealed class Button : Element {

    public Action OnClick;

    public Button(Font font = null) : base(font) {
      ShowBounds = true;
      IsSelectable = true;

      Bounds = new Rectangle(0, -(MenuDefaults.ButtonHeight / 2), MenuDefaults.ButtonWidth, MenuDefaults.ButtonHeight);

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
      // use our menu controller, or the passed in menu controller

      if (MC != null && OnClick != null) {
        if (Input.MouseLeftClicked() && pointInBounds(mouseX, mouseY)) {
          OnClick();
        }


        if (Input.KeyPressed(MC.KeySelect) && Selected) {
          OnClick();
        }
      }

      base.Update(mouseX, mouseY);
    }
  }
}
