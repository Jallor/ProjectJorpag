using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotator : MonoBehaviour
{
    [SerializeField] private float _RotationSpeed = -90f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(0, 0, _RotationSpeed * Time.deltaTime);
    }
}
