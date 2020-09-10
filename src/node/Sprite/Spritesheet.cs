using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utils {

  public sealed class FrameData {
    public Rectangle Rect;
    public Size Size;
  }

  public sealed class Spritesheet {
    public Texture2D Texture { get; private set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Dictionary<string, FrameData> Frames { get; private set; }
    public Dictionary<string, List<string>> Animations { get; private set; }


    public Spritesheet() {
    }

    public static Spritesheet FromJson(string path) {
      JObject json = GetJson(path);

      /*
      Texture
      */
      string imagePath = json["meta"]["image"].ToString();
      int resultWidth = (int)json["meta"]["size"]["w"];
      int resultHeight = (int)json["meta"]["size"]["h"];
      Texture2D resultTexture = Engine.LoadTexture(imagePath);

      /*
      Frames
      */
      IList<JToken> frames = json["frames"].Children().ToList();
      Dictionary<string, FrameData> resultFrames = new Dictionary<string, FrameData>();

      foreach (JToken frame in frames) {
        string key = frame["filename"].ToString();
        if (key.EndsWith(".aseprite")) {
          key = key.Replace(".aseprite", "");
        }

        FrameData data = new FrameData {
          Rect = new Rectangle(
            x: (int)frame["frame"]["x"],
            y: (int)frame["frame"]["y"],
            width: (int)frame["frame"]["w"],
            height: (int)frame["frame"]["h"]
          ),
          Size = new Size(
            width: (float)frame["sourceSize"]["w"],
            height: (float)frame["sourceSize"]["h"]
          )
        };

        resultFrames.Add(key, data);
      }

      /*
      Animations

      For each key in our frame, split the key into two parts. The first part will be the same
      for every item in the same animation. 
      */
      Dictionary<string, List<string>> resultAnimations = new Dictionary<string, List<string>>();

      foreach (KeyValuePair<string, FrameData> pair in resultFrames) {
        int splitIdx = pair.Key.LastIndexOf(' ');
        if (splitIdx == -1) {
          continue;
        }
        string animId = pair.Key.Substring(0, splitIdx);
        string animNum = pair.Key.Substring(splitIdx + 1);

        if (!int.TryParse(animNum, out _)) {
          continue;
        }

        if (!resultAnimations.ContainsKey(animId)) {
          resultAnimations.Add(animId, new List<string>());
        }

        resultAnimations[animId].Add(pair.Key);
      }

      return new Spritesheet {
        Texture = resultTexture,
        Frames = resultFrames,
        Animations = resultAnimations,
        Width = resultWidth,
        Height = resultHeight
      };
    }

    public static JObject GetJson(string path) {
      JsonSerializer ser = new JsonSerializer();
      using (StreamReader stream = File.OpenText(Path.Combine(Engine.ContentDirectory, path)))
      using (JsonTextReader reader = new JsonTextReader(stream)) {
        return JObject.Parse(ser.Deserialize(reader).ToString());
      }
    }
  }
}