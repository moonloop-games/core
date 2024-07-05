using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonloop.Core.Editor
{
    public static class EditorTools
    {

        public static void SetDirty(Object obj)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
            #endif
        }
    }
}
