using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Utils {

  public sealed class Button : Element {

    public Action OnClick;

    public Button(Font font = null) : base(font) {
      Bounds.IsHidden = false;
      IsSelectable = true;

      Bounds.Rect = new Rectangle(0, -(MenuDefaults.ButtonHeight / 2), MenuDefaults.ButtonWidth, MenuDefaults.ButtonHeight);

      ShowCenter = true;
    }

    public override void SetSelected() {
      Selected = true;
      Bounds.Color = SelectedColor;
      Bounds.Alpha = SelectedAlpha;
      Label.Color = TextSelectedColor;
    }

    public override void SetUnselected() {
      Selected = false;
      Bounds.Color = BackgroundColor;
      Bounds.Alpha = BackgroundAlpha;
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
