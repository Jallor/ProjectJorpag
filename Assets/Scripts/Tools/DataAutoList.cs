using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public interface DataList<dataType>
{
    public abstract List<dataType> GetDataList();
    public abstract void SetDataList(List<dataType> newList);
}

public class DataAutoList<dataType, selfType> : SingletonScriptable<selfType>, DataList<dataType>
    where selfType : ScriptableObject, DataList<dataType>
    where dataType : UnityEngine.Object
{
    public List<dataType> dataList = new List<dataType>();

    public List<dataType> GetDataList()
    {
        return dataList;
    }

    public void SetDataList(List<dataType> newList)
    {
        dataList = newList;
    }

    public void RefreshDataList()
    {
#if UNITY_EDITOR
        string[] allDataGUID = UnityEditor.AssetDatabase.FindAssets("t:" + typeof(dataType).ToString());

        List<dataType> newList = new List<dataType>();

        foreach (string cardInProject in allDataGUID)
        {
            dataType asset = UnityEditor.AssetDatabase.LoadAssetAtPath<dataType>(UnityEditor.AssetDatabase.GUIDToAssetPath(cardInProject));

            newList.Add(asset);
        }

        bool updateList = false;

        if (newList.Count == DataAutoList < dataType, selfType >.Instance.GetDataList().Count)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                if (!DataAutoList<dataType, selfType>.Instance.GetDataList().Contains(newList[i]))
                {
                    updateList = true;
                    break;
                }
            }
        }
        else
        {
            updateList = true;
        }

        if (updateList)
        {
            DataAutoList<dataType, selfType>.Instance.SetDataList(newList);
            EditorUtility.SetDirty(DataAutoList<dataType, selfType>.Instance);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}