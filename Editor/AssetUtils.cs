using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Phil.Edit {

public static class AssetUtils {

	public static string CreateUniqueFolder(string parent, string folderName){
		string guid = AssetDatabase.CreateFolder (parent, folderName);
		return AssetDatabase.GUIDToAssetPath (guid);
	}

	public static string GetDirectory(Object obj){
		return Path.GetDirectoryName (AssetDatabase.GetAssetPath (obj));
	}

	public static string[] GetSubDirectories(string directory){
		string[] subDirs = Directory.GetDirectories (directory);
		if (subDirs == null) {
			return null;
		}
		for (int i = 0; i < subDirs.Length; i++) {
			subDirs[i] = subDirs[i].Replace('\\','/');
		}
		return subDirs;
	}

	public static string[] GetSubDirectories(Object obj){
		string[] subDirs = Directory.GetDirectories (GetDirectory(obj));
		if (subDirs == null) {
			return null;
		}
		for (int i = 0; i < subDirs.Length; i++) {
			subDirs[i] = subDirs[i].Replace('\\','/');
		}
		return subDirs;
	}

	public static T GetNeighborAsset<T> ( Object centralAsset) where T:Object {
		List<T> neighbors = new List<T> (1);
		GetNeighborAssets<T> (centralAsset, ref neighbors);
		if (neighbors.Count > 0) {
			return neighbors [0];
		} else {
			return null;
		}
	}

	public static void GetNeighborAssets<T> ( Object centralAsset, ref List<T> neighboringAssets) where T:Object {

		neighboringAssets.Clear ();
		string dirCentralAsset = GetDirectory (centralAsset);

		GetAssets<T> (ref neighboringAssets, dirCentralAsset);
		// Filter ones in lower directories...
		string dirNeighborAsset;
		for (int i = neighboringAssets.Count-1; i >= 0; i--) {
			dirNeighborAsset = GetDirectory (neighboringAssets [i]);
			if (dirNeighborAsset != dirCentralAsset) {
				neighboringAssets.RemoveAt (i);
			}
		}

	}

	public static T GetFirstAsset<T>(string directory="Assets") where T:Object {
		List<T> list = new List<T> ();
		GetAssets<T> (ref list, directory);
		if (list.Count > 0) {
			return list [0];
		} else {
			return null;
		}
	}

	public static void GetAssets<T> (ref List<T> list, string directory="Assets") where T:Object {

		string[] dirList = new string[1]; dirList [0] = directory;
		System.Type t = typeof(T);
		string typeName = t.Name;

		// Look for them.
		string[] GUIDs = AssetDatabase.FindAssets("t:" + typeName, dirList);
		for (int i = 0; i < GUIDs.Length; i++) {
			string assetPath = AssetDatabase.GUIDToAssetPath (GUIDs [i]);
			T asset = AssetDatabase.LoadAssetAtPath<T> (assetPath);
			list.Add (asset);
		}

	}

	public static T GetNeighborAssetWithScript<T> ( Object centralAsset ) where T:Component {
		List<T> neighbors = new List<T> (1);
		GetNeighborAssetsWithScript<T> (centralAsset, ref neighbors);
		if (neighbors.Count > 0)
			return neighbors [0];
		else
			return null;
	}

	public static void GetNeighborAssetsWithScript<T> (Object centralAsset, ref List<T> neighboringAssets) 
		where T:Component 
	{
		neighboringAssets.Clear ();
		string dirCentralAsset = GetDirectory (centralAsset);

		GetAssetsWithScript<T> (ref neighboringAssets, dirCentralAsset);
		// Filter-out.
		string dirNeighborAsset;
		for (int i = neighboringAssets.Count - 1; i >= 0; i--) {
			dirNeighborAsset = GetDirectory (neighboringAssets [i]);
			if (dirNeighborAsset != dirCentralAsset) {
				neighboringAssets.RemoveAt (i);
			}
		}
			
	}

	public static void GetAssetsWithScript<T> (ref List<T> list, string directory="Assets") where T:Component {
		string[] dirList = new string[1]; dirList [0] = directory;

		// Look for them.
		string[] GUIDs = AssetDatabase.FindAssets("t:Prefab", dirList);

		for (int i = 0; i < GUIDs.Length; i++) {
			string assetPath = AssetDatabase.GUIDToAssetPath (GUIDs [i]);
			GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject> (assetPath);
			T component = gameObject.GetComponent<T> ();
			if (component != null) {
				// We don't wanna add crap the list.
				list.Add (component);
			}
		}
	}

	public static T MakeScriptableObjectHere<T>(string directory, string defaultName) where T:ScriptableObject {

		string so_path = AssetDatabase.GenerateUniqueAssetPath (directory + "/" + defaultName + ".asset");
		T so = ScriptableObject.CreateInstance<T> ();
		AssetDatabase.CreateAsset(so, so_path);

		return so;
	}

	public static T CloneScriptableObjectHere<T>(T src, string directory) where T:ScriptableObject {
		string so_path = AssetDatabase.GenerateUniqueAssetPath (directory + "/" + src.name + ".asset");
		T so = ScriptableObject.CreateInstance<T> ();
		AssetDatabase.CreateAsset(so, so_path);

		EditorUtility.SetDirty (so);
		EditorUtility.CopySerializedIfDifferent (src, so);

		return so;
	}

}

}
