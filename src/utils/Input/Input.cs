using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utils {

  public static partial class Input {

    private static MouseState MouseState;
    private static MouseState LastMouseState;

    public static float MouseX;
    public static float MouseY;

    public enum InputType {
      Keyboard,
      GamePad
    }

    public static KeyboardState KeyboardState;
    public static KeyboardState LastKeyboardState;

    private static Dictionary<PlayerIndex, PortControl> PortControls = new Dictionary<PlayerIndex, PortControl>() {
      {PlayerIndex.One, new PortControl()},
      {PlayerIndex.Two, new PortControl()},
      {PlayerIndex.Three, new PortControl()},
      {PlayerIndex.Four, new PortControl()}
    };

    public static void Init() {
      KeyboardState = Keyboard.GetState();

      foreach (PlayerIndex playerKey in PortControls.Keys) {
        PortControls[playerKey].GamePadState = GamePad.GetState(playerKey);
      }
    }

    public static void Update() {
      LastKeyboardState = KeyboardState;
      KeyboardState = Keyboard.GetState();

      foreach (PlayerIndex playerKey in PortControls.Keys) {
        PortControls[playerKey].LastGamePadState = PortControls[playerKey].GamePadState;
        PortControls[playerKey].GamePadState = GamePad.GetState(playerKey);
      }

      LastMouseState = MouseState;
      MouseState = Mouse.GetState();

      MouseX = MouseState.X;
      MouseY = MouseState.Y;
    }

    public static bool IsGamePadConnected(PlayerIndex pi) {
      PortControl pc = PortControls[pi];
      return pc.GamePadState.IsConnected;
    }

    public static Keys GetRecentKey(PlayerIndex pi) {
      PortControl PC = PortControls[pi];
      if (PC.InputType != InputType.Keyboard) {
        return 0;
      }

      Keys[] k = KeyboardState.GetPressedKeys();
      if (k.Length == 0 || LastKeyboardState.IsKeyDown(k[0])) {
        return 0;
      }
      return k[0];
    }

    public static Buttons GetRecentButton(PlayerIndex pi) {
      PortControl pc = PortControls[pi];
      if (pc.InputType != InputType.GamePad) {
        return 0;
      }

      System.Array inputs = Enum.GetValues(typeof(Buttons));
      foreach (Buttons input in inputs) {
        if (pc.GamePadState.IsButtonDown(input) && !pc.LastGamePadState.IsButtonDown(input)) {
          return input;
        }
      }
      return 0;
    }

    public static InputType GetPortInput(PlayerIndex pi) {
      return PortControls[pi].InputType;
    }

    public static void SetPortInput(InputType inputType, PlayerIndex pi) {
      PortControls[pi].InputType = inputType;
    }

    public static void SetActionMap(Dictionary<string, Keys> keyboardMap, PlayerIndex pi) {
      //PortControls[pi].KeyboardMap = keyboardMap;
      PortControls[pi].SetKeyboardMap(keyboardMap);
    }

    public static void SetActionMap(Dictionary<string, Buttons> gamePadMap, PlayerIndex pi) {
      //PortControls[pi].GamePadMap = gamePadMap;
      PortControls[pi].SetGamePadMap(gamePadMap);
    }

    public static void SetAction(string action, Keys val, PlayerIndex pi) {
      PortControls[pi].SetKeyboardAction(action, val);
    }

    public static void SetAction(string action, Buttons val, PlayerIndex pi) {
      PortControls[pi].SetGamePadAction(action, val);
    }

    public static bool IsActionDown(string action, PlayerIndex pi) {
      return PortControls[pi].IsActionDown(action);
    }

    public static bool IsActionUp(string action, PlayerIndex pi) {
      return PortControls[pi].IsActionUp(action);
    }

    public static bool ActionPressed(string action, PlayerIndex pi) {
      return PortControls[pi].ActionPressed(action);
    }

    public static List<string> GetActiveActions(PlayerIndex pi) {
      PortControl pc = PortControls[pi];
      if (pc.InputType == InputType.GamePad) {
        return new List<string>(pc.GamePadMap.Keys);
      }
      else {
        return new List<string>(pc.KeyboardMap.Keys);
      }
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
  }
}
