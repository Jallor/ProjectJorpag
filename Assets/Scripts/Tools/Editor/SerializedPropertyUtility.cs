using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

public static class SerializedPropertyUtility
{
    public static object GetValue(this SerializedProperty property, out Type fieldType)
    {
        fieldType = null;
        if (property == null)
            return null;

        string path = property.propertyPath.Replace(".Array.data[", "[");
        string[] elements = path.Split('.');
        object targetObject = property.serializedObject.targetObject;

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].EndsWith("]"))
            {
                string elementName = elements[i].Substring(0, elements[i].IndexOf("["));
                string indexString = elements[i].Substring(elements[i].IndexOf("[")).Replace("[", "").Replace("]", "");
                int index = System.Convert.ToInt32(indexString);

                targetObject = GetFieldValueAtIndex(targetObject, elementName, index, out fieldType);
            }
            else
            {
                targetObject = GetFieldValue(targetObject, elements[i], out fieldType);
            }
        }

        return targetObject;
    }

    private static object GetFieldValue(object source, string name, out Type fieldType)
    {
        fieldType = null;
        if (source == null)
            return null;

        Type type = source.GetType();

        while (type != null)
        {
            FieldInfo fieldInfo = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                fieldType = fieldInfo.FieldType;
                return fieldInfo.GetValue(source);
            }

            type = type.BaseType;
        }

        return null;
    }

    private static object GetFieldValueAtIndex(object source, string name, int index, out Type fieldType)
    {
        fieldType = null;

        Type type;
        object obj = GetFieldValue(source, name, out type);
        IEnumerable enumerable = obj as IEnumerable;

        if (enumerable == null) return null;
        IEnumerator enumerator = enumerable.GetEnumerator();

        for (int i = 0; i <= index; i++)
        {
            if (!enumerator.MoveNext()) return null;
        }

        if (type.IsArray)
        {
            fieldType = type.GetElementType();
        }
        else
        {
            Debug.Assert(type.IsGenericType && typeof(List<>).IsAssignableFrom(type.GetGenericTypeDefinition()));
            fieldType = type.GetGenericArguments()[0];
        }

        return enumerator.Current;
    }
}