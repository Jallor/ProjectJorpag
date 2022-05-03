using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(SelectImplementationAttribute))]
public class SelectImplementationDrawer : PropertyDrawer
{
    private Type[] _cachedImplementations;
    private int _implementationIndex;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        bool isArrayElement;

        {
            // In order to know if the element is in an array, we try to get the first subelement property name.
            // If there is no subelement, it will return the current property name (for which isArray will be false)
            // Otherwise, it'll return the first element and isArray will be true
            string propertyName = property.propertyPath.Split('.')[0];
            SerializedProperty newProp = property.serializedObject.FindProperty(propertyName);

            isArrayElement = (newProp.isArray);
        }

        const int horizontalPropertySpacing = 2;
        Rect popupRect = new Rect(position.x + EditorGUIUtility.labelWidth + horizontalPropertySpacing, position.y, position.width - EditorGUIUtility.labelWidth - horizontalPropertySpacing, EditorGUIUtility.singleLineHeight);

        if (isArrayElement)
        {
            popupRect.x -= 20;
            popupRect.width += 20;
        }

        Type fieldType;
        object fieldValue = property.GetValue(out fieldType);

        // For some reason the fieldType can be null a very short time. We can't continue safely with a null fieldType so we early out here.
        if (fieldType == null)
            return;

        if (_cachedImplementations == null)
        {
            _cachedImplementations = GetImplementations(fieldType)
                .Where(impl => !impl.IsSubclassOf(typeof(UnityEngine.Object))).ToArray();
        }

        if (fieldValue != null)
            _implementationIndex = Array.IndexOf(_cachedImplementations, fieldValue.GetType());

        EditorGUI.BeginChangeCheck();
        {
            _implementationIndex = EditorGUI.Popup(popupRect, fieldValue == null ? -1 : _implementationIndex, _cachedImplementations.Select(impl => GetImplementationName(impl)).ToArray());
        }
        if (EditorGUI.EndChangeCheck())
        {
            property.managedReferenceValue = Activator.CreateInstance(_cachedImplementations[_implementationIndex]);
            property.isExpanded = true;
        }

        string displayName = property.displayName;

        EditorGUI.PropertyField(position, property, new GUIContent(displayName), true);
    }

    public static Type[] GetImplementations(Type interfaceType)
    {
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());

        List<Type> typeList = types.Where(p => interfaceType.IsAssignableFrom(p) && !p.IsAbstract).ToList();
        typeList.Sort(TypeComparison);

        return typeList.ToArray();
    }

    private static int TypeComparison(Type a, Type b)
    {
        string aName = GetImplementationName(a);
        string bName = GetImplementationName(b);

        int result = GetSlashCount(aName).CompareTo(GetSlashCount(bName));
        if (result != 0)
        {
            return -result;
        }

        return aName.CompareTo(bName);
    }

    private static int GetSlashCount(string str)
    {
        int counter = 0;
        foreach (char c in str)
        {
            if (c == '/')
            {
                counter++;
            }
        }

        return counter;
    }

    private static string GetImplementationName(Type implementationType)
    {
        object[] attr = implementationType.GetCustomAttributes(typeof(SelectImplementationNameAttribute), true);

        if (attr != null && attr.Length > 0)
        {
            var nameAttribute = attr[0] as SelectImplementationNameAttribute;
            if (!string.IsNullOrEmpty(nameAttribute.Name))
            {
                return nameAttribute.Name;
            }
        }
        
        return implementationType.FullName;
    }
}