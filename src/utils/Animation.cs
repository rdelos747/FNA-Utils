using System;
using Microsoft.Xna.Framework;

namespace Engine {

  public enum AnimationType {
    CURVE,
    STEP
  }

  public class Animation {

    int elapsedTime = 0;
    int maxTime = 0;
    int numPoints = 0;
    public int index { get; private set; }
    public bool loop = true;
    public AnimationType animationType;

    Curve curve = new Curve();

    public Animation(bool setLoop = true, AnimationType type = AnimationType.CURVE) {
      loop = setLoop;
      animationType = type;
      index = 0;
    }

    public void addKeyframe(int time, float value) {
      curve.Keys.Add(new CurveKey(time, value));
      maxTime = time;
      numPoints++;
    }

    public void reset() {
      elapsedTime = 0;
      index = 0;
      smoothTangents();
    }

    public float update(GameTime gameTime, ref float value) {
      float lastValue = value;
      value = curve.Evaluate(elapsedTime); // ref value represents actual point on curve

      if (animationType == AnimationType.STEP) {
        value = curve.Keys[index].Value;
      }
      if (float.IsNaN(value)) {
        value = 0;
      }

      updateTime(gameTime);
      return value - lastValue; // return delta 
    }

    public float update(GameTime gameTime) {
      float value = animationType == AnimationType.STEP ? curve.Keys[index].Value : curve.Evaluate(elapsedTime);

      if (float.IsNaN(value)) {
        value = 0;
      }
      updateTime(gameTime);
      return value;
    }

    private void updateTime(GameTime gameTime) {
      elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
      if (index < numPoints - 1 && elapsedTime >= curve.Keys[index + 1].Position) {
        index++;
      }
      if (loop && elapsedTime > maxTime) {
        elapsedTime = 0;
        index = 0;
      }
    }

    private void smoothTangents() {
      CurveKey prev;
      CurveKey current;
      CurveKey next;
      int prevIndex;
      int nextIndex;
      for (int i = 0; i < curve.Keys.Count; i++) {
        prevIndex = i - 1;
        if (prevIndex < 0) prevIndex = i;

        nextIndex = i + 1;
        if (nextIndex == curve.Keys.Count) nextIndex = i;

        prev = curve.Keys[prevIndex];
        next = curve.Keys[nextIndex];
        current = curve.Keys[i];
        SetCurveKeyTangent(ref prev, ref current, ref next);
        curve.Keys[i] = current;
      }
    }

    private void SetCurveKeyTangent(ref CurveKey prev, ref CurveKey cur, ref CurveKey next) {
      float dt = next.Position - prev.Position;
      float dv = next.Value - prev.Value;
      if (Math.Abs(dv) < float.Epsilon) {
        cur.TangentIn = 0;
        cur.TangentOut = 0;
      }
      else {
        // The in and out tangents should be equal to the slope between the adjacent keys.
        cur.TangentIn = dv * (cur.Position - prev.Position) / dt;
        cur.TangentOut = dv * (next.Position - cur.Position) / dt;
      }
    }
  }
}