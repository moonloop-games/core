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

		/// <summary>
		/// Loads the assets from the assets folder and returns a list of them
		/// </summary>
		/// <param name="assetFolderPath">The folder path to load assets from. Example: "Assets/Data/"</param>
		/// <param name="includeSubClasses">If true, will do a slower search to include all subclasses of the class.</param>
		public List<T> LoadAssets(string assetFolderPath, bool includeSubClasses)
		{
			List<T> returnList = new List<T>();

#if UNITY_EDITOR
			string typeName = includeSubClasses ? typeof(UnityEngine.ScriptableObject).Name : typeof(T).Name;
			var guids = AssetDatabase.FindAssets("t:" + typeName, new[] { assetFolderPath });
			foreach (var guid in guids)
			{
				var assetPath = AssetDatabase.GUIDToAssetPath(guid);
				var asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
				if (asset != null && asset is T typedAsset)
					returnList.Add(typedAsset);
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

		/// <summary>
		/// Checks if any element with the given GUID is in the library, and if so returns it.
		/// </summary>
		/// <param name="guid">the guid of the element youre looking for.</param>
		/// <returns>The element matching the given guid</returns>
		public T GetElement(string guid)
		{
			T element = null;
			if (elements.TryGetValue(guid, out element))
				return element;

			Debug.LogError("No element was found for guid " + guid);
			return null;
		}

		/// <summary>
		/// Returns a list of elements that match the given guids.
		/// </summary>
		/// <param name="guids">List of guids of the elements you want</param>
		/// <returns></returns>
		public List<T> GetElements(List<string> guids)
		{
			List<T> elements = new List<T>();
			foreach (string guid in guids)
			{
				T element = GetElement(guid);
				if (element != null)
					elements.Add(element);
			}

			return elements;
		}

		/// <summary>
		/// Returns a list of guids of the given elements.
		/// </summary>
		/// <param name="elements"></param>
		/// <returns></returns>
		public List<T> GetGuids(List<T> elements)
		{
			List<T> guids = new List<T>();
			foreach (T element in elements)
			{
				string guid = GetGuid(element);
				if (!string.IsNullOrEmpty(guid))
					guids.Add(element);
			}

			return guids;
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