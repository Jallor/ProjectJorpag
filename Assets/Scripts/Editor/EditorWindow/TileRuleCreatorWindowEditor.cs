using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

using static TileRuleCreatorConfigScriptObj;

public class TileRuleCreatorWindowEditor : EditorWindow
{
    public enum TileCornerType
    {
        EMPTY,
        FULL_CORNER,
        LEFT_PART,
        RIGHT_PART,
        INNER_CORNER
    }

    private static TileRuleCreatorWindowEditor _CurrentWindow;

    private Sprite _SprToUseCenter = null;
    private Sprite _SprToUseTopLeft = null;
    private Sprite _SprToUseTopRight = null;
    private Sprite _SprToUseBottomLeft = null;
    private Sprite _SprToUseBottomRight = null;
    private Texture2D _TexToUseCenter = null;
    private Texture2D _TexToUseTopLeft = null;
    private Texture2D _TexToUseTopRight = null;
    private Texture2D _TexToUseBottomLeft = null;
    private Texture2D _TexToUseBottomRight = null;

    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsTopLeft = new Dictionary<TileCornerType,Texture2D>();
    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsTopRight = new Dictionary<TileCornerType,Texture2D>();
    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsBottomLeft = new Dictionary<TileCornerType,Texture2D>();
    private Dictionary<TileCornerType, Texture2D> _TileCornerPartsBottomRight = new Dictionary<TileCornerType,Texture2D>();

    private TileRuleCreatorConfigScriptObj _SelectedTileConfig = null;
    private string _OutputFolderPath = "/Arts/Sprites/Palettes/TileRules";
    private string _TileSetName = "";

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

        GUILayout.Label("Tile Rule Confif");
        _SelectedTileConfig = EditorGUILayout.ObjectField(_SelectedTileConfig, typeof(TileRuleCreatorConfigScriptObj)) as TileRuleCreatorConfigScriptObj;
        GUILayout.Label("Output Folder");
        _OutputFolderPath = GUILayout.TextField(_OutputFolderPath);
        GUILayout.Label("TileSet Name");
        _TileSetName = GUILayout.TextField(_TileSetName);

        if (GUILayout.Button("Export Tile Set"))
        {
            ExportTileSet();
        }

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

    private void ExportTileSet()
    {
        string fullPath = Application.dataPath + _OutputFolderPath;
        if (!Directory.Exists(fullPath + "/" + _TileSetName))
        {
            Directory.CreateDirectory(fullPath + "/" + _TileSetName);
        }

        int index = 0;
        foreach (TileConfig tileConfig in _SelectedTileConfig.TileConfigList)
        {
            if (tileConfig.RotateConfig)
            {
                for (int j = 0; j < 4; j++)
                {
                    CreatTile(tileConfig, fullPath, index, j);
                    ++index;
                }
            }
            else
            {
                CreatTile(tileConfig, fullPath, index, 0);
                ++index;
            }
        }
    }

    private void CreatTile(TileConfig tileConfig, string fullPath, int index, int cornerOffset)
    {
        Texture2D newTex = new Texture2D(48, 48);

        List<TileCornerType> tileCornerTypes = new List<TileCornerType>();
        tileCornerTypes.Add(tileConfig.CornerTopLeft);
        tileCornerTypes.Add(tileConfig.CornerTopRight);
        tileCornerTypes.Add(tileConfig.CornerBottomRight);
        tileCornerTypes.Add(tileConfig.CornerBottomLeft);

        Color[] pixelsTL = _TileCornerPartsTopLeft[tileCornerTypes[(0 + cornerOffset) % 4]].GetPixels(0, 0, 24, 24);
        Color[] pixelsTR = _TileCornerPartsTopRight[tileCornerTypes[(1 + cornerOffset) % 4]].GetPixels(0, 0, 24, 24);
        Color[] pixelsBR = _TileCornerPartsBottomRight[tileCornerTypes[(2 + cornerOffset) % 4]].GetPixels(0, 0, 24, 24);
        Color[] pixelsBL = _TileCornerPartsBottomLeft[tileCornerTypes[(3 + cornerOffset) % 4]].GetPixels(0, 0, 24, 24);

        newTex.SetPixels(0, 24, 24, 24, pixelsTL);
        newTex.SetPixels(24, 24, 24, 24, pixelsTR);
        newTex.SetPixels(24, 0, 24, 24, pixelsBR);
        newTex.SetPixels(0, 0, 24, 24, pixelsBL);
        newTex.Apply();

        byte[] file = newTex.EncodeToPNG();
        File.WriteAllBytes($"{fullPath}/{_TileSetName}/{_TileSetName}_{index.ToString("000")}.png", file);


    }
}
