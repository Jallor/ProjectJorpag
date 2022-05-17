using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileRuleCreatorWindowEditor : EditorWindow
{
    private static TileRuleCreatorWindowEditor _CurrentWindow;

    private Sprite _SprToUseCenter;
    private Sprite _SprToUseTopLeft;
    private Sprite _SprToUseTopRight;
    private Sprite _SprToUseDownLeft;
    private Sprite _SprToUseDownRight;

    [MenuItem("Editors/Tile Rule Creator")]
    public static void ShowWindow()
    {
        _CurrentWindow = (TileRuleCreatorWindowEditor)GetWindow(typeof(TileRuleCreatorWindowEditor));
        _CurrentWindow.Show();
        _CurrentWindow.minSize = new Vector2(400, 400);
    }

    [MenuItem("Editors/Debug/Reset Tile Rule Creator", priority = 0)]
    public static void ResetWindow()
    {
        _CurrentWindow.Close();
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.EndVertical();
    }
}
