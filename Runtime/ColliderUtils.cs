using UnityEngine;

namespace Moonloop.Core {
    public static class ColliderUtils
    {
        /// <summary>
        /// Returns the world-space center of the capsule's start sphere.
        /// </summary>
        public static Vector3 CapsuleStartPoint(CapsuleCollider capsule)
        {
            Vector3 dir = CapsuleDirectionVector(capsule);
            return capsule.transform.TransformPoint(capsule.center - dir * (capsule.height * 0.5f - capsule.radius));
        }

        /// <summary>
        /// Returns the world-space center of the capsule's end sphere.
        /// </summary>
        public static Vector3 CapsuleEndPoint(CapsuleCollider capsule)
        {
            Vector3 dir = CapsuleDirectionVector(capsule);
            return capsule.transform.TransformPoint(capsule.center + dir * (capsule.height * 0.5f - capsule.radius));
        }

        /// <summary>
        /// Returns the object-space direction defined by the capsule collider.
        /// The direction shows up in the editor as a little dropdown for "x-axis", "y-axis", "z-axis".
        /// </summary>
        public static Vector3 CapsuleDirectionVector(CapsuleCollider capsule)
        {
            switch (capsule.direction)
            {
                case 0:
                    return Vector3.right;
                case 1:
                    return Vector3.up;
                case 2:
                    return Vector3.forward;
                default:
                    return Vector3.up;
            }
        }
    }
}