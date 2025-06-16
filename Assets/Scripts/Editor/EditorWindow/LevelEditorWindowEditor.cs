using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

public class LevelEditorWindowEditor : EditorWindow
{
    private const string _LevelEditorSceneName = "LevelEditor";
    private const string _TileMapsPath = "Datas/Scenaris/TileGridMaps";

    private static LevelEditorWindowEditor _CurrentWindow;

    private string _SelectedTileMapName = "NewTileMap";
    private int _SelectMapToOpenIndex = 0;
    private string[] _TileGridMapNameList = new string[0];

    // TODO Add a bool to partially hide other layer of the game grid

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
        _CurrentWindow = null;
    }

    public void OnGUI()
    {
        SceneView.duringSceneGui -= OnScene;
        SceneView.duringSceneGui += OnScene;

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

    public void OnScene(SceneView sceneview)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LevelEditor")
        {
            Color[] pix = new Color[2 * 2];
            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = Color.red;
            }
            Texture2D result = new Texture2D(2, 2);
            result.SetPixels(pix);
            result.Apply();
            GUIStyle g = new GUIStyle();
            g.normal.background = result;
            g.fontSize = 17;
            g.normal.textColor = Color.white;
            g.alignment = TextAnchor.MiddleCenter;

            // TODO : finish to compute the grid position
            Handles.BeginGUI();
            GUI.skin.label.fontSize = 11;
            float mult = EditorGUIUtility.pixelsPerPoint;
            Vector2 mouseScreenPosition = Event.current.mousePosition * mult;
            mouseScreenPosition.y -= sceneview.position.height;
            GameTileGrid grid = FindAnyObjectByType<GameTileGrid>();
            Vector3 mouseGridPosition = grid.WorldPositionToGridPosition(sceneview.camera.ScreenToWorldPoint(mouseScreenPosition));
            GUI.Label(new Rect(0, 0, sceneview.position.width, 20),
                "LEVEL EDITOR (" + mouseGridPosition.x + "; " + mouseGridPosition.y + ")", g);
        }
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

        _SelectedTileMapName = _TileGridMapNameList[_SelectMapToOpenIndex];
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

