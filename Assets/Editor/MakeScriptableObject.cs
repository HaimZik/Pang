using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject
{
    public static string SavePath = "Assets/Prefabs/scriptableObjectsCollections/";
}

public class MakeScriptableObject<T> where T:ScriptableObject
{
    public static void CreateAsset()
    {
        T asset = ScriptableObject.CreateInstance<T>();
        string[] className = typeof(T).ToString().Split('.');
        string assetName = className[className.Length - 1];
        assetName = MakeScriptableObject.SavePath + assetName + ".asset";
        AssetDatabase.CreateAsset(asset, assetName);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}

