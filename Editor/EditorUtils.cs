using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Moonloop.Core
{
    public static class EditorUtils
    {
        /// <summary>
        /// Returns a list of all the assets of the given type.
        /// </summary>
        /// <param name="includeSubclasses">If true, will include subclasses of the given type. This is slower</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetAllAssetsOfType<T>(bool includeSubclasses = false) where T : ScriptableObject
        {
            List<T> assets = new List<T>();

            // gather a list of all assets in the project
            string[] guids;

            // get the name of the type as a string
            string typeName = typeof(T).Name;
            if (includeSubclasses) typeName = typeof(ScriptableObject).Name;

            // Use asset database to get all the guids of assets of type T
            guids = AssetDatabase.FindAssets("t:" + typeName);
            Debug.Log("found " + guids.Length + " assets of type " + typeName);

            // loop through the guids and load the assets into a list
            foreach (string guid in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var assetObject = AssetDatabase.LoadAssetAtPath(assetPath, typeof(ScriptableObject));

                if (assetObject is T)
                {
                    var typedAsset = assetObject as T;
                    assets.Add(typedAsset);
                }
            }

            return assets;
        }
    }
}