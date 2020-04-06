using Microsoft.Xna.Framework;

namespace Utils {

  public class Size {

    public float Width;
    public float Height;

    public Size(float Width, float Height) {
      this.Width = Width;
      this.Height = Height;
    }

    public static Size operator +(Size s1, Size s2) {
      return new Size(s1.Width + s2.Width, s1.Height + s2.Height);
    }

    public static Vector2 operator +(Size s, Vector2 v) {
      return v + new Vector2(s.Width, s.Height);
    }

    override public string ToString() {
      return "{Width:" + Width + " Height:" + Height + "}";
    }
  }
}