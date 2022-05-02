using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour
{
    [SerializeField] private string _PoolName = "";
    [SerializeField] private GameObject _PoolPrefab = null;
    [SerializeField] private int _MinimumNbOfObjects = 1;

    private Stack<GameObject> _availableObjects = new Stack<GameObject>();
    private List<GameObject> _usedObjects = new List<GameObject>();

    private void Awake()
    {
        Debug.Assert(_PoolPrefab);

        for (int i = 0; i < _MinimumNbOfObjects; i++)
        {
            InstantiateNewObject();
        }
    }

    private void InstantiateNewObject()
    {
        GameObject newObj = Instantiate(_PoolPrefab);
        newObj.SetActive(false);
        _availableObjects.Push(newObj);
    }

    public GameObject GetObject()
    {
        if (_availableObjects.Count <= 0)
        {
            InstantiateNewObject();
        }

        GameObject obj = _availableObjects.Pop();
        _usedObjects.Add(obj);
        obj.SetActive(true);
        return (obj);
    }

    public void FreeObject(GameObject obj)
    {
        _usedObjects.Remove(obj);
        obj.SetActive(false);
        _availableObjects.Push(obj);
    }
}
