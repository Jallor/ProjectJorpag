using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SingletonScriptable<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance = null;
    public static T Inst
    {
        get
        {
            if (!_instance)
            {
                Resources.LoadAll("DataList", typeof(T));
                T[] findObjects = Resources.FindObjectsOfTypeAll<T>();
                Debug.Assert(findObjects.Length > 0,
                    "No SingletonScriptable of type " + typeof(T).ToString());
                Debug.Assert(findObjects.Length <= 1,
                    "Too much SingletonScriptable of type " + typeof(T).ToString());

                if (findObjects.Length > 0)
                {
                    _instance = findObjects[0];
                }
            }

            return _instance;
        }
    }
}