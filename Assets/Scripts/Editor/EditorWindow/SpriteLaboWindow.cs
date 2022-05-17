using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class SpriteLaboWindow : EditorWindow
{
    private static SpriteLaboWindow _CurrentWindow;

    [MenuItem("Editors/Sprite Labo")]
    public static void ShowWindow()
    {
        _CurrentWindow = (SpriteLaboWindow)GetWindow(typeof(SpriteLaboWindow));
        _CurrentWindow.Show();
        _CurrentWindow.minSize = new Vector2(400, 400);
    }

    [MenuItem("Editors/Debug/Sprite Labo", priority = 0)]
    public static void ResetWindow()
    {
        _CurrentWindow.Close();
    }

    Texture2D tex;
    Texture2D tex2;
    Texture2D tex3;
    Texture2D tex4;
    Texture2D tex5;
    Color filterColor;
    Texture2D FilterResult;
    ECalcType CalculTypeID;

    public SpriteLaboWindow()
    {
    }

    void OnEnable()
    {
        tex = new Texture2D(10, 10);
        tex2 = new Texture2D(10, 10);
        tex3 = new Texture2D(10, 10);
        tex4 = new Texture2D(10, 10);
        tex5 = new Texture2D(10, 10);
        filterColor = Color.white;
        FilterResult = new Texture2D(10, 10);
        CalculTypeID = ECalcType.Vanilla;
    }
    void OnGUI()
    {
        ShowGUI();
    }

    void ShowGUI()
    {
        BidouilleWithBrickTexture();
        ApplyColorFilter();
    }

    void BidouilleWithBrickTexture()
    {
        if (GUILayout.Button("Button"))
        {
            Debug.Log("plop");
            // tex = AllTiles.Instance.getTileFromID(1).SprForUI.texture;
            // Debug.Log(AllTiles.Instance.getTileFromID(1).SprForUI.texture.name);

            Texture2D newTex = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
            Color[] allPixel = tex.GetPixels();
            for (int i = 0; i < allPixel.Length; i++)
            {
                allPixel[i].r = 1f - allPixel[i].r;
                allPixel[i].g = 1f - allPixel[i].g;
                allPixel[i].b = 1f - allPixel[i].b;
            }
            newTex.SetPixels(0, 0, tex.width, tex.height, allPixel);
            newTex.Apply();

            byte[] file = newTex.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/testscreen.png", file);
        }
        else
        {
            /*Texture2D tex = new Texture2D(100, 100);
            tex.SetPixel(0, 0, Color.red);
            tex.SetPixel(1, 0, Color.red);
            tex.SetPixel(1, 1, Color.red);
            tex.SetPixel(0, 1, Color.red);
            tex.SetPixel(2, 2, Color.red);
            tex.Apply();
            GUILayout.Box(tex);*/
        }
        // GUILayout.Box(tex);
    }

    enum ECalcType
    {
        Vanilla,
        Moyenne,
        Inversed_Saturation,
        Inversed_Color
    }
    void ApplyColorFilter()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Open A File", GUILayout.Width(100)))
        {
            var path = EditorUtility.OpenFilePanel(
                  "Open A Sprite", "Assets/Resources", "png");
            Debug.Log(path);
            tex3.LoadImage(File.ReadAllBytes(path));
            tex3.Apply();
            FilterResult = new Texture2D(tex3.width, tex3.height);
            CalculColorFilter();
        }
        Color c;
        if ((c = EditorGUILayout.ColorField(filterColor, GUILayout.Width(100))) != filterColor)
        {
            filterColor = c;
            CalculColorFilter();
        }
        ECalcType ct;
        if ((ct = (ECalcType)EditorGUILayout.EnumPopup(CalculTypeID, GUILayout.Width(150))) != CalculTypeID)
        {
            CalculTypeID = ct;
            CalculColorFilter();
        }
        GUILayout.EndHorizontal();
        GUILayout.Box(FilterResult);
    }
    void CalculColorFilter()
    {
        Color[] allPix = tex3.GetPixels();
        for (int i = 0; i < allPix.Length; i++)
        {
            if (CalculTypeID == ECalcType.Vanilla)
            {
                allPix[i].r = filterColor.r * allPix[i].r;
                allPix[i].g = filterColor.g * allPix[i].g;
                allPix[i].b = filterColor.b * allPix[i].b;
            }
            else if (CalculTypeID == ECalcType.Moyenne)
            {
                allPix[i].r = (allPix[i].r + filterColor.r) / 2;
                allPix[i].g = (allPix[i].g + filterColor.g) / 2;
                allPix[i].b = (allPix[i].b + filterColor.b) / 2;
            }
            else if (CalculTypeID == ECalcType.Inversed_Saturation)
            {
                allPix[i].r = filterColor.r - allPix[i].r;
                allPix[i].g = filterColor.g - allPix[i].g;
                allPix[i].b = filterColor.b - allPix[i].b;
            }
            else if (CalculTypeID == ECalcType.Inversed_Color)
            {
                allPix[i].r = allPix[i].r - filterColor.r;
                allPix[i].g = allPix[i].g - filterColor.g;
                allPix[i].b = allPix[i].b - filterColor.b;
            }
        }
        FilterResult.SetPixels(allPix);
        FilterResult.Apply();
    }
}
