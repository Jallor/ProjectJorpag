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
    public string filePathContaningEnumGenerated;

    public bool HasDefaulValue = false;
    public EnumValue defaultValue = new EnumValue { enumValueName  = "DEFAULT", uniqueID = -1, resetIDToHash = false };

    public List<EnumDyna> enumList;


    [Serializable]
    public class EnumDyna
    {
        public string enumName;
        public List<EnumValue> enumValueList;
    }

    [Serializable]
    public class EnumValue
    {
        public string enumValueName;
        [NaughtyAttributes.ReadOnly]
        public int uniqueID;

        public bool resetIDToHash = true;
    }

    [Button]
    void UpdateEnum()
    {
        UpdateEnumHash();

        if (VerifyUniqueIdCollision())
        {
            UpdateEnumFile();
        }
            
    }

    void UpdateEnumHash()
    {
        foreach (EnumDyna enumDyna in enumList)
        {
            foreach (EnumValue enumValue in enumDyna.enumValueList)
            {
                if (enumValue.resetIDToHash)
                {
                    enumValue.uniqueID = enumValue.enumValueName.GetHashCode();
                    enumValue.resetIDToHash = false;
                }
            }
        }
    }


    bool VerifyUniqueIdCollision()
    {
        HashSet<int> AllHashcode = new HashSet<int>();

        foreach (EnumDyna enumDyna in enumList)
        {
            foreach (EnumValue enumValue in enumDyna.enumValueList)
            {
                if (AllHashcode.Contains(enumValue.uniqueID) || (HasDefaulValue && enumValue.uniqueID == defaultValue.uniqueID) )
                {
                    Debug.LogError("hash duplicate, change name of " + enumValue.enumValueName + "in enum list " + enumDyna.enumName);
                    return false;
                }

                AllHashcode.Add(enumValue.uniqueID);
            }

            AllHashcode.Clear();
        }

        return true;
    }

    void UpdateEnumFile()
    {
        Debug.Assert(!String.IsNullOrEmpty(filePathContaningEnumGenerated), "need a path for generated enum");

        StringBuilder stringBuilder = new System.Text.StringBuilder();

        stringBuilder.AppendLine("//file generated from " + AssetDatabase.GetAssetPath(this));
        stringBuilder.AppendLine("//Don't modify this file");

        foreach (EnumDyna enumDyna in enumList)
        {
            stringBuilder.AppendLine("public enum " + enumDyna.enumName);
            stringBuilder.AppendLine("{");

            if (HasDefaulValue)
            {
                stringBuilder.AppendLine("	" + defaultValue.enumValueName + " = " + defaultValue.uniqueID.ToString() + ",");
            }

            foreach (EnumValue enumValue in enumDyna.enumValueList)
            {
                stringBuilder.AppendLine("	" + enumValue.enumValueName + " = " + enumValue.uniqueID.ToString() + ",");
            }

            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
        }

        string ScriptFilePath = Application.dataPath + "/" + filePathContaningEnumGenerated;
        System.IO.File.WriteAllText(ScriptFilePath, stringBuilder.ToString(), System.Text.Encoding.UTF8);
        AssetDatabase.ImportAsset("Assets/" + filePathContaningEnumGenerated);

        AssetDatabase.Refresh();
    }

    /*
    [CustomPropertyDrawer(typeof(EnumValue))]
    public class EnumValueDrawerUIE : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Create property container element.
            var container = new VisualElement();

            // Create property fields.
            var amountField = new PropertyField(property.FindPropertyRelative("enumValueName"), "Value");
            var unitField = new PropertyField(property.FindPropertyRelative("uniqueID"), "ResetIdToHash");
            var nameField = new PropertyField(property.FindPropertyRelative("resetIDToHash"), "ResetIdToHash");

            // Add fields to the container.
            container.Add(amountField);
            container.Add(unitField);
            container.Add(nameField);

            return container;
        }
    }*/
}
