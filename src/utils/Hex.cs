using System.Globalization;
using Microsoft.Xna.Framework;

namespace Utils {
  public sealed class Hex {

    public static Color ToColor(string hex) {
      if (hex.IndexOf('#') != -1) {
        hex = hex.Replace("#", "");
      }
      int r, g, b = 0;

      r = int.Parse(hex.Substring(0, 2), NumberStyles.AllowHexSpecifier);
      g = int.Parse(hex.Substring(2, 2), NumberStyles.AllowHexSpecifier);
      b = int.Parse(hex.Substring(4, 2), NumberStyles.AllowHexSpecifier);

      return new Color(r, g, b);
    }

    public const string DarkBlue1 = "#1d2b53";
    public const string DarkBlue2 = "#20273e";
    public const string DarkBlue3 = "#091536";
    public const string DarkBlue4 = "#576ba9";
    public const string DarkBlue5 = "#6d7ca9";
    public const string Purple1 = "#7e2552";
    public const string Purple2 = "#5f2c46";
    public const string Purple3 = "#520c30";
    public const string Purple4 = "#bf5a8d";
    public const string Purple5 = "#bf749a";
    public const string DarkGreen1 = "#008751";
    public const string DarkGreen2 = "#196547";
    public const string DarkGreen3 = "#005834";
    public const string DarkGreen4 = "#31c388";
    public const string DarkGreen5 = "#58c398";
    public const string Brown1 = "#ab5236";
    public const string Brown2 = "#804e3e";
    public const string Brown3 = "#6f2812";
    public const string Brown4 = "#d58268";
    public const string Brown5 = "#d59885";
    public const string DarkBrown1 = "#5f574f";
    public const string DarkBrown2 = "#47433e";
    public const string DarkBrown3 = "#3e2c1a";
    public const string DarkBrown4 = "#afa499";
    public const string DarkBrown5 = "#afa79f";
    public const string Red1 = "#ff004d";
    public const string Red2 = "#bf305b";
    public const string Red3 = "#a60032";
    public const string Red4 = "#ff407a";
    public const string Red5 = "#ff739d";
    public const string Orange1 = "#ffa200";
    public const string Orange2 = "#bf8b30";
    public const string Orange3 = "#a66a00";
    public const string Orange4 = "#ffba40";
    public const string Orange5 = "#ffcc73";
    public const string Yellow1 = "#ffeb27";
    public const string Yellow2 = "#bfb446";
    public const string Yellow3 = "#a6980d";
    public const string Yellow4 = "#fff05d";
    public const string Yellow5 = "#fff488";
    public const string Green1 = "#00e432";
    public const string Green2 = "#2bab47";
    public const string Green3 = "#009421";
    public const string Green4 = "#3cf264";
    public const string Green5 = "#6df28a";
    public const string Blue1 = "#29adff";
    public const string Blue2 = "#4790bf";
    public const string Blue3 = "#0d6aa6";
    public const string Blue4 = "#5fc0ff";
    public const string Blue5 = "#89d1ff";
    public const string Indigo1 = "#83769c";
    public const string Indigo2 = "#676075";
    public const string Indigo3 = "#3c2665";
    public const string Indigo4 = "#b5a8ce";
    public const string Indigo5 = "#bbb2ce";
    public const string Pink1 = "#ff77a7";
    public const string Pink2 = "#bf738e";
    public const string Pink3 = "#a62754";
    public const string Pink4 = "#ff99bd";
    public const string Pink5 = "#ffb4cf";
    public const string Peach1 = "#ffccaa";
    public const string Peach2 = "#bfa38f";
    public const string Peach3 = "#a66437";
    public const string Peach4 = "#ffd9bf";
    public const string Peach5 = "#ffe3d0";
    public const string White = "#ffffff";
    public const string Gray1 = "#c2c3c7";
    public const string Gray2 = "#929395";
    public const string Gray3 = "#6e6f70";
    public const string Gray4 = "#535354";
    public const string Gray5 = "#3e3e3f";
    public const string Gray6 = "#2f2f2f";
    public const string Gray7 = "#232323";
    public const string Gray8 = "#1a1a1a";
    public const string Black = "#000000";
  }
}