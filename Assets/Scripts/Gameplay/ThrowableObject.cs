using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private float _Speed = 1;
    [SerializeField] private Vector2 _Direction = new Vector2(0, -1);

    private CharacterManager _Owner;
    [Required][SerializeField] private TriggerArea _TriggerArea = null;

    void Start()
    {
        if (!_TriggerArea)
        {
            if (GetComponent<TriggerArea>())
            {
                _TriggerArea = GetComponent<TriggerArea>();
            }
        }
    }

    void Update()
    {
        Vector3 currentMove = _Direction * _Speed * Time.deltaTime;

        transform.position += currentMove;
    }

    public void SetSpeed(float speed) { _Speed = speed; }
    public void SetDirection(Vector2 direction) { _Direction = direction;}
    public void SetOwner(CharacterManager newOwner) { _Owner = newOwner; }
}
