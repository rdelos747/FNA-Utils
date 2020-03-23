using System;
using Microsoft.Xna.Framework;

namespace Utils {

  // public enum AnimationType {
  //   CURVE,
  //   STEP
  // }

  public class Animation {

    int ElapsedTime = 0;
    // int maxTime = 0;
    //int numPoints = 0;
    public int Index { get; private set; }
    public bool Loop = true;
    private KeyFrames Frames;
    //public AnimationType animationType;

    //Curve curve = new Curve();

    public Animation(KeyFrames kf, bool loop = true) {
      Loop = loop;
      Frames = kf;
      //animationType = type;
      Index = 0;
    }

    // public void addKeyframe(int time, float value) {
    //   curve.Keys.Add(new CurveKey(time, value));
    //   maxTime = time;
    //   numPoints++;
    // }

    public void reset() {
      ElapsedTime = 0;
      Index = 0;
      //smoothTangents();
    }

    public float Update(GameTime gameTime, ref float value) {
      if (Frames == null) {
        return 0;
      }

      float lastValue = value;
      // value = Frames.Curve.Evaluate(ElapsedTime); // ref value represents actual point on curve

      // if (animationType == AnimationType.STEP) {
      //   value = Frames.Curve.Keys[Index].Value;
      // }
      // if (float.IsNaN(value)) {
      //   value = 0;
      // }
      value = Frames.Evaluate(ElapsedTime, Index);

      UpdateTime(gameTime);
      return value - lastValue; // return delta 
    }

    public float Update(GameTime gameTime) {
      // float value = animationType == AnimationType.STEP ? curve.Keys[Index].Value : curve.Evaluate(elapsedTime);

      // if (float.IsNaN(value)) {
      //   value = 0;
      // }
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
      return Index >= Frames.NumPoints;
    }


  }
}