using System;
using Microsoft.Xna.Framework;

namespace Utils {

  public class Animation {
    public bool Loop = true;
    private int ElapsedTime = 0;
    private KeyFrames Frames;

    public Animation(KeyFrames kf, bool loop = true, int startOffset = 0) {
      Loop = loop;
      Frames = kf;
      ElapsedTime = startOffset;
    }

    public void reset() {
      ElapsedTime = 0;
    }

    public float Update(GameTime gameTime, ref float value) {
      if (Frames == null) {
        return 0;
      }

      float lastValue = value;
      UpdateTime(gameTime);
      value = Frames.Evaluate(ElapsedTime);

      return value - lastValue; // return delta 
    }

    public float Update(GameTime gameTime) {
      UpdateTime(gameTime);
      float value = Frames.Evaluate(ElapsedTime);
      return value;
    }

    private void UpdateTime(GameTime gameTime) {
      ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
      if (Loop && ElapsedTime >= Frames.MaxTime) {
        ElapsedTime = 0;
      }
    }

    public bool IsFinished() {
      if (Loop) {
        return false;
      }
      return ElapsedTime > Frames.MaxTime;
    }
  }
}