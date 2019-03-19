using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonTile : Tile
{

public bool pressed;
    
#if UNITY_EDITOR
// The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/ButtonTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save ButtonTile", "New ButtonTile", "Asset", "Save ButtonTile", "Assets");
        if (path == "")
            return;
    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ButtonTile>(), path);
    }
#endif
}