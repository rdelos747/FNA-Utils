using System;
using Microsoft.Xna.Framework;

namespace Utils {

  public sealed class KeySwitcher : Element {

    public Action OnClick;

    public TextObject KeyLabel;

    public KeySwitcher(string key, Font font = null) : base(font) {
      ShowBounds = true;
      IsSelectable = true;

      Bounds = new Rectangle(0, -(MenuDefaults.ButtonHeight / 2), MenuDefaults.ButtonWidth, MenuDefaults.ButtonHeight);
      Label.Text = key;
      Label.VerticalAlignment = VerticalAlignment.CENTER;

      ShowCenter = true;

      KeyLabel = new TextObject(font);
      KeyLabel.VerticalAlignment = VerticalAlignment.CENTER;
      KeyLabel.Text = Input.KeyAsText(key);
      KeyLabel.X = 200;

      AddChild(KeyLabel);
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
  }
}