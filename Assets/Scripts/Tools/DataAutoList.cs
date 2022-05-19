using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using NaughtyAttributes;

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

    [Button("Refresh Auto List")]
    public virtual void RefreshDataList()
    {
#if UNITY_EDITOR
        string[] allDataGUID = AssetDatabase.FindAssets("t:" + typeof(dataType).ToString());

        List<dataType> newList = new List<dataType>();

        foreach (string cardInProject in allDataGUID)
        {
            dataType asset = AssetDatabase.LoadAssetAtPath<dataType>(AssetDatabase.GUIDToAssetPath(cardInProject));

            newList.Add(asset);
        }

        bool updateList = false;

        if (newList.Count == DataAutoList<dataType, selfType>.Inst.GetDataList().Count)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                if (!DataAutoList<dataType, selfType>.Inst.GetDataList().Contains(newList[i]))
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
            DataAutoList<dataType, selfType>.Inst.SetDataList(newList);
            EditorUtility.SetDirty(DataAutoList<dataType, selfType>.Inst);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}