using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Utils
{

  public static class Json
  {
    //Path.Combine(Engine.ContentDirectory, path)

    public static JObject GetJson(string path)
    {
      JsonSerializer ser = new JsonSerializer();

      try
      {
        using (StreamReader stream = File.OpenText(path))
        using (JsonTextReader reader = new JsonTextReader(stream))
        {
          return JObject.Parse(ser.Deserialize(reader).ToString());
        }
      }
      catch (FileNotFoundException)
      {
        return null;
      }
      catch (DirectoryNotFoundException)
      {
        return null;
      }
    }

    public static void WriteJson(string path, JObject json)
    {
      using (StreamWriter file = File.CreateText(path))
      using (JsonTextWriter writer = new JsonTextWriter(file))
      {
        json.WriteTo(writer);
      }
    }
  }
}