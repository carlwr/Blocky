using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ConversationalAgentTile : Tile
{

public string text;
    
#if UNITY_EDITOR
// The following is a helper that adds a menu item to create a RoadTile Asset
    [MenuItem("Assets/Create/ConversationalAgentTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Conversational Agent Tile", "New Conversational Agent Tile", "Asset", "Save Road Tile", "Assets");
        if (path == "")
            return;
    AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ConversationalAgentTile>(), path);
    }
#endif
}