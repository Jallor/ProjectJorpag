using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class LevelEditorWindowEditor : EditorWindow
{
    private const string _LevelEditorSceneName = "LevelEditor";
    private const string _TileMapsPath = "Datas/TileGridMaps";

    private static LevelEditorWindowEditor _CurrentWindow;

    private string _SelectedTileMapName = "NewTileMap";
    private int _SelectMapToOpenIndex = 0;
    private string[] _TileGridMapNameList = new string[0];

    [MenuItem("Editors/Level Editor")]
    public static void ShowWindow()
    {
        _CurrentWindow = (LevelEditorWindowEditor)GetWindow(typeof(LevelEditorWindowEditor));
        _CurrentWindow.Show();
        _CurrentWindow.minSize = new Vector2(400, 400);
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
            GUILayout.Label("Wrong Scene ! Open " + _LevelEditorSceneName + " Scene.");
            if (GUILayout.Button("Open " + _LevelEditorSceneName))
            {
                EditorSceneManager.OpenScene("Assets/Scenes/" + _LevelEditorSceneName + ".unity");
            }
            return;
        }

        GUILayout.BeginVertical();

        if (GUILayout.Button("Clear Grid"))
        {
            GameTileGrid tileGrid = FindObjectOfType<GameTileGrid>();
            tileGrid.Clear();
        }

        GUILayout.Space(10);

        GUILayout.Label("TileGridMap name to save");
        _SelectedTileMapName = GUILayout.TextField(_SelectedTileMapName);
        if (GUILayout.Button("Save TileGridMap"))
        {
            SaveMap();
        }

        GUILayout.Space(10);

        GUILayout.Label("TileGridMap to open");
        _SelectMapToOpenIndex = EditorGUILayout.Popup(_SelectMapToOpenIndex, _TileGridMapNameList);
        if (GUILayout.Button("Load TileGridMap"))
        {
            LoadMap();
        }


        GUILayout.EndVertical();
    }

    private void SaveMap()
    {
        GameTileGrid tileGrid = FindObjectOfType<GameTileGrid>();
        SerializedTileGridMapData tileGrideMapData = tileGrid.SerializeTileGridMap();

        AssetDatabase.CreateAsset(tileGrideMapData, $"Assets/{_TileMapsPath}/{_SelectedTileMapName}.asset");
        AssetDatabase.Refresh();
        AllTileGridMapsDataList.Inst.RefreshDataList();

        UpdateTileGridMapList();
    }

    private void LoadMap()
    {
        SerializedTileGridMapData tileGridMapData = AllTileGridMapsDataList.Inst.dataList[_SelectMapToOpenIndex];
        GameTileGrid tileGrid = FindObjectOfType<GameTileGrid>();

        tileGrid.LoadTileGridMap(tileGridMapData);
    }

    public void Awake()
    {
        HeavyGuiChange();
    }

    public void OnFocus()
    {
        HeavyGuiChange();
        UpdateTileGridMapList();
    }

    public void OnInspectorUpdate()
    {
        HeavyGuiChange();
    }

    private void HeavyGuiChange()
    {
    }

    private void UpdateTileGridMapList()
    {
        List<SerializedTileGridMapData> mapList = AllTileGridMapsDataList.Inst.dataList;

        _TileGridMapNameList = new string[mapList.Count];
        for (int i = 0; i < mapList.Count; i++)
        {
            _TileGridMapNameList[i] = mapList[i].name;
        }
    }
}
