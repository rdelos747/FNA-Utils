using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Utils
{

  public static partial class Input
  {
    public static string NoneCode = "None";

    private static MouseState MouseState;
    private static MouseState LastMouseState;

    public static Vector2 MousePos = Vector2.Zero;

    public enum InputType
    {
      Keyboard,
      GamePad
    }

    public static KeyboardState KeyboardState;
    public static KeyboardState LastKeyboardState;

    public static Action OnGamePadConnectionChanged = () => { };

    public static Dictionary<PlayerIndex, PortControl> PortControls = new Dictionary<PlayerIndex, PortControl>() {
      {PlayerIndex.One, new PortControl()},
      {PlayerIndex.Two, new PortControl()},
      {PlayerIndex.Three, new PortControl()},
      {PlayerIndex.Four, new PortControl()}
    };

    public static void Init()
    {
      KeyboardState = Keyboard.GetState();

      foreach (PlayerIndex playerKey in PortControls.Keys)
      {
        PortControls[playerKey].GamePadState = GamePad.GetState(playerKey);
      }
    }

    public static void Update()
    {
      LastKeyboardState = KeyboardState;
      KeyboardState = Keyboard.GetState();

      foreach (PlayerIndex playerKey in PortControls.Keys)
      {
        PortControls[playerKey].LastGamePadState = PortControls[playerKey].GamePadState;
        bool lastGamepadConnected = PortControls[playerKey].GamePadState.IsConnected;
        PortControls[playerKey].GamePadState = GamePad.GetState(playerKey);

        if (PortControls[playerKey].GamePadState.IsConnected != lastGamepadConnected)
        {
          OnGamePadConnectionChanged();
        }
      }

      LastMouseState = MouseState;
      MouseState = Mouse.GetState();

      MousePos = new Vector2(MouseState.X, MouseState.Y);
    }

    public static bool IsGamePadConnected(PlayerIndex pi = PlayerIndex.One)
    {
      PortControl pc = PortControls[pi];
      return pc.GamePadState.IsConnected;
    }

    public static string[] DecodeAction(string action, PlayerIndex pi = PlayerIndex.One)
    {
      PortControl pc = PortControls[pi];
      return pc.DecodeAction(action);
    }

    public static Keys GetRecentKey(PlayerIndex pi = PlayerIndex.One)
    {
      PortControl PC = PortControls[pi];
      if (PC.InputType != InputType.Keyboard)
      {
        return 0;
      }

      Keys[] k = KeyboardState.GetPressedKeys();
      if (k.Length == 0 || LastKeyboardState.IsKeyDown(k[0]))
      {
        return 0;
      }
      return k[0];
    }

    public static Buttons GetRecentButton(PlayerIndex pi = PlayerIndex.One)
    {
      PortControl pc = PortControls[pi];
      if (pc.InputType != InputType.GamePad)
      {
        return 0;
      }

      System.Array inputs = Enum.GetValues(typeof(Buttons));
      foreach (Buttons input in inputs)
      {
        if (pc.GamePadState.IsButtonDown(input) && !pc.LastGamePadState.IsButtonDown(input))
        {
          return input;
        }
      }
      return 0;
    }

    public static InputType GetPortInput(PlayerIndex pi = PlayerIndex.One)
    {
      return PortControls[pi].InputType;
    }

    public static Keys[] GetKeysForAction(string action, PlayerIndex pi = PlayerIndex.One)
    {
      return PortControls[pi].KeyboardMap[action];
    }

    public static Buttons[] GetButtonsForAction(string action, PlayerIndex pi = PlayerIndex.One)
    {
      return PortControls[pi].GamePadMap[action];
    }

    public static void SetPortInput(InputType inputType, PlayerIndex pi = PlayerIndex.One)
    {
      PortControls[pi].InputType = inputType;
    }

    public static void SetActionMap(Dictionary<string, Keys[]> keyboardMap, PlayerIndex pi = PlayerIndex.One)
    {
      PortControls[pi].SetKeyboardMap(keyboardMap);
    }

    public static void SetActionMap(Dictionary<string, Buttons[]> gamePadMap, PlayerIndex pi = PlayerIndex.One)
    {
      PortControls[pi].SetGamePadMap(gamePadMap);
    }

    public static void SetAction(string action, Keys[] vals, PlayerIndex pi = PlayerIndex.One)
    {
      PortControls[pi].SetKeyboardAction(action, vals);
    }

    public static void SetAction(string action, Buttons[] vals, PlayerIndex pi = PlayerIndex.One)
    {
      PortControls[pi].SetGamePadAction(action, vals);
    }

    public static bool ActionDown(string action, PlayerIndex pi = PlayerIndex.One)
    {
      return PortControls[pi].ActionDown(action);
    }

    // public static bool ActionUp(string action, PlayerIndex pi = PlayerIndex.One)
    // {
    //   return PortControls[pi].ActionUp(action);
    // }

    public static bool ActionPressed(string action, PlayerIndex pi = PlayerIndex.One)
    {
      return PortControls[pi].ActionPressed(action);
    }

    public static List<string> GetActiveActions(PlayerIndex pi = PlayerIndex.One)
    {
      PortControl pc = PortControls[pi];
      if (pc.InputType == InputType.GamePad)
      {
        return new List<string>(pc.GamePadMap.Keys);
      }
      else
      {
        return new List<string>(pc.KeyboardMap.Keys);
      }
    }

    public static bool MouseLeftDown()
    {
      if (MouseState.LeftButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool MouseRightDown()
    {
      if (MouseState.RightButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool MouseMiddleDown()
    {
      if (MouseState.MiddleButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool MouseLeftClicked()
    {
      if (MouseState.LeftButton == ButtonState.Pressed && LastMouseState.LeftButton == ButtonState.Released)
        return true;
      else
        return false;
    }

    public static bool MouseRightClicked()
    {
      if (MouseState.RightButton == ButtonState.Pressed && LastMouseState.RightButton == ButtonState.Released)
        return true;
      else
        return false;
    }

    public static bool MouseMiddleClicked()
    {
      if (MouseState.MiddleButton == ButtonState.Pressed && LastMouseState.MiddleButton == ButtonState.Released)
        return true;
      else
        return false;
    }


    public static int MouseScrolled()
    {
      return MouseState.ScrollWheelValue - LastMouseState.ScrollWheelValue;
    }
  }
}
