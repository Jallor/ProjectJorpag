using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterVfxManager : MonoBehaviour
{
    private CharacterManager _CharacterManager;

    [Foldout("Key Positions")]
    [Required][SerializeField] private Transform _VfxPosition;

    private Dictionary<int, GameObject> _InstantiedVfx = new Dictionary<int, GameObject>();

    public void Initialize(CharacterManager characterManager)
    {
        _CharacterManager = characterManager;

    }

    // return the Id of this Vfx
    public int AddStatusVfx(EStatusVfx statusVfxToAdd)
    {
        GameObject prefabVfx = ListsManager.Inst.GetPrefabFromStatusVfxType(statusVfxToAdd);
        GameObject newStatusVfx = Instantiate(prefabVfx);

        newStatusVfx.transform.position = _VfxPosition.position;

        int newVfxId = Random.Range(0, int.MaxValue);
        while (_InstantiedVfx.Keys.Contains(newVfxId))
        {
            newVfxId = Random.Range(0, int.MaxValue);
        }

        _InstantiedVfx[newVfxId] = newStatusVfx;

        return (newVfxId);
    }

    public bool RemoveVfx(int idToRemove)
    {
        if (!_InstantiedVfx.Keys.Contains(idToRemove))
        {
            return false;
        }

        Destroy(_InstantiedVfx[idToRemove]);
        _InstantiedVfx.Remove(idToRemove);

        return true;
    }
}
