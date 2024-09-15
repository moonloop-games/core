using UnityEngine;

namespace Moonloop {

    public static class GizmoColors 
    {
        public static Color will = Color.white;
        public static Color velocity = new Color(1, .647f, 0, 1);
        public static Color locomotion = Color.green;

        public static Color SetAlpha(this Color input, float alpha) => new Color(input.r, input.g, input.b, alpha);
    }
}