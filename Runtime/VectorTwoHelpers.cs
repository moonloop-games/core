using UnityEngine;

namespace Moonloop.Core {

    public static class VectorTwoHelpers
    {
        public static Vector2 Abs(this Vector2 input) 
        {
            return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
        }

        public static Vector2 Sign(this Vector2 input) 
        {
            return new Vector2(
                input.x >= 0 ? 1 : -1,
                input.y >= 0 ? 1 : -1
            );
        }

        public static bool Approximately(this Vector2 a, Vector2 b) 
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y);
        }
    }
}