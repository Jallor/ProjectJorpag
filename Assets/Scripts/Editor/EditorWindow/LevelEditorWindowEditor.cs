using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelEditorWindowEditor : EditorWindow
{
    private const string _LevelEditorSceneName = "LevelEditor";
    private static LevelEditorWindowEditor _CurrentWindow;

    [MenuItem("Editors/Level Editor")]
    public static void ShowWindow()
    {
        if (SceneManager.GetActiveScene().name == _LevelEditorSceneName)
        {
            _CurrentWindow = (LevelEditorWindowEditor)GetWindow(typeof(LevelEditorWindowEditor));
            _CurrentWindow.Show();
            _CurrentWindow.minSize = new Vector2(400, 400);
        }
        else
        {
            Debug.LogError("Can be open only in scene " + _LevelEditorSceneName);
        }
    }

    [MenuItem("Editors/Debug/Reset Level Editor", priority = 0)]
    public static void ResetWindow()
    {
        _CurrentWindow.Close();
    }

    public void OnGUI()
    {
        if (SceneManager.GetActiveScene().name != _LevelEditorSceneName)
        {
            _CurrentWindow.Close();
            return;
        }

        GUILayout.BeginVertical();

        if (GUILayout.Button("Test Read Map"))
        {
            Tilemap map = FindObjectOfType<Tilemap>();
            Debug.Log(map.name);
            TileBase tile = map.GetTile(new Vector3Int(-12, -4, 0));
            TileBase[] allTile = map.GetTilesBlock(map.cellBounds);
            // map.SetTiles()
        }

        GUILayout.EndVertical();
    }
}
