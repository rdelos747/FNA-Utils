using System;
using Microsoft.Xna.Framework;

namespace Engine {

  public class Animation {

    int elapsedTime = 0;

    Curve curve = new Curve();

    public Animation(float initialValue = 0) {
      curve.Keys.Add(new CurveKey(0, initialValue));
    }

    public void addKeyframe(int time, float value) {
      curve.Keys.Add(new CurveKey(time, value));
    }

    public void reset() {
      elapsedTime = 0;
      smoothTangents();
    }

    public float update(GameTime gameTime, ref float value) {
      float lastValue = value;
      value = curve.Evaluate(elapsedTime); // ref value represents actual point on curve
      if (float.IsNaN(value)) {
        value = 0;
      }
      elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
      return value - lastValue; // return delta 
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