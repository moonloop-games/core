using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonloop.Core
{
    public class CoroutineHelper : MonoBehaviour
    {
        public static void NewCoroutine(IEnumerator coroutine, bool destroyOnLoad = false)
        {
            // get the name of the coroutine as a string
            string coroutineName = coroutine.ToString();
            GameObject coroutineHelper = new GameObject("Coroutine Helper: " + coroutineName);

            if (!destroyOnLoad)
                DontDestroyOnLoad(coroutineHelper);
                
            var coroutineHelperObject = coroutineHelper.AddComponent<CoroutineHelper>();
            coroutineHelperObject.StartCoroutine(coroutineHelperObject.DoRoutineAndDestroy(coroutine));
        }

        IEnumerator DoRoutineAndDestroy(IEnumerator coroutine)
        {
            yield return coroutine;
            Destroy(gameObject);
        }
    }
}