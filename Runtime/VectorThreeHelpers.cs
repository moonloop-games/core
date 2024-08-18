using UnityEngine;

namespace Moonloop.Core {

    public static class VectorThreeHelpers
    {
        public static Vector3 Abs(this Vector3 input) 
        {
            return new Vector3(Mathf.Abs(input.x), Mathf.Abs(input.y), Mathf.Abs(input.z));
        }

        public static Vector3 Sign(this Vector3 input) 
        {
            return new Vector3(
                input.x >= 0 ? 1 : -1,
                input.y >= 0 ? 1 : -1,
                input.z >= 0 ? 1 : -1
            );
        }

        public static bool Approximately(this Vector3 a, Vector3 b) 
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }
    }
}