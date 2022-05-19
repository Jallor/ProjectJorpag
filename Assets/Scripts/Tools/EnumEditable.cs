using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "EditableEnum", menuName = "Data/Enum")]
public class EnumEditable : ScriptableObject
{
    public string FilePathContaningEnumGenerated = "Scripts/Enums/ENewEnum.cs";

    public bool HasDefaulValue = false;
    public EnumValue DefaultValue = new EnumValue { enumValueName  = "NONE", uniqueID = -1, resetIDToHash = false };

    public string EnumName = "ENewEnumName";
    public List<EnumValue> EnumValues;

    [Serializable]
    public class EnumValue
    {
        public string enumValueName;
        [NaughtyAttributes.ReadOnly]
        public int uniqueID;

        public bool resetIDToHash = true;
    }

    [Button]
    public void UpdateEnum()
    {
#if UNITY_EDITOR
        UpdateEnumHash();

        if (VerifyUniqueIdCollision())
        {
            UpdateEnumFile();
        }
#endif            
    }

    void UpdateEnumHash()
    {
#if UNITY_EDITOR
        foreach (EnumValue enumValue in EnumValues)
        {
            if (enumValue.resetIDToHash)
            {
                enumValue.uniqueID = enumValue.enumValueName.GetHashCode();
                enumValue.resetIDToHash = false;
            }
        }
#endif
    }

    bool VerifyUniqueIdCollision()
    {
        HashSet<int> AllHashcode = new HashSet<int>();

        foreach (EnumValue enumValue in EnumValues)
        {
            if (AllHashcode.Contains(enumValue.uniqueID) || (HasDefaulValue && enumValue.uniqueID == DefaultValue.uniqueID))
            {
                Debug.LogError("hash duplicate, change name of " + enumValue.enumValueName);
                return false;
            }

            AllHashcode.Add(enumValue.uniqueID);
        }

        AllHashcode.Clear();

        return true;
    }

    void UpdateEnumFile()
    {
#if UNITY_EDITOR
        Debug.Assert(!String.IsNullOrEmpty(FilePathContaningEnumGenerated), "need a path for generated enum");

        StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine("//file generated from " + AssetDatabase.GetAssetPath(this));
        stringBuilder.AppendLine("//Don't modify this file");

        stringBuilder.AppendLine("public enum " + EnumName);
        stringBuilder.AppendLine("{");

        if (HasDefaulValue)
        {
            stringBuilder.AppendLine("	" + DefaultValue.enumValueName + " = " + DefaultValue.uniqueID.ToString() + ",");
        }

        foreach (EnumValue enumValue in EnumValues)
        {
            stringBuilder.AppendLine("	" + enumValue.enumValueName + " = " + enumValue.uniqueID.ToString() + ",");
        }

        stringBuilder.AppendLine("}");
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();

        string ScriptFilePath = Application.dataPath + "/" + FilePathContaningEnumGenerated;
        System.IO.File.WriteAllText(ScriptFilePath, stringBuilder.ToString(), System.Text.Encoding.UTF8);
        AssetDatabase.ImportAsset("Assets/" + FilePathContaningEnumGenerated);

        AssetDatabase.Refresh();
#endif
    }
}
