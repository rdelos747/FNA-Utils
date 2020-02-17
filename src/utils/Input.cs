using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Engine {
  public static class Input {
    private static KeyboardState KeyboardState = Keyboard.GetState();
    private static KeyboardState LastKeyboardState;

    private static MouseState MouseState;
    private static MouseState LastMouseState;

    public static Dictionary<string, Keys> InputMap { get; private set; } = new Dictionary<string, Keys>();

    public static float MouseX;
    public static float MouseY;

    public static void update() {
      LastKeyboardState = KeyboardState;
      KeyboardState = Keyboard.GetState();

      LastMouseState = MouseState;
      MouseState = Mouse.GetState();

      MouseX = MouseState.X;
      MouseY = MouseState.Y;
    }

    public static bool IsKeyDown(Keys input) {
      return KeyboardState.IsKeyDown(input);
    }
    public static bool IsKeyDown(string mapKey) {
      return HasKey(mapKey) && IsKeyDown(InputMap[mapKey]);
    }

    public static bool IsKeyUp(Keys input) {
      return KeyboardState.IsKeyUp(input);
    }
    public static bool IsKeyUp(string mapKey) {
      return HasKey(mapKey) && IsKeyUp(InputMap[mapKey]);
    }

    public static bool KeyPressed(Keys input) {
      if (KeyboardState.IsKeyDown(input) == true && LastKeyboardState.IsKeyDown(input) == false)
        return true;
      else
        return false;
    }
    public static bool KeyPressed(string mapKey) {
      return HasKey(mapKey) && KeyPressed(InputMap[mapKey]);
    }

    public static bool MouseLeftDown() {
      if (MouseState.LeftButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool MouseRightDown() {
      if (MouseState.RightButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool MouseLeftClicked() {
      if (MouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released)
        return true;
      else
        return false;
    }

    public static bool MouseRightClicked() {
      if (MouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released)
        return true;
      else
        return false;
    }

    public static void SetInputMapKey(string key, Keys val) {
      InputMap[key] = val;
    }

    public static void SetInputMap(Dictionary<string, Keys> newMap) {
      InputMap = newMap;
    }

    public static bool HasKey(string key) {
      return InputMap.ContainsKey(key);
    }

    public static string KeyAsText(string key) {
      if (!HasKey(key)) {
        return "no key found";
      }

      switch (InputMap[key]) {
        case (Keys.None):
          return "None";
        case (Keys.Back):
          return "Back";
        case (Keys.Tab):
          return "Tab";
        case (Keys.Enter):
          return "Enter";
        case (Keys.CapsLock):
          return "CapsLocl";
        case (Keys.Escape):
          return "Esc";
        case (Keys.Space):
          return "Space";
        case (Keys.PageUp):
          return "Page Up";
        case (Keys.PageDown):
          return "Page Down";
        case (Keys.End):
          return "End";
        case (Keys.Home):
          return "Home";
        case (Keys.Left):
          return "Left";
        case (Keys.Up):
          return "Up";
        case (Keys.Right):
          return "Right";
        case (Keys.Down):
          return "Down";
        case (Keys.Select):
          return "Select";
        case (Keys.Print):
          return "Print";
        case (Keys.Execute):
          return "Execute";
        case (Keys.PrintScreen):
          return "Print Screen";
        case (Keys.Insert):
          return "Insert";
        case (Keys.Delete):
          return "Delete";
        case (Keys.Help):
          return "Help";
        case (Keys.D0):
          return "0";
        case (Keys.D1):
          return "1";
        case (Keys.D2):
          return "2";
        case (Keys.D3):
          return "3";
        case (Keys.D4):
          return "4";
        case (Keys.D5):
          return "5";
        case (Keys.D6):
          return "6";
        case (Keys.D7):
          return "7";
        case (Keys.D8):
          return "8";
        case (Keys.D9):
          return "9";
        case (Keys.A):
          return "A";
        case (Keys.B):
          return "B";
        case (Keys.C):
          return "C";
        case (Keys.D):
          return "D";
        case (Keys.E):
          return "E";
        case (Keys.F):
          return "F";
        case (Keys.G):
          return "G";
        case (Keys.H):
          return "H";
        case (Keys.I):
          return "I";
        case (Keys.J):
          return "J";
        case (Keys.K):
          return "K";
        case (Keys.L):
          return "L";
        case (Keys.M):
          return "M";
        case (Keys.N):
          return "N";
        case (Keys.O):
          return "O";
        case (Keys.P):
          return "P";
        case (Keys.Q):
          return "Q";
        case (Keys.R):
          return "R";
        case (Keys.S):
          return "S";
        case (Keys.T):
          return "T";
        case (Keys.U):
          return "U";
        case (Keys.V):
          return "V";
        case (Keys.W):
          return "W";
        case (Keys.X):
          return "X";
        case (Keys.Y):
          return "Y";
        case (Keys.Z):
          return "Z";
        case (Keys.LeftWindows):
          return "Left Windows";
        case (Keys.RightWindows):
          return "Right Windows";
        case (Keys.Apps):
          return "Apps";
        case (Keys.Sleep):
          return "Sleep";
        case (Keys.NumPad0):
          return "Num Pad 0";
        case (Keys.NumPad1):
          return "Num Pad 1";
        case (Keys.NumPad2):
          return "Num Pad 2";
        case (Keys.NumPad3):
          return "Num Pad 3";
        case (Keys.NumPad4):
          return "Num Pad 4";
        case (Keys.NumPad5):
          return "Num Pad 5";
        case (Keys.NumPad6):
          return "Num Pad 6";
        case (Keys.NumPad7):
          return "Num Pad 7";
        case (Keys.NumPad8):
          return "Num Pad 8";
        case (Keys.NumPad9):
          return "Num Pad 9";
        case (Keys.Multiply):
          return "Multiply";
        case (Keys.Add):
          return "Add";
        case (Keys.Separator):
          return "Separator";
        case (Keys.Subtract):
          return "Subtract";
        case (Keys.Decimal):
          return "Decimal";
        case (Keys.Divide):
          return "Divide";
        case (Keys.F1):
          return "F1";
        case (Keys.F2):
          return "F2";
        case (Keys.F3):
          return "F3";
        case (Keys.F4):
          return "F4";
        case (Keys.F5):
          return "F5";
        case (Keys.F6):
          return "F6";
        case (Keys.F7):
          return "F7";
        case (Keys.F8):
          return "F8";
        case (Keys.F9):
          return "F9";
        case (Keys.F10):
          return "F10";
        case (Keys.F11):
          return "F11";
        case (Keys.F12):
          return "F12";
        case (Keys.F13):
          return "F13";
        case (Keys.F14):
          return "F14";
        case (Keys.F15):
          return "F15";
        case (Keys.F16):
          return "F16";
        case (Keys.F17):
          return "F17";
        case (Keys.F18):
          return "F18";
        case (Keys.F19):
          return "F19";
        case (Keys.F20):
          return "F20";
        case (Keys.F21):
          return "F21";
        case (Keys.F22):
          return "F22";
        case (Keys.F23):
          return "F23";
        case (Keys.F24):
          return "F24";
        case (Keys.NumLock):
          return "Num Lock";
        case (Keys.Scroll):
          return "Scroll";
        case (Keys.LeftShift):
          return "Left Shift";
        case (Keys.RightShift):
          return "Right Shift";
        case (Keys.LeftControl):
          return "Left Control";
        case (Keys.RightControl):
          return "Right Control";
        case (Keys.LeftAlt):
          return "Left Alt";
        case (Keys.RightAlt):
          return "Right Alt";
        case (Keys.BrowserBack):
          return "";
        case (Keys.BrowserForward):
          return "";
        case (Keys.BrowserRefresh):
          return "";
        case (Keys.BrowserStop):
          return "";
        case (Keys.BrowserSearch):
          return "";
        case (Keys.BrowserFavorites):
          return "";
        case (Keys.BrowserHome):
          return "";
        case (Keys.VolumeMute):
          return "";
        case (Keys.VolumeDown):
          return "";
        case (Keys.VolumeUp):
          return "";
        case (Keys.MediaNextTrack):
          return "";
        case (Keys.MediaPreviousTrack):
          return "";
        case (Keys.MediaStop):
          return "";
        case (Keys.MediaPlayPause):
          return "";
        case (Keys.LaunchMail):
          return "";
        case (Keys.SelectMedia):
          return "";
        case (Keys.LaunchApplication1):
          return "";
        case (Keys.LaunchApplication2):
          return "";
        case (Keys.OemSemicolon):
          return "";
        case (Keys.OemPlus):
          return "";
        case (Keys.OemComma):
          return "";
        case (Keys.OemMinus):
          return "";
        case (Keys.OemPeriod):
          return "";
        case (Keys.OemQuestion):
          return "";
        case (Keys.OemTilde):
          return "";
        case (Keys.OemOpenBrackets):
          return "";
        case (Keys.OemPipe):
          return "";
        case (Keys.OemCloseBrackets):
          return "";
        case (Keys.OemQuotes):
          return "";
        case (Keys.Oem8):
          return "";
        case (Keys.OemBackslash):
          return "";
        case (Keys.ProcessKey):
          return "";
        case (Keys.Attn):
          return "";
        case (Keys.Crsel):
          return "";
        case (Keys.Exsel):
          return "";
        case (Keys.EraseEof):
          return "";
        case (Keys.Play):
          return "";
        case (Keys.Zoom):
          return "";
        case (Keys.Pa1):
          return "";
        case (Keys.OemClear):
          return "";
        case (Keys.ChatPadGreen):
          return "";
        case (Keys.ChatPadOrange):
          return "";
        case (Keys.Pause):
          return "";
        case (Keys.ImeConvert):
          return "";
        case (Keys.ImeNoConvert):
          return "";
        case (Keys.Kana):
          return "";
        case (Keys.Kanji):
          return "";
        case (Keys.OemAuto):
          return "";
        case (Keys.OemCopy):
          return "";
        case (Keys.OemEnlW):
          return "";
        default:
          return "no key found";
      }
    }

    // public static Keys getKey(string key) {
    //   //return inputMap.ContainsKey(key);
    //   return hasKey
    // }

    // /// <summary>
    // /// Gets mouse coordinates adjusted for virtual resolution and camera position.
    // /// </summary>
    // public static Vector2 MousePositionCamera() {
    //   Vector2 mousePosition = Vector2.Zero;
    //   mousePosition.X = mouseState.X;
    //   mousePosition.Y = mouseState.Y;

    //   return ScreenToWorld(mousePosition);
    // }

    // /// <summary>
    // /// Gets the last mouse coordinates adjusted for virtual resolution and camera position.
    // /// </summary>
    // public static Vector2 LastMousePositionCamera() {
    //   Vector2 mousePosition = Vector2.Zero;
    //   mousePosition.X = lastMouseState.X;
    //   mousePosition.Y = lastMouseState.Y;

    //   return ScreenToWorld(mousePosition);
    // }

    // /// <summary>
    // /// Takes screen coordinates (2D position like where the mouse is on screen) then converts it to world position (where we clicked at in the world). 
    // /// </summary>
    // private static Vector2 ScreenToWorld(Vector2 input) {
    //   input.X -= Resolution.VirtualViewportX;
    //   input.Y -= Resolution.VirtualViewportY;

    //   return Vector2.Transform(input, Matrix.Invert(Camera.GetTransformMatrix()));
    // }
  }
}
