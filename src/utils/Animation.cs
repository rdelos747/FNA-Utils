using System;
using Microsoft.Xna.Framework;

namespace Utils {

  public class Animation {
    public bool Loop = true;
    private float ElapsedTime = 0;
    private KeyFrames Frames;
    public int Index { get; private set; }

    public Animation(KeyFrames kf, bool loop = true, int startOffset = 0) {
      Loop = loop;
      Frames = kf;
      ElapsedTime = startOffset;

      if (startOffset > 0 && Frames.Curve.Keys.Count > 0) {
        int index = 0;

        while (Frames.Curve.Keys[index].Position < startOffset) {
          index++;
        }
        Index = index;
      }
    }

    public void Reset() {
      ElapsedTime = 0;
      Index = 0;
    }

    public float Update(GameTime gameTime, ref float value) {
      if (Frames == null) {
        return 0;
      }

      float lastValue = value;
      value = Evaluate(gameTime);

      return value - lastValue; // return delta 
    }

    public float Update(GameTime gameTime) {
      return Evaluate(gameTime);
    }

    private float Evaluate(GameTime gameTime) {
      ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
      if (Index < Frames.Curve.Keys.Count - 1 && ElapsedTime >= Frames.Curve.Keys[Index + 1].Position) {
        Index++;
      }
      if (Loop && ElapsedTime > Frames.MaxTime) {
        ElapsedTime = 0;
        Index = 0;
      }

      float value = 0;

      if (Frames.CurveType == CurveType.STEP && Index < Frames.Curve.Keys.Count) {
        value = Frames.Curve.Keys[Index].Value;
      }
      else {
        value = Frames.Curve.Evaluate(ElapsedTime);
      }

      if (float.IsNaN(value)) {
        value = 0;
      }

      return value;
    }

    public bool IsFinished() {
      if (Loop) {
        return false;
      }
      return ElapsedTime > Frames.MaxTime;
    }
  }
}