/*
  Usage: 
  defined actions and reducer methods for manipulating engine settings. 
*/


// // Defines all settings necessary to run the engine
// // Defines all default settings
// using System;
// using Microsoft.Xna.Framework;

// namespace Utils {
//   public static class SettingTypes {
//     public const string MENU_OPEN = "MENU_OPEN";
//     public const string MENU_CLOSE = "MENU_CLOSE";
//     public const string MENU_CLICK = "MENU_CLICK";
//   }

//   public sealed class EngineSettings : Settings {
//     private Renderer renderer;

//     public EngineSettings(Renderer renderer) {
//       this.renderer = renderer;
//     }

//     protected override void reducer(string action, object payload) {
//       switch (action) {
//         case SettingTypes.MENU_CLICK:
//           Console.WriteLine("in update thing");
//           break;
//         case SettingTypes.MENU_OPEN:
//           Console.WriteLine("in opem");
//           renderer.IsMouseVisible = true;
//           break;
//         case SettingTypes.MENU_CLOSE:
//           Console.WriteLine("in close");
//           renderer.IsMouseVisible = false;
//           break;
//         default:
//           Console.WriteLine("in default");
//           break;
//       }
//     }
//   }
// }