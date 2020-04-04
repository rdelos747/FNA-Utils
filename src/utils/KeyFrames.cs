using System;
using Microsoft.Xna.Framework;


namespace Utils {

  public enum CurveType {
    CURVE,
    STEP
  }

  public sealed class KeyFrames {
    public float MaxTime { get; private set; } = 0;
    public CurveType CurveType;
    public Curve Curve { get; private set; } = new Curve();

    public KeyFrames(CurveType ct) {
      CurveType = ct;
    }

    public KeyFrames(CurveType ct, (int time, int value)[] frames) {
      CurveType = ct;

      for (int i = 0; i < frames.Length; i++) {
        AddKeyframe(frames[i].time, frames[i].value);
      }
    }

    public void AddKeyframe(int time, float value) {
      Curve.Keys.Add(new CurveKey(time, value));
      MaxTime = time;
    }

    public Animation Create(bool loop = true, int startOffset = 0) {
      return new Animation(this, loop, startOffset);
    }

    public void smoothTangents() {
      CurveKey prev;
      CurveKey current;
      CurveKey next;
      int prevIndex;
      int nextIndex;
      for (int i = 0; i < Curve.Keys.Count; i++) {
        prevIndex = i - 1;
        if (prevIndex < 0) prevIndex = i;

        nextIndex = i + 1;
        if (nextIndex == Curve.Keys.Count) nextIndex = i;

        prev = Curve.Keys[prevIndex];
        next = Curve.Keys[nextIndex];
        current = Curve.Keys[i];
        SetCurveKeyTangent(ref prev, ref current, ref next);
        Curve.Keys[i] = current;
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