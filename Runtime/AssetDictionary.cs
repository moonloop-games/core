using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Moonloop.Core {

	/// <summary>
	/// Creates a dictionary where each object is linked to a GUID.
	/// Also includes tools to get things by guids and other nice things.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[System.Serializable]
	public abstract class AssetDictionary<T> : SerializedScriptableObject where T : UnityEngine.Object 
	{
		[ReadOnly, ShowInInspector, SerializeField]
		protected Dictionary<string, T> elements = new Dictionary<string, T>();

		[Button]
		abstract protected void Populate();

		/// <summary>
		/// Populates the dictionary with the given list of assets.
		/// Also checks if the asset is already in the dictionary, so no need to worry about changing guids
		/// </summary>
		/// <param name="sourceAssets"></param>
		public void GenerateDictionary(List<T> sourceAssets)
		{
			// Check which star data are already indexed, so we can skip it.
			List<T> indexedAlready = new List<T>();
			indexedAlready.AddRange(elements.Values);

			foreach (T newElement in sourceAssets)
			{
				if (indexedAlready.Contains(newElement)) 
					continue;

				// add the newly found element to the dictionary
				Guid newGuid = Guid.NewGuid();
				elements.Add(newGuid.ToString(), newElement);
			}
		}

		public List<T> LoadAssetsFromAssetsFolder(string typeName, string assetFolderPath)
		{
			List<T> returnList = new List<T>();

			#if UNITY_EDITOR
			var guids = AssetDatabase.FindAssets("t:" + typeName, new[] {assetFolderPath});
			foreach (var guid in guids)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T)) as T;
				returnList.Add(asset);
			}
			#endif

			return returnList;
		}

		public List<T> Values() 
		{
			List<T> values = new List<T>();
			values.AddRange(elements.Values);
			return values;
		}

		public string GetGuid(T element) 
		{
			if (!element) {
				Debug.Log("A null element was given, can't give a guid.");
				return "";
			}

			foreach (KeyValuePair<string, T> kvp in elements) {
				if (kvp.Value == element) 
					return kvp.Key;
			}

			Debug.Log("No GUID was found for the data " + element.name, element);
			return "";
		}

		public T GetElement(string guid)
		{
			T element = null;
			if (elements.TryGetValue(guid, out element))
				return element;

			Debug.LogError("No element was found for guid " + guid);
			return null;
		}


		[Button]
		protected void PruneDictionary() 
		{
			List<string> keysToPrune = new List<string>();
			foreach (KeyValuePair<string, T> kvp in elements) {
				if (kvp.Value == null) 
					keysToPrune.Add(kvp.Key);
			}

			foreach( var key in keysToPrune) 
				elements.Remove(key);
		}

		[Button]
		public abstract void PrepForBuild();
	}
}