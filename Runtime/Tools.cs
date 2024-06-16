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
	}
}
