using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : Un script dégeu pour gérer des listes
public class ListsManager : MonobehaviourSingleton<ListsManager>
{
    [SerializeField] private List<PairStatusVfx> _AllStatusVfx = new List<PairStatusVfx>();

    [System.Serializable]
    public class PairStatusVfx
    {
        public EStatusVfx StatusVFXType;
        public GameObject StatusVFXObject;
    }

    public GameObject GetPrefabFromStatusVfxType(EStatusVfx statusVfxType)
    {
        foreach (PairStatusVfx pair in _AllStatusVfx)
        {
            if (pair.StatusVFXType == statusVfxType)
            {
                return (pair.StatusVFXObject);
            }
        }

        print("!!! VFX not found !!!");
        return (null);
    }
}
