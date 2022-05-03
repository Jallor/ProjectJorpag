using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SelectImplementationNameAttribute : PropertyAttribute
{
    public string Name { get; private set; }

    public SelectImplementationNameAttribute(string name)
    {
        this.Name = name;
    }
}