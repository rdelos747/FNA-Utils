using System;
using Microsoft.Xna.Framework;

namespace Utils {

  public class Animation {
    public int Index { get; private set; }
    public bool Loop = true;
    private int ElapsedTime = 0;
    private KeyFrames Frames;

    public Animation(KeyFrames kf, bool loop = true) {
      Loop = loop;
      Frames = kf;
      Index = 0;
    }

    public void reset() {
      ElapsedTime = 0;
      Index = 0;
    }

    public float Update(GameTime gameTime, ref float value) {
      if (Frames == null) {
        return 0;
      }

      float lastValue = value;
      value = Frames.Evaluate(ElapsedTime, Index);

      UpdateTime(gameTime);
      return value - lastValue; // return delta 
    }

    public float Update(GameTime gameTime) {
      float value = Frames.Evaluate(ElapsedTime, Index);
      UpdateTime(gameTime);
      return value;
    }

    private void UpdateTime(GameTime gameTime) {
      ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
      if (Index < Frames.NumPoints - 1 && ElapsedTime >= Frames.Curve.Keys[Index + 1].Position) {
        Index++;
      }
      if (Loop && ElapsedTime > Frames.MaxTime) {
        ElapsedTime = 0;
        Index = 0;
      }
    }

    public bool IsFinished() {
      if (Loop) {
        return false;
      }
      return Index >= Frames.NumPoints - 1;
    }
  }
}