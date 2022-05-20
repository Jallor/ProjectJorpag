using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelEditorWindowEditor : EditorWindow
{
    private const string _LevelEditorSceneName = "LevelEditor";
    private const string _TileMapsPath = "Datas/TileGridMaps";
    private string _SelectedTileMapName = "NewTileMap";
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

        GUILayout.Label("TileGridMap name");
        _SelectedTileMapName = GUILayout.TextField(_SelectedTileMapName);

        if (GUILayout.Button("Save TileGridMap"))
        {
            GameTileGrid tileGrid = FindObjectOfType<GameTileGrid>();
            SerializedTileGridMapData tileGrideMapData = tileGrid.SerializeTileGridMap();

            AssetDatabase.CreateAsset(tileGrideMapData, $"Assets/{_TileMapsPath}/{_SelectedTileMapName}.asset");
            AssetDatabase.Refresh();
            AllTileGridMapsDataList.Inst.RefreshDataList();
        }

        GUILayout.EndVertical();
    }
}
