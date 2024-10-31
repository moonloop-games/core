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

        ///<summary> 
        ///scales the animation curve so that it always goes from 0 to 1 time and 0 to 1 intensity 
        ///</summary>
        public static void NormalizeCurve(this AnimationCurve curve) 
        {
            float maxTime = 0;
            float maxIntensity = 0;
            for (int i = 0; i < curve.length; i++)
            {
                maxTime = Mathf.Max(maxTime, curve.keys[i].time);
                maxIntensity = Mathf.Max(maxIntensity, curve.keys[i].value);
            }

            for (int i = 0; i < curve.length; i++)
            {
                Keyframe key = curve.keys[i];
                key.time /= maxTime;
                key.value /= maxIntensity;
                curve.MoveKey(i, key);
            }
        }

        /// <summary>
        /// Scales the curve so that the time (x-axis) goes from 0 to 1
        /// </summary>
        public static void NormalizeCurveHorizontal( this AnimationCurve curve )
        {
            float maxTime = 0;
            for (int i = 0; i < curve.length; i++)
                maxTime = Mathf.Max(maxTime, curve.keys[i].time);

            for (int i = 0; i < curve.length; i++)
            {
                Keyframe key = curve.keys[i];
                key.time /= maxTime;
                curve.MoveKey(i, key);
            }
        }

        /// <summary>
        /// Scales the curve so that the intensity (y-axis) goes from 0 to 1
        public static void NormalizeCurveVertical( this AnimationCurve curve )
        {
            float maxIntensity = 0;
            for (int i = 0; i < curve.length; i++)
                maxIntensity = Mathf.Max(maxIntensity, curve.keys[i].value);

            for (int i = 0; i < curve.length; i++)
            {
                Keyframe key = curve.keys[i];
                key.value /= maxIntensity;
                curve.MoveKey(i, key);
            }
        }
        
        /// <summary>
        /// Sets all the points on the curve to have a constant tangent mode. This makes the curve look like stairs
        /// </summary>
        public static void MakeCurveConstant( this AnimationCurve curve )
        {
            #if UNITY_EDITOR
            for (int i = 0; i < curve.length; i++)
            {
                Keyframe key = curve.keys[i];
                UnityEditor.AnimationUtility.SetKeyLeftTangentMode(curve, i, UnityEditor.AnimationUtility.TangentMode.Constant);
                UnityEditor.AnimationUtility.SetKeyRightTangentMode(curve, i, UnityEditor.AnimationUtility.TangentMode.Constant);
            }
            #endif
        }
    }
}