using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileRuleCreatorWindowEditor : EditorWindow
{
    private enum TileCornerType
    {
        EMPTY,
        FULL_CORNER,
        LEFT_PART,
        RIGHT_PART,
        INNER_CORNER
    }

    private static TileRuleCreatorWindowEditor _CurrentWindow;

    private Sprite _SprToUseCenter;
    private Sprite _SprToUseTopLeft;
    private Sprite _SprToUseTopRight;
    private Sprite _SprToUseBottomLeft;
    private Sprite _SprToUseBottomRight;
    private Texture2D _TexToUseCenter;
    private Texture2D _TexToUseTopLeft;
    private Texture2D _TexToUseTopRight;
    private Texture2D _TexToUseBottomLeft;
    private Texture2D _TexToUseBottomRight;

    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsTopLeft = new Dictionary<TileCornerType,Texture2D>();
    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsTopRight = new Dictionary<TileCornerType,Texture2D>();
    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsBottomLeft = new Dictionary<TileCornerType,Texture2D>();
    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsBottomRight = new Dictionary<TileCornerType,Texture2D>();

    [MenuItem("Editors/Tile Rule Creator")]
    public static void ShowWindow()
    {
        _CurrentWindow = (TileRuleCreatorWindowEditor)GetWindow(typeof(TileRuleCreatorWindowEditor));
        _CurrentWindow.Show();
        _CurrentWindow.minSize = new Vector2(800, 400);
    }

    [MenuItem("Editors/Debug/Reset Tile Rule Creator", priority = 0)]
    public static void ResetWindow()
    {
        _CurrentWindow.Close();
    }

    public void OnGUI()
    {
        GUILayout.BeginVertical();
        
        GUILayout.BeginHorizontal();
        ShowInputSprite(ref _SprToUseCenter, ref _TexToUseCenter, "Center Sprite");
        ShowInputSprite(ref _SprToUseTopLeft, ref _TexToUseTopLeft, "Top Left Sprite");
        ShowInputSprite(ref _SprToUseTopRight, ref _TexToUseTopRight, "Top Right Sprite");
        ShowInputSprite(ref _SprToUseBottomLeft, ref _TexToUseBottomLeft, "Bottom Left Sprite");
        ShowInputSprite(ref _SprToUseBottomRight, ref _TexToUseBottomRight, "Bottom Right Sprite");
        GUILayout.EndHorizontal();

        List<Texture2D> allCornerParts = new List<Texture2D>();
        allCornerParts.AddRange(_TileCornerPartsTopLeft.Values);
        allCornerParts.AddRange(_TileCornerPartsTopRight.Values);
        allCornerParts.AddRange(_TileCornerPartsBottomLeft.Values);
        allCornerParts.AddRange(_TileCornerPartsBottomRight.Values);
        GUILayout.Label($"All Corner Parts ({allCornerParts.Count})");
        GUILayout.BeginHorizontal();
        foreach (Texture2D tex in allCornerParts)
        {
            GUILayout.Box(tex);
        }
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }

    private void ShowInputSprite(ref Sprite sprHolder, ref Texture2D texHolder, string label)
    {
        GUILayout.BeginVertical();

        GUILayout.Label(label);
        Sprite spr = EditorGUILayout.ObjectField(sprHolder, typeof(Sprite)) as Sprite;
        if (spr != null && sprHolder != spr)
        {
            sprHolder = spr;

            texHolder = new Texture2D(48, 48);
            Rect rect = sprHolder.rect;
            Color[] pixels = sprHolder.texture.GetPixels((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);
            texHolder.SetPixels(pixels);
            texHolder.Apply();

            RefreshCornerParts();
        }
        if (texHolder != null)
        {
            GUILayout.Box(texHolder);
        }

        GUILayout.EndVertical();
    }

    private void RefreshCornerParts()
    {
        if (_TexToUseCenter)
        {
            List<Texture2D> splitedTex = SplitInputSpriteTexture(_TexToUseCenter);
            _TileCornerPartsTopLeft.Remove(TileCornerType.INNER_CORNER);
            _TileCornerPartsTopRight.Remove(TileCornerType.INNER_CORNER);
            _TileCornerPartsBottomLeft.Remove(TileCornerType.INNER_CORNER);
            _TileCornerPartsBottomRight.Remove(TileCornerType.INNER_CORNER);

            _TileCornerPartsTopLeft.Add(TileCornerType.INNER_CORNER, splitedTex[0]);
            _TileCornerPartsTopRight.Add(TileCornerType.INNER_CORNER, splitedTex[1]);
            _TileCornerPartsBottomLeft.Add(TileCornerType.INNER_CORNER, splitedTex[2]);
            _TileCornerPartsBottomRight.Add(TileCornerType.INNER_CORNER, splitedTex[3]);
        }
        if (_TexToUseTopLeft)
        {
            List<Texture2D> splitedTex = SplitInputSpriteTexture(_TexToUseTopLeft);
            _TileCornerPartsTopLeft.Remove(TileCornerType.FULL_CORNER);
            _TileCornerPartsTopRight.Remove(TileCornerType.LEFT_PART);
            _TileCornerPartsBottomLeft.Remove(TileCornerType.RIGHT_PART);
            _TileCornerPartsBottomRight.Remove(TileCornerType.EMPTY);

            _TileCornerPartsTopLeft.Add(TileCornerType.FULL_CORNER, splitedTex[0]);
            _TileCornerPartsTopRight.Add(TileCornerType.LEFT_PART, splitedTex[1]);
            _TileCornerPartsBottomLeft.Add(TileCornerType.RIGHT_PART, splitedTex[2]);
            _TileCornerPartsBottomRight.Add(TileCornerType.EMPTY, splitedTex[3]);
        }
        if (_TexToUseTopRight)
        {
            List<Texture2D> splitedTex = SplitInputSpriteTexture(_TexToUseTopRight);
            _TileCornerPartsTopLeft.Remove(TileCornerType.RIGHT_PART);
            _TileCornerPartsTopRight.Remove(TileCornerType.FULL_CORNER);
            _TileCornerPartsBottomLeft.Remove(TileCornerType.EMPTY);
            _TileCornerPartsBottomRight.Remove(TileCornerType.LEFT_PART);

            _TileCornerPartsTopLeft.Add(TileCornerType.RIGHT_PART, splitedTex[0]);
            _TileCornerPartsTopRight.Add(TileCornerType.FULL_CORNER, splitedTex[1]);
            _TileCornerPartsBottomLeft.Add(TileCornerType.EMPTY, splitedTex[2]);
            _TileCornerPartsBottomRight.Add(TileCornerType.LEFT_PART, splitedTex[3]);
        }
        if (_TexToUseBottomLeft)
        {
            List<Texture2D> splitedTex = SplitInputSpriteTexture(_TexToUseBottomLeft);
            _TileCornerPartsTopLeft.Remove(TileCornerType.LEFT_PART);
            _TileCornerPartsTopRight.Remove(TileCornerType.EMPTY);
            _TileCornerPartsBottomLeft.Remove(TileCornerType.FULL_CORNER);
            _TileCornerPartsBottomRight.Remove(TileCornerType.RIGHT_PART);

            _TileCornerPartsTopLeft.Add(TileCornerType.LEFT_PART, splitedTex[0]);
            _TileCornerPartsTopRight.Add(TileCornerType.EMPTY, splitedTex[1]);
            _TileCornerPartsBottomLeft.Add(TileCornerType.FULL_CORNER, splitedTex[2]);
            _TileCornerPartsBottomRight.Add(TileCornerType.RIGHT_PART, splitedTex[3]);
        }
        if (_TexToUseBottomRight)
        {
            List<Texture2D> splitedTex = SplitInputSpriteTexture(_TexToUseBottomRight);
            _TileCornerPartsTopLeft.Remove(TileCornerType.EMPTY);
            _TileCornerPartsTopRight.Remove(TileCornerType.RIGHT_PART);
            _TileCornerPartsBottomLeft.Remove(TileCornerType.LEFT_PART);
            _TileCornerPartsBottomRight.Remove(TileCornerType.FULL_CORNER);

            _TileCornerPartsTopLeft.Add(TileCornerType.EMPTY, splitedTex[0]);
            _TileCornerPartsTopRight.Add(TileCornerType.RIGHT_PART, splitedTex[1]);
            _TileCornerPartsBottomLeft.Add(TileCornerType.LEFT_PART, splitedTex[2]);
            _TileCornerPartsBottomRight.Add(TileCornerType.FULL_CORNER, splitedTex[3]);
        }
    }

    private List<Texture2D> SplitInputSpriteTexture(Texture2D texToSplit)
    {
        List<Texture2D> splitedTexs = new List<Texture2D>();

        Texture2D texTL = new Texture2D(24, 24);
        Texture2D texTR = new Texture2D(24, 24);
        Texture2D texBL = new Texture2D(24, 24);
        Texture2D texBR = new Texture2D(24, 24);

        texTL.SetPixels(texToSplit.GetPixels(0, 23, 24, 24));
        texTR.SetPixels(texToSplit.GetPixels(23, 23, 24, 24));
        texBL.SetPixels(texToSplit.GetPixels(0, 0, 24, 24));
        texBR.SetPixels(texToSplit.GetPixels(23, 0, 24, 24));

        texTL.Apply();
        texTR.Apply();
        texBL.Apply();
        texBR.Apply();

        splitedTexs.Add(texTL);
        splitedTexs.Add(texTR);
        splitedTexs.Add(texBL);
        splitedTexs.Add(texBR);

        return (splitedTexs);
    }
}
