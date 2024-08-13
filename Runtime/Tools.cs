using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Moonloop.Core {
	
	public static class Tools 
	{
		/// <summary>
		/// For things that instantiate in onDisable or onDestroy, this checks if it's safe.
		/// If the editor is destroying things because it's exiting playmode, this will return false.
		/// For builds, always returns true
		/// </summary>
		public static bool SafeToInstantiate()
		{
			#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && UnityEditor.EditorApplication.isPlaying ) {
				return false;
			}
			if (!UnityEditor.EditorApplication.isPlaying) {
				return false;
			}
			#endif
			return true;
		}

		/// <summary>
		/// Set the object as dirty in the editor, so the editor knows to save it. If you do any changes to an object in the editor
		/// via script, you should call this to make sure the changes are saved.
		/// This function is wrapped in a UNITY_EDITOR define so it won't be called in builds.
		/// </summary>
		public static void SetDirty(Object obj)
        {
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(obj);
            #endif
        }
	}
}
