using UnityEngine;

namespace Moonloop.Core {
    public static class CurveUtils
    {
        public static float Duration(this AnimationCurve curve)
        {
            return curve.keys[curve.length - 1].time;
        }

        public static float Height(this AnimationCurve curve)
        {
            // get the highest and lowest point in the curve
            float highest = Mathf.NegativeInfinity;
            float lowest = Mathf.Infinity;

            foreach (var key in curve.keys)
            {
                if (key.value > highest) highest = key.value;
                if (key.value < lowest) lowest = key.value;
            }

            return highest - lowest;
        }
    }
}