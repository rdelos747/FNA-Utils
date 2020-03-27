using System;
using Microsoft.Xna.Framework;

namespace Utils {

  public sealed class KeySwitcher : Element {

    public Action OnClick;

    public Label KeyLabel;

    public KeySwitcher(string key, Font font = null) : base(font) {
      IsHidden = false;
      IsSelectable = true;

      Bounds = new Rectangle(0, -(MenuDefaults.ButtonHeight / 2), MenuDefaults.ButtonWidth, MenuDefaults.ButtonHeight);
      Label.Text = key;
      Label.VerticalAlignment = VerticalAlignment.CENTER;

      ShowCenter = true;

      KeyLabel = new Label(font);
      KeyLabel.VerticalAlignment = VerticalAlignment.CENTER;
      KeyLabel.Text = Input.KeyAsText(key);
      KeyLabel.Position.X = 200;

      AddChild(KeyLabel);
    }

    public override void SetSelected() {
      Selected = true;
      Color = SelectedColor;
      Alpha = SelectedAlpha;
      Label.Color = TextSelectedColor;
    }

    public override void SetUnselected() {
      Selected = false;
      Color = BackgroundColor;
      Alpha = BackgroundAlpha;
      Label.Color = TextColor;
    }
  }
}