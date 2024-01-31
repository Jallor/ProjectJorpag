using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruct : MonoBehaviour
{
    [SerializeField] private float _TimeRemainingBeforeDestruct = 5f;

    void Start()
    {
        Destroy(gameObject, _TimeRemainingBeforeDestruct);
    }
}
