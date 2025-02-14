using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonloop.Core
{
    public class CoroutineHelper : MonoBehaviour
    {
        static CoroutineHelper instance;
        static CoroutineHelper persistentInstance;

        public static Coroutine NewCoroutine(IEnumerator coroutine, bool dontDestroyOnLoad = false)
        {
            var helperInstance = EnsureInstance(dontDestroyOnLoad);
            return helperInstance.StartCoroutine(coroutine);
        }

        static CoroutineHelper EnsureInstance(bool dontDestroyOnLoad)
        {
            if (dontDestroyOnLoad)
            {
                if (persistentInstance) return persistentInstance;
                GameObject go = new GameObject("Coroutine Helper - persistent");
                persistentInstance = go.AddComponent<CoroutineHelper>();
                DontDestroyOnLoad(go);
                return persistentInstance;
            }
            else
            {
                if (instance) return instance;
                GameObject go = new GameObject("Coroutine Helper");
                instance = go.AddComponent<CoroutineHelper>();
                return instance;
            }
        }
    }
}