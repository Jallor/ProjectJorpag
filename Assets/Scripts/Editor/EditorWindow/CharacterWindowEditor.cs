using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterWindowEditor : EditorWindow
{
    private static CharacterWindowEditor _CurrentWindow;

    [MenuItem("Editors/Character Editor")]
    public static void ShowWindow()
    {
        _CurrentWindow = (CharacterWindowEditor)GetWindow(typeof(CharacterWindowEditor));
        _CurrentWindow.Show();
        _CurrentWindow.minSize = new Vector2(500, 600);
    }

    [MenuItem("Editors/Debug/Reset Character Editor", priority = 0)]
    public static void ResetWindow()
    {
        _CurrentWindow.Close();
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        // Character list
        GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(150));
        List<CharacterData> charaList = AllCharactersDataList.Instance.GetDataList();

        foreach (CharacterData charaData in charaList)
        {
            if (GUILayout.Button(charaData.CharaName))
            {
            }
        }
        GUILayout.EndVertical();

        // Character details

        GUILayout.EndHorizontal();
    }

    public void Awake()
    {
        HeavyGuiChange();
    }

    public void OnFocus()
    {
        HeavyGuiChange();
    }

    public void OnInspectorUpdate()
    {
        HeavyGuiChange();
    }

    private void HeavyGuiChange()
    {
        // Do stuff
    }
}
