using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private List<OnEnterAreaActionTrigger> _OnEnterActionTrigger = new List<OnEnterAreaActionTrigger>();
    [SerializeField] private List<OnExitAreaActionTrigger> _OnExitActionTrigger = new List<OnExitAreaActionTrigger>();
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (OnEnterAreaActionTrigger actionTrigger in _OnEnterActionTrigger)
        {
            actionTrigger.TriggerAction();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        foreach (OnExitAreaActionTrigger actionTrigger in _OnExitActionTrigger)
        {
            actionTrigger.TriggerAction();
        }
    }
}
