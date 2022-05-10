using System.Collections;
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

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        // Character list

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
        OnGUI();

        // Do stuff
    }
}
