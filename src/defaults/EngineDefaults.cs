using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Engine {
  public static class EngineDefaults {
    public static readonly Dictionary<string, Keys> inputMap = new Dictionary<string, Keys>() {
      {"primary", Keys.X},
      {"secondary", Keys.Z},
      {"up", Keys.Up},
      {"down", Keys.Down},
      {"left", Keys.Left},
      {"right", Keys.Right}
    };
  }
}