// Old Code From Magma Mobile !
/*﻿
public class LevelEditorWindow : EditorWindow
{
    [MenuItem("Magma Mobile/BB Rescue 2 - Level Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LevelEditorWindow));
    }

    private Color defaultColor;
    private Color defaultContentColor;
    private Color defaultBackgroundColor;

    public LevelEditorWindow()
    {
        defaultColor = GUI.color;
        defaultContentColor = GUI.contentColor;
        defaultBackgroundColor = GUI.backgroundColor;

        this.autoRepaintOnSceneChange = true;

        EditorApplication.hierarchyWindowChanged += hierarchyWindowChanged;
    }

    private string currentLoadedScene;
    private void hierarchyWindowChanged()
    {
        if (currentLoadedScene != UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            || LatestScenes.reloadedBuildingScene)
        {

            currentLoadedScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            //a scene change has happened
            if (currentLoadedScene == "SceneBuilding")
            {
                LatestScenes.reloadedBuildingScene = false;

                clickOnLevelButton(currentLevelIdx);
            }
        }
    }

    private bool isReadyToUse = false;

    #region Paths
    const string AllLevelFolder = "Assets/@Levels/"; // dossier dans lequel sont stocké TOUS les niveaux 

    enum EnumUser
    {
        Dev,
        Rachel,
        Prod
    } // liste des différents user
    private static EnumUser CurrentUser
    {
        get
        {
            return ((EnumUser)EditorPrefs.GetInt("currentUser", 0));
        }
        set
        {
            EditorPrefs.SetInt("currentUser", (int)value);
        }
    } // user actuel qui fais les niveaux (possibilité de changer d'user durant l'édition)
    const string LevelListFile = "/LevelsList.txt"; // fichier qui liste l'ordre des Lv
    const string LevelFolder = "/LevelItems/"; // dossier qui contient tous les Lv

    string PathLevelsListTxt
    {
        get
        {
            return (AllLevelFolder + CurrentUser + LevelListFile);
        }
    } // getter du path complet du fichier contenant la liste des levels 
    string PathLevelFolder
    {
        get
        {
            return (AllLevelFolder + CurrentUser + LevelFolder);
        }
    } // getter du path complet du dossier contenant tous les levels
    #endregion

    // Scrollview params
    private Vector2 _scrollLevels = new Vector2(0, EditorPrefs.GetFloat("scrollLevelsY", 0));

    // Level Params
    private int currentLevelIdx = 0; // index du niveau courant
    static LevelProfile currentProfile = null;
    static LevelProfile clipBoarbLevelProfile = null;
    static LevelProfile tmpLevel = null;
    static List<string> levelsList = new List<string>(); // liste des GUID de niveaux
    static List<LevelProfile> levels = new List<LevelProfile>(); // liste des niveaux avec toutes les data qu'il faut dedans (liste des obj, ...)
    static bool isSceneEdited = false;
    bool isPhysicTesing = false;
    private int nbCageInScene = 0;
    static int toolbarInt = 0;
    private string[] toolbarStrings = new string[] { "Cage", "World", "Monsters", "Other", "Weather", "LevelOptions" };

    // Prefabs
    static Dictionary<string, prefabNFO> prefabsList = new Dictionary<string, prefabNFO>();

    static string[] prefabListOptions;

    private static GUIStyle _labelButton;
    public static GUIStyle labelButton
    {
        get
        {
            if (_labelButton == null)
            {
                _labelButton = new GUIStyle(GUI.skin.GetStyle("label"));
                _labelButton.wordWrap = true;
                _labelButton.normal.textColor = Color.white;
                _labelButton.alignment = TextAnchor.MiddleCenter;
            }
            return _labelButton;
        }
    }

    public void resetColors()
    {
        GUI.color = defaultColor;
        GUI.contentColor = defaultContentColor;
        GUI.backgroundColor = defaultBackgroundColor;
    }

    void OnEnable()
    {
        if (!isReadyToUse)
        {
            int i;
            i = EditorUtility.DisplayDialogComplex("BubbleBlastRescue2", "Select a User name for edit levels", "Dev", "Rachel", "Prod");
            switch (i)
            {
                case 0:
                    CurrentUser = EnumUser.Dev;
                    break;
                case 1:
                    CurrentUser = EnumUser.Rachel;
                    break;
                case 2:
                    CurrentUser = EnumUser.Prod;
                    break;
                default:
                    break;
            }
            isReadyToUse = true;
        }

        isPhysicTesing = false;
        currentLevelIdx = EditorPrefs.GetInt("currentLevelIdx", 0);
        loadLevelsList();
        prefabsList.Clear();
        DirectoryInfo directory = new DirectoryInfo("Assets/@PrefabsScene/");
        if (directory.Exists)
        {
            System.IO.FileInfo[] unityPackageFiles = directory.GetFiles("*.prefab", SearchOption.AllDirectories).Where(fi => (fi.Attributes & FileAttributes.Hidden) == 0).ToArray();

            for (int f = 0; f < unityPackageFiles.Count(); f++)
            {
                enumPrefabType tmpEnum = enumPrefabType.Other;
                Object prefab = AssetDatabase.LoadAssetAtPath("Assets" + unityPackageFiles[f].FullName.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject));
                if (((GameObject)prefab).GetComponent<BBCage>())
                {
                    tmpEnum = enumPrefabType.Cage;
                }
                if (((GameObject)prefab).GetComponent<BBMonster>())
                {
                    tmpEnum = enumPrefabType.Monster;
                }
                if (((GameObject)prefab).GetComponent<BBWorld>())
                {
                    tmpEnum = enumPrefabType.World;
                }
                if (((GameObject)prefab).GetComponent<BBWeather>())
                {
                    tmpEnum = enumPrefabType.Weather;
                }

                prefabsList.Add(
                    unityPackageFiles[f].Name.Replace(".prefab", ""),
                    new prefabNFO(
                        unityPackageFiles[f].Name.Replace(".prefab", ""),
                        unityPackageFiles[f].FullName,
                        tmpEnum,
                        AssetPreview.GetAssetPreview(AssetDatabase.LoadAssetAtPath("Assets" + unityPackageFiles[f].FullName.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject)))
                    ));
            }
        }

        if (!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            clickOnLevelButton(currentLevelIdx);
        }
    }

    bool isPlaying = false;
    int idxForScreenshots = -1;
    void Update()
    {
        if (!isPlaying && EditorApplication.isPlaying)
        {
            // Le jeu comence a "play"
            isPlaying = true;
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SceneBuilding")
            {
                saveCurrentProfile();
                cleanGameObjectHasChanged();
                isSceneEdited = false;
            }
        }
        if (isPlaying && !EditorApplication.isPlaying)
        {
            // Le jeu quitte le mode "play"
            isPlaying = false;
            isPhysicTesing = false;
            clickOnLevelButton(currentLevelIdx);
        }

        if (idxForScreenshots >= 0)
            TakeAScreen();
    }

    private void OnScene(SceneView sceneview)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SceneBuilding")
        {

            Color[] pix = new Color[2 * 2];
            for (int i = 0; i < pix.Length; i++)
            {
                if (isSceneEdited)
                    pix[i] = Color.red;
                else
                    pix[i] = new Color(0.25f, 0.6f, 0);
            }
            Texture2D result = new Texture2D(2, 2);
            result.SetPixels(pix);
            result.Apply();
            GUIStyle g = new GUIStyle();
            g.normal.background = result;
            g.fontSize = 17;
            g.normal.textColor = Color.white;
            g.alignment = TextAnchor.MiddleCenter;

            Handles.BeginGUI();
            GUI.skin.label.fontSize = 11;
            GUI.Label(new Rect(0, 0, 800, 20), "SCENE BUILDING", g);

            if (isSceneEdited)
            {
                if (GUI.Button(new Rect(0, 0, 70, 19), "Save"))
                {
                    saveCurrentProfile();
                    cleanGameObjectHasChanged();
                    isSceneEdited = false;
                }
            }
            Handles.EndGUI();

            for (int i = 0; i < pix.Length; i++)
            {
                pix[i] = Color.magenta;
            }
            result = new Texture2D(2, 2);
            result.SetPixels(pix);
            result.Apply();
            g.normal.background = result;

            if (isPhysicTesing)
            {
                Handles.BeginGUI();
                GUI.skin.label.fontSize = 11;
                GUI.Label(new Rect(0, 20, 800, 20), "TESTING", g);
                Handles.EndGUI();
            }
        }
    }

    void OnGUI()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        SceneView.onSceneGUIDelegate += OnScene;

        if (currentProfile == null)
        {
            if (currentLevelIdx < levels.Count)
            {
                currentProfile = levels[currentLevelIdx];
            }
        }
        GUILayout.BeginHorizontal();
        leftCol();
        rightCol();
        GUILayout.EndHorizontal();

        if (!isSceneEdited)
        {
            if (Selection.activeGameObject && Selection.activeGameObject.transform.hasChanged == true)
            {
                isSceneEdited = true;
            }
        }
    }

    int numberOfMove = 1; // stock le nombre de déplacement que fais le lv en cliquant sur ▲ ou ▼
    void leftCol()
    {
        GUILayout.BeginVertical(GUILayout.Width(300));
        GUILayout.BeginHorizontal();

        GUILayout.Label("Levels list", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "SceneBuilding")
        {
            if (GUILayout.Button("Load SceneBuilding", GUILayout.Width(125)))
            {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene("Assets/@Scenes/SceneBuilding.unity");
                }
            }
        }
        else if (GUILayout.Button("New Level", GUILayout.Width(125)))
        {
            LevelProfile lp = new LevelProfile();
            lp.fileName = System.Guid.NewGuid().ToString();
            levels.Add(lp);
            levelsList.Add(lp.fileName);
            lp.saveMe(PathLevelFolder);
            saveLevelsList();
            AssetDatabase.Refresh();
            currentLevelIdx = levels.Count - 1;
            clickOnLevelButton(currentLevelIdx);
            EditorPrefs.SetInt("currentLevelIdx", currentLevelIdx);
        }

        EnumUser newUser;
        newUser = (EnumUser)EditorGUILayout.EnumPopup(CurrentUser);
        if (newUser != CurrentUser)
        {
            CurrentUser = newUser;
            loadLevelsList();
            if (currentLevelIdx >= levels.Count)
            {
                currentLevelIdx = levels.Count - 1;
            }
            clickOnLevelButton(currentLevelIdx);
            EditorPrefs.SetInt("currentLevelIdx", currentLevelIdx);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("x1", GUILayout.Width(40)))
            numberOfMove = 1;
        if (GUILayout.Button("x10", GUILayout.Width(40)))
            numberOfMove = 10;
        if (GUILayout.Button("x50", GUILayout.Width(40)))
            numberOfMove = 50;
        if (GUILayout.Button("x100", GUILayout.Width(40)))
            numberOfMove = 100;
        GUILayout.EndHorizontal();

        _scrollLevels = EditorGUILayout.BeginScrollView(_scrollLevels, false, true, new GUILayoutOption[] { GUILayout.MaxHeight((position.height)), GUILayout.Width(290) });

        for (int i = 0; i < levelsList.Count; i++)
        {
            GUIStyle LvNameStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            LvNameStyle.alignment = TextAnchor.MiddleLeft;
            if (i == currentLevelIdx)
            {
                GUILayout.BeginHorizontal();

                GUI.backgroundColor = Color.gray + Color.red;
                GUI.contentColor = Color.white;

                if (GUILayout.Button(numberOfMove + "▼", GUILayout.Width(40)))
                {
                    for (int j = 0; j < numberOfMove; j++)
                    {
                        if (currentLevelIdx < levelsList.Count - 1)
                        {
                            levelsList.Swap(i, i + 1);
                            levels.Swap(i, i + 1);
                            currentLevelIdx += 1;
                            ++i;
                            saveLevelsList(false);
                        }
                    }
                }
                if (GUILayout.Button("Level " + int2String0000(i + 1), GUILayout.Width(90)))
                {
                    int saveIt = 1;
                    if (!isSceneEdited || (saveIt = EditorUtility.DisplayDialogComplex("BubbleBlastRescue2", "Do you want to save the changes you made in this level ?", "Save", "Don't save", "Cancel")) != 2)
                    {
                        currentLevelIdx = i;
                        EditorPrefs.SetInt("currentLevelIdx", currentLevelIdx);

                        if (saveIt == 0)
                        {
                            saveCurrentProfile();
                            cleanGameObjectHasChanged();
                            isSceneEdited = false;
                        }

                        clickOnLevelButton(currentLevelIdx);
                    }
                }
                if (GUILayout.Button(numberOfMove + "▲", GUILayout.Width(40)))
                {
                    for (int j = 0; j < numberOfMove; j++)
                    {
                        if (currentLevelIdx > 0)
                        {
                            levelsList.Swap(i, i - 1);
                            levels.Swap(i, i - 1);
                            currentLevelIdx -= 1;
                            --i;
                            saveLevelsList(false);
                        }
                    }
                }
                string lvName = currentProfile.levelName;
                if (currentProfile.nbTuto != 0)
                    lvName = "*" + lvName;
                if (currentProfile.nbCinematic != 0)
                    lvName = "#" + lvName;
                GUILayout.Label(lvName, LvNameStyle); // max 12 char
                GUILayout.EndHorizontal();
            }
            else
            {
                GUI.backgroundColor = Color.gray + Color.blue;
                GUI.contentColor = Color.white;

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Level " + int2String0000(i + 1), GUILayout.Width(178)))
                {
                    int saveIt = 1;
                    if (!isSceneEdited || (saveIt = EditorUtility.DisplayDialogComplex("BubbleBlastRescue2", "Do you want to save the changes you made in this level ?", "Save", "Don't save", "Cancel")) != 2)
                    {
                        currentLevelIdx = i;
                        EditorPrefs.SetInt("currentLevelIdx", currentLevelIdx);

                        if (saveIt == 0)
                        {
                            saveCurrentProfile();
                            cleanGameObjectHasChanged();
                            isSceneEdited = false;
                        }

                        clickOnLevelButton(currentLevelIdx);
                    }
                }
                string lvName = levels[i].levelName;
                if (levels[i].nbTuto != 0)
                    lvName = "*" + lvName;
                if (levels[i].nbCinematic != 0)
                    lvName = "#" + lvName;
                GUILayout.Label(lvName, LvNameStyle); // max 12 char
                GUILayout.EndHorizontal();
            }
            //EditorGUILayout.EndHorizontal();
        }
        GUI.backgroundColor = Color.gray + Color.blue;
        GUI.contentColor = Color.white;


        EditorGUILayout.EndScrollView();

        GUILayout.EndVertical();
    }

    void rightCol()
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal(GUILayout.Height(20));
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("SAVE", GUILayout.Width(125)))
        {
            saveCurrentProfile();
            cleanGameObjectHasChanged();
            isSceneEdited = false;
        }
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = new Color(1f, 0.7f, 0);
        if (LevelsManagerPrefab.Instance != null)
        {
            if (GUILayout.Button("Test in-Game", GUILayout.Width(125)))
            {
                CompileLevels();
                SceneAsset.FindObjectOfType<LevelsManagerPrefab>().lvToLoad = currentLevelIdx;
                UnityEngine.SceneManagement.SceneManager.LoadScene("SceneGame");
            }
        }
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Compile", GUILayout.Width(125)))
        {
            CompileLevels();
        }
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Take Screenshots", GUILayout.Width(125)))
        {
            idxForScreenshots = 0;
        }
        GUILayout.EndHorizontal();

        string warningMsg = "";
        GUI.backgroundColor = Color.gray;
        GUIStyle gBox = new GUIStyle();
        if (Event.current.type == EventType.Layout)
        {
            if (isSceneEdited && currentLoadedScene == "SceneBuilding")
            {
                warningMsg += "The scene have changed";
            }
        }

        Color[] pix = new Color[2 * 2];
        for (int i = 0; i < pix.Length; i++)
        {
            if (isSceneEdited)
            {
                pix[i] = Color.red + Color.black;
            }
            else
            {
                pix[i] = Color.grey;
            }
        }
        Texture2D result = new Texture2D(2, 2);
        result.SetPixels(pix);
        result.Apply();
        gBox.normal.background = result;
        gBox.fontSize = 20;
        gBox.normal.textColor = Color.white;

        EditorGUILayout.BeginHorizontal(gBox);

        EditorGUILayout.BeginVertical(GUILayout.Width(350));

        GUILayout.BeginHorizontal();
        GUILayout.Label("FileName : ", EditorStyles.boldLabel, GUILayout.Width(75));
        if (currentProfile != null)
            GUILayout.Label(currentProfile.fileName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("LevelName : ", EditorStyles.boldLabel, GUILayout.Width(75));
        if (currentProfile != null)
        {
            string tmpLvName;
            tmpLvName = GUILayout.TextField(currentProfile.levelName, GUILayout.Width(100));
            if (currentProfile.levelName != tmpLvName)
            {
                isSceneEdited = true;
                if (tmpLvName.Length > 12)
                {
                    Debug.LogError("Maximum 12 characters for the LevelName");
                    tmpLvName = tmpLvName.Substring(0, 12);
                }
                if (tmpLvName.Contains(";"))
                    tmpLvName = tmpLvName.Replace(';', '_');
                if (tmpLvName.Contains("*"))
                    tmpLvName = tmpLvName.Replace('*', '_');
                if (tmpLvName.Contains("#"))
                    tmpLvName = tmpLvName.Replace('#', '_');
                currentProfile.levelName = tmpLvName;
            }
        }
        GUILayout.EndHorizontal();

        List<BBMonster> lstMonsters = SceneAsset.FindObjectsOfType<BBMonster>().ToList<BBMonster>();
        GUILayout.BeginHorizontal();
        GUILayout.Label("nb of Monstre : ", EditorStyles.boldLabel, GUILayout.Width(100));
        GUILayout.Label("" + lstMonsters.Count);
        GUILayout.EndHorizontal();
        List<BBObject> lstObj = SceneAsset.FindObjectsOfType<BBObject>().ToList<BBObject>();
        GUILayout.BeginHorizontal();
        GUILayout.Label("nb of Object : ", EditorStyles.boldLabel, GUILayout.Width(100));
        GUILayout.Label("" + lstObj.Count);
        GUILayout.EndHorizontal();

        GUI.backgroundColor = Color.grey;
        if (GUILayout.Button("Open Level File", GUILayout.Width(110)))
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(PathLevelFolder + currentProfile.fileName + ".txt");
            System.Diagnostics.Process.Start(fi.FullName);
        }
        resetColors();
        if (isSceneEdited)
        {
            GUI.backgroundColor = Color.red;
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();
        if ((warningMsg != "" || nbCageInScene != 3) && currentLoadedScene == "SceneBuilding")
        {
            GUILayout.Label("WARNING :\n" + warningMsg);
            if (nbCageInScene != 3)
            {
                GUI.contentColor = Color.red;
                GUILayout.Label("There isn't 3 cages");
                resetColors();
            }
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        resetColors();

        GUILayout.BeginHorizontal(GUILayout.Height(40));

        GUI.backgroundColor = Color.gray + Color.cyan;
        GUI.contentColor = Color.white;
        if (SceneBuildingManagerPrefab.Instance != null)
        {
            if (!isPhysicTesing)
            {
                if (GUILayout.Button("Test Physic", GUILayout.Width(130)))
                {
                    isPhysicTesing = true;
                    tmpLevel = new LevelProfile("tmpLevel");
                    saveTmpLevel();
                    SceneBuildingManagerPrefab.Instance.YouCanMove();
                    loadTmpLevel();
                }
            }
            else
            {
                if (GUILayout.Button("Stop Physic Test", GUILayout.Width(130)))
                {
                    isPhysicTesing = false;
                    loadTmpLevel();
                    SceneBuildingManagerPrefab.Instance.TheyWillDontMove();
                }

            }
        }
        if (GUILayout.Button("Copy", GUILayout.Width(125)))
        {
            clipBoarbLevelProfile = currentProfile.getClone();
        }
        if (clipBoarbLevelProfile != null)
        {
            if (GUILayout.Button("Paste", GUILayout.Width(125)))
            {
                isSceneEdited = true;
                string tmpFilename = currentProfile.fileName;
                currentProfile = clipBoarbLevelProfile.getClone();

                if (tmpFilename != "")
                {
                    currentProfile.fileName = tmpFilename;
                }
                loadCurrentProfile();
                clipBoarbLevelProfile = null;
            }
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();

        GUI.backgroundColor = Color.gray + Color.green;
        List<GameObject> lstCage = SceneAsset.FindObjectsOfType<BBCage>().ToList<BBCage>().Select(t => t.gameObject).ToList<GameObject>();
        toolbarStrings[0] = "Cage (" + lstCage.Count + "/3)";
        toolbarInt = GUILayout.Toolbar(toolbarInt, toolbarStrings, GUILayout.Width(600));
        switch (toolbarInt)
        {
            case 0:
                displayObjButons(enumPrefabType.Cage, "Cage");
                break;
            case 1:
                displayObjButons(enumPrefabType.World, "Worlds");
                break;
            case 2:
                displayObjButons(enumPrefabType.Monster, "Monsters");
                break;
            case 3:
                displayObjButons(enumPrefabType.Other, "Others");
                break;
            case 4:
                displayObjButons(enumPrefabType.Weather, "Weather");
                break;
            case 5:
                displayLevelOption();
                break;
            default:
                break;
        }

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    public void CompileLevels()
    {
        LevelsManagerPrefab LMP;
        if ((LMP = SceneAsset.FindObjectOfType<LevelsManagerPrefab>()) != null)
        {
            DestroyImmediate(LMP.gameObject);
        }

        GameObject go = new GameObject();
        go.AddComponent<LevelsManagerPrefab>();
        go.name = "LevelManagerProfile";
        go.GetComponent<LevelsManagerPrefab>().cinematicName = new string[4];
        go.GetComponent<LevelsManagerPrefab>().cinematicName[0] = "SceneGame";
        go.GetComponent<LevelsManagerPrefab>().cinematicName[1] = "SceneCutsceneScaffold";
        go.GetComponent<LevelsManagerPrefab>().cinematicName[2] = "SceneCutsceneSmallIsland";
        go.GetComponent<LevelsManagerPrefab>().cinematicName[3] = "SceneCutsceneTrain";
        levels[0].isLocked = false;
        for (int i = 0; i < levels.Count; i++)
        {
            levels[i].loadMe(PathLevelFolder);
        }
        go.GetComponent<LevelsManagerPrefab>().setLevelList(levels);
        var prefab = PrefabUtility.CreatePrefab("Assets/@Prefabs/Levels/LevelManagerProfilePrefab.prefab", go);
        int j = -1;
        while (++j < go.GetComponent<LevelsManagerPrefab>().LevelList.Count)
            prefab.GetComponent<LevelsManagerPrefab>().LevelList[j].renameMe("Level " + (j + 1));
        PrefabUtility.ConnectGameObjectToPrefab(go, prefab);
    }

    public void TakeAScreen()
    {
        Debug.Log("Level" + (idxForScreenshots + 1));
        currentLevelIdx = idxForScreenshots;
        clickOnLevelButton(idxForScreenshots);
        Application.CaptureScreenshot("LevelsScreenshots/level " + (idxForScreenshots + 1) + ".png");
        ++idxForScreenshots;
        if (idxForScreenshots == levels.Count)
            idxForScreenshots = -1;
    }

    public void displayObjButons(enumPrefabType _type, string _label)
    {
        List<prefabNFO> lst = prefabsList.Select(t => t.Value).Where(d => d.type == _type).ToList<prefabNFO>();

        bool canAdd = true;
        if (_type == enumPrefabType.Cage)
        {
            List<GameObject> lstCage = SceneAsset.FindObjectsOfType<BBCage>().ToList<BBCage>().Select(t => t.gameObject).ToList<GameObject>();
            canAdd = (lstCage.Count < 3);
            nbCageInScene = lstCage.Count;
        }

        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.gray + Color.blue;
        for (int i = 0; i < lst.Count; i++)
        {
            if (lst[i].texture == null)
            {
                lst[i].texture = AssetPreview.GetAssetPreview(AssetDatabase.LoadAssetAtPath("Assets" + lst[i].path.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject)));
            }
            GUILayout.BeginVertical(GUILayout.Width(50));
            if (GUILayout.Button(lst[i].texture, new GUILayoutOption[] { GUILayout.Height(50), GUILayout.Width(50) }) && canAdd)
            {
                if (isPhysicTesing)
                {
                    isPhysicTesing = false;
                    loadTmpLevel();
                    SceneBuildingManagerPrefab.Instance.TheyWillDontMove();
                }
                if (_type == enumPrefabType.World)
                {
                    addUniquePrefabInScene(lst[i], enumPrefabType.World);
                }
                else if (_type == enumPrefabType.Weather)
                {
                    addUniquePrefabInScene(lst[i], enumPrefabType.Weather);
                }
                else
                {
                    addPrefabInScene(lst[i]);
                }
            }
            GUILayout.Label(lst[i].name, labelButton);
            GUILayout.EndVertical();
            if (i % 10 == 9)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        GUILayout.EndHorizontal();
    }

    public void displayLevelOption()
    {
        GUI.backgroundColor = Color.gray + Color.blue;
        int aTmpInt = 0;

        GUILayout.BeginVertical();
        GUILayout.Label("Options\n\n");

        GUILayout.BeginHorizontal();
        GUILayout.Label("Tuto (*)", EditorStyles.boldLabel, GUILayout.Width(100));
        if (currentProfile.nbTuto == 0)
        {
            GUILayout.Label("NULL", GUILayout.Width(40));
        }
        else
        {
            GUILayout.Label(currentProfile.nbTuto.ToString(), GUILayout.Width(40));
        }
        aTmpInt = (int)GUILayout.HorizontalSlider(currentProfile.nbTuto, 0, 8, GUILayout.Width(100));
        if (aTmpInt != currentProfile.nbTuto)
        {
            isSceneEdited = true;
            currentProfile.nbTuto = aTmpInt;
        }
        GUILayout.Label(((ShopManager.EBonusType)(aTmpInt - 1)).ToString(), GUILayout.Width(75));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cinematic (#)", EditorStyles.boldLabel, GUILayout.Width(100));
        if (currentProfile.nbCinematic == 0)
        {
            GUILayout.Label("NULL", GUILayout.Width(40));
        }
        else
        {
            GUILayout.Label(currentProfile.nbCinematic.ToString(), GUILayout.Width(40));
        }
        aTmpInt = (int)GUILayout.HorizontalSlider(currentProfile.nbCinematic, 0, 3, GUILayout.Width(100));
        if (aTmpInt != currentProfile.nbCinematic)
        {
            isSceneEdited = true;
            currentProfile.nbCinematic = aTmpInt;
        }
        if (currentProfile.nbTuto != 0 && currentProfile.nbCinematic != 0)
        {
            Debug.Log("Please, don't put a tuto and a cinematic on the same level.");
            currentProfile.nbCinematic = 0;
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    public void addPrefabInScene(prefabNFO prefab)
    {
        Object p = AssetDatabase.LoadAssetAtPath("Assets" + prefab.path.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject));
        GameObject go = (GameObject)SceneAsset.Instantiate(p);
        RepereScene repere = SceneAsset.FindObjectOfType<RepereScene>();
        go.transform.position = new Vector3(repere.transform.position.x, repere.transform.position.y, repere.transform.position.z);
        go.name = prefab.name;
        go.transform.hasChanged = false;
        isSceneEdited = true;
        Selection.activeGameObject = go;

        if (SceneBuildingManagerPrefab.Instance != null)
        {
            foreach (Rigidbody r in go.GetComponentsInChildren<Rigidbody>())
            {
                r.isKinematic = true;
            }
            foreach (Collider c in go.GetComponentsInChildren<Collider>())
            {
                c.isTrigger = true;
            }
        }
    }
    public void addUniquePrefabInScene(prefabNFO prefab, enumPrefabType type)
    {
        // Si un monde/weather existe déjà, on le supprime pour le remplacer par le nouveau
        if (type == enumPrefabType.World)
        {
            BBWorld world = SceneAsset.FindObjectOfType<BBWorld>();
            if (world != null)
            {
                DestroyImmediate(world.gameObject);
            }
        }
        else if (type == enumPrefabType.Weather)
        {
            BBWeather weather = SceneAsset.FindObjectOfType<BBWeather>();
            if (weather != null)
            {
                DestroyImmediate(weather.gameObject);
            }
        }
        else
        {
            return;
        }
        Object p = AssetDatabase.LoadAssetAtPath("Assets" + prefab.path.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject));
        GameObject go = (GameObject)SceneAsset.Instantiate(p);
        go.name = prefab.name;
        go.transform.hasChanged = false;
        isSceneEdited = true;
    }

    public void clickOnLevelButton(int index)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "SceneBuilding")
        {
            EditorUtility.DisplayDialog("Not in SceneBuilding", "Please load SceneBuilding", "OK");
            return;
        }
        levels[index].loadMe(PathLevelFolder);
        currentProfile = levels[index];
        loadCurrentProfile();
        isSceneEdited = false;
        cleanGameObjectHasChanged();
    }

    void cleanGameObjectHasChanged()
    {
        List<GameObject> lst = getAllSceneObjects();
        for (int i = 0; i < lst.Count; i++)
        {
            lst[i].transform.hasChanged = false;
        }
    }

    #region Loads/Saves
    void cleanScene()
    {
        List<GameObject> lst = getAllSceneObjects();

        //List<BBMonster> lst = SceneAsset.FindObjectsOfType<BBMonster>().ToList<BBMonster>();
        for (int i = 0; i < lst.Count; i++)
        {
            DestroyImmediate(lst[i]);
        }
    }
    void loadCurrentProfile()
    {
        cleanScene();
        for (int m = 0; m < currentProfile.sceneObjects.Count; m++)
        {
            if (prefabsList.ContainsKey(currentProfile.sceneObjects[m].name))
            {
                //Debug.Log("/Assets" + prefabsList[currentProfile.sceneObjects[m].name].path.Replace(Application.dataPath.Replace("/", "\\"), ""));
                Object prefab = AssetDatabase.LoadAssetAtPath("Assets" + prefabsList[currentProfile.sceneObjects[m].name].path.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject));
                if (prefab == null)
                {
                    Debug.Log("Prefab Inexistant : " + prefabsList[currentProfile.sceneObjects[m].name]);
                }
                GameObject go = (GameObject)SceneAsset.Instantiate(prefab);
                go.transform.position = currentProfile.sceneObjects[m].position;
                go.transform.rotation = currentProfile.sceneObjects[m].rotation;
                go.transform.localScale = currentProfile.sceneObjects[m].scale;
                go.name = currentProfile.sceneObjects[m].name;
                if (go.GetComponent<BBMonster>() != null)
                {
                    go.GetComponent<BBMonster>().hatType = currentProfile.sceneObjects[m].HatType;
                }
                else if (go.GetComponent<BBWorld>() != null &&
                    UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SceneBuilding")
                {
                    GameObject deathSea = go.GetComponentInChildren<SeaColliderDeath>().gameObject;
                    deathSea.SetActive(false);
                }
                go.transform.hasChanged = false;
            }
        }
        if (SceneBuildingManagerPrefab.Instance != null)
        {
            SceneBuildingManagerPrefab.Instance.TheyWillDontMove();
        }
        isPhysicTesing = false;
    }
    void loadTmpLevel()
    {
        cleanScene();
        for (int m = 0; m < tmpLevel.sceneObjects.Count; m++)
        {
            if (prefabsList.ContainsKey(tmpLevel.sceneObjects[m].name))
            {
                Object prefab = AssetDatabase.LoadAssetAtPath("Assets" + prefabsList[tmpLevel.sceneObjects[m].name].path.Replace(Application.dataPath.Replace("/", "\\"), ""), typeof(GameObject));
                GameObject go = (GameObject)SceneAsset.Instantiate(prefab);
                go.transform.position = tmpLevel.sceneObjects[m].position;
                go.transform.rotation = tmpLevel.sceneObjects[m].rotation;
                go.transform.localScale = tmpLevel.sceneObjects[m].scale;
                go.name = tmpLevel.sceneObjects[m].name;
                if (go.GetComponent<BBMonster>() != null)
                {
                    go.GetComponent<BBMonster>().hatType = tmpLevel.sceneObjects[m].HatType;
                }
                else if (go.GetComponent<BBWorld>() != null &&
                      UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SceneBuilding")
                {
                    GameObject deathSea = go.GetComponentInChildren<SeaColliderDeath>().gameObject;
                    deathSea.SetActive(false);
                }
                //GameObject SCD = FindObjectOfType<SeaColliderDeath>().gameObject;
                //SCD.SetActive(false);

                go.transform.hasChanged = false;
            }
        }
        //if (SceneBuildingManagerPrefab.Instance != null) {
        //    SceneBuildingManagerPrefab.Instance.TheyWillDontMove();
        //}
    }

    List<GameObject> getAllSceneObjects()
    {
        List<GameObject> lst = new List<GameObject>();
        List<GameObject> lstCage = SceneAsset.FindObjectsOfType<BBCage>().ToList<BBCage>().Select(t => t.gameObject).ToList<GameObject>();
        List<GameObject> lstMonsters = SceneAsset.FindObjectsOfType<BBMonster>().ToList<BBMonster>().Select(t => t.gameObject).ToList<GameObject>();
        List<GameObject> lstWorld = SceneAsset.FindObjectsOfType<BBWorld>().ToList<BBWorld>().Select(t => t.gameObject).ToList<GameObject>();
        List<GameObject> lstObject = SceneAsset.FindObjectsOfType<BBObject>().ToList<BBObject>().Select(t => t.gameObject).ToList<GameObject>();
        List<GameObject> lstWeather = SceneAsset.FindObjectsOfType<BBWeather>().ToList<BBWeather>().Select(t => t.gameObject).ToList<GameObject>();

        lst.AddRange(lstCage);
        lst.AddRange(lstMonsters);
        lst.AddRange(lstWorld);
        lst.AddRange(lstObject);
        lst.AddRange(lstWeather);

        return lst;
    }

    void saveCurrentProfile()
    {
        if (currentProfile == null)
        {
            Debug.LogError("CurrentProfile is Null");
        }

        // Level Option

        // Scene Objects
        currentProfile.sceneObjects.Clear();
        List<GameObject> lst = getAllSceneObjects();

        for (int m = 0; m < lst.Count; m++)
        {
            BBMonster.enumHatType hatType = BBMonster.enumHatType.None;
            if (lst[m].GetComponent<BBMonster>() != null)
            {
                hatType = lst[m].GetComponent<BBMonster>().hatType;

                Transform body = lst[m].transform.FindChild("Body");
                lst[m].transform.Translate(body.localPosition);
                body.position.Set(0, 0, 0);
            }
            string[] splitedName = lst[m].name.Split(' ');
            if (splitedName.Length > 1)
            {
                if (splitedName[1][0] == '(' && splitedName[1].Last<char>() == ')')
                {
                    Debug.Log(lst[m].name + " was renamed " + splitedName[0]);
                    lst[m].name = splitedName[0];
                }
            }

            currentProfile.sceneObjects.Add(new SceneObject(
                lst[m].name,
                new Vector3(
                    lst[m].transform.position.x,
                    lst[m].transform.position.y,
                    lst[m].transform.position.z
                ),
                new Quaternion(
                    lst[m].transform.rotation.x,
                    lst[m].transform.rotation.y,
                    lst[m].transform.rotation.z,
                    lst[m].transform.rotation.w
                ),
                new Vector3(
                    lst[m].transform.localScale.x,
                    lst[m].transform.localScale.y,
                    lst[m].transform.localScale.z
                    ),
                hatType
                ));
        }

        currentProfile.saveMe(PathLevelFolder);
    }
    void saveTmpLevel()
    {
        if (tmpLevel == null)
        {
            Debug.LogError("CurrentProfile is Null");
        }
        tmpLevel.sceneObjects.Clear();

        List<GameObject> lst = getAllSceneObjects();

        for (int m = 0; m < lst.Count; m++)
        {
            BBMonster.enumHatType hatType = BBMonster.enumHatType.None;
            if (lst[m].GetComponent<BBMonster>() != null)
            {
                hatType = lst[m].GetComponent<BBMonster>().hatType;
            }
            tmpLevel.sceneObjects.Add(new SceneObject(
                lst[m].name,
                new Vector3(
                    lst[m].transform.position.x,
                    lst[m].transform.position.y,
                    lst[m].transform.position.z
                ),
                new Quaternion(
                    lst[m].transform.rotation.x,
                    lst[m].transform.rotation.y,
                    lst[m].transform.rotation.z,
                    lst[m].transform.rotation.w
                ),
                new Vector3(
                    lst[m].transform.localScale.x,
                    lst[m].transform.localScale.y,
                    lst[m].transform.localScale.z
                    ),
                hatType
                ));
        }

        tmpLevel.saveMe(PathLevelFolder);
    }
    void loadLevelsList()
    {
        string file;
        levelsList = new List<string>();
        levels = new List<LevelProfile>();

        if (System.IO.File.Exists(PathLevelsListTxt))
        {
            file = System.IO.File.ReadAllText(PathLevelsListTxt);
            string[] lines = file.Split("\n".ToCharArray());
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i] != "")
                {
                    levelsList.Add(lines[i]);
                    levels.Add(new LevelProfile(lines[i]));
                    if (levels[i].levelName == "")
                    {
                        levels[i].loadMe(PathLevelFolder);
                    }
                }
            }
        }
    }
    void saveLevelsList(bool _forceRefresh = true)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < levelsList.Count; i++)
        {
            if (i > 0)
            {
                sb.Append("\n");
            }
            sb.Append(levelsList[i]);
        }
        System.IO.File.WriteAllText(PathLevelsListTxt, sb.ToString());
        if (_forceRefresh)
        {
            AssetDatabase.Refresh();
        }
    }
    #endregion

    public static string int2String0000(int num)
    {
        return (num.ToString("d4"));
    }

    public enum enumPrefabType
    {
        Cage,
        World,
        Monster,
        Obj,
        Other,
        Weather
    }

    /// <summary>
    /// Permet de sauvegarder chaque prefab du jeu qui sont présent dans les levels (monstres, map, object, cage et weather)
    /// </summary>
    public class prefabNFO
    {
        public string name;
        public string path;
        public Texture2D texture;
        public enumPrefabType type;

        public prefabNFO(string _name, string _path, enumPrefabType _type)
        {
            name = _name;
            path = _path;
            type = _type;
        }
        public prefabNFO(string _name, string _path, enumPrefabType _type, Texture2D _texture)
        {
            name = _name;
            path = _path;
            type = _type;
            texture = _texture;
        }
    }
}
*/
