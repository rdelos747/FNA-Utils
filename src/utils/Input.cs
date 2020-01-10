using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Engine {
  public static class Input {
    private static KeyboardState keyboardState = Keyboard.GetState();
    private static KeyboardState lastKeyboardState;

    private static MouseState mouseState;
    private static MouseState lastMouseState;

    private static Dictionary<string, Keys> inputMap = new Dictionary<string, Keys>();

    public static float mouseX;
    public static float mouseY;

    public static void update() {
      lastKeyboardState = keyboardState;
      keyboardState = Keyboard.GetState();

      lastMouseState = mouseState;
      mouseState = Mouse.GetState();

      mouseX = mouseState.X;
      mouseY = mouseState.Y;
    }

    public static bool isKeyDown(Keys input) {
      return keyboardState.IsKeyDown(input);
    }
    public static bool isKeyDown(string mapKey) {
      return hasKey(mapKey) && isKeyDown(inputMap[mapKey]);
    }

    public static bool isKeyUp(Keys input) {
      return keyboardState.IsKeyUp(input);
    }
    public static bool isKeyUp(string mapKey) {
      return hasKey(mapKey) && isKeyUp(inputMap[mapKey]);
    }

    public static bool keyPressed(Keys input) {
      if (keyboardState.IsKeyDown(input) == true && lastKeyboardState.IsKeyDown(input) == false)
        return true;
      else
        return false;
    }
    public static bool keyPressed(string mapKey) {
      return hasKey(mapKey) && keyPressed(inputMap[mapKey]);
    }

    public static bool mouseLeftDown() {
      if (mouseState.LeftButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool mouseRightDown() {
      if (mouseState.RightButton == ButtonState.Pressed)
        return true;
      else
        return false;
    }

    public static bool mouseLeftClicked() {
      if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
        return true;
      else
        return false;
    }

    public static bool msouseRightClicked() {
      if (mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
        return true;
      else
        return false;
    }

    public static void setInputMapKey(string key, Keys val) {
      inputMap[key] = val;
    }

    public static void setInputMap(Dictionary<string, Keys> newMap) {
      inputMap = newMap;
    }

    public static bool hasKey(string key) {
      return inputMap.ContainsKey(key);
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
