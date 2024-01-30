using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private CharacterManager _Owner = null;

    [SerializeField] private List<OnEnterAreaActionTrigger> _OnEnterActionTrigger = new List<OnEnterAreaActionTrigger>();
    [SerializeField] private List<OnExitAreaActionTrigger> _OnExitActionTrigger = new List<OnExitAreaActionTrigger>();
    
    // TODO filter 

    public void OnTriggerEnter2D(Collider2D collision)
    {
        GameContext context = new GameContext();
        context.Owner = new CharacterVarWrapper(_Owner);
        context.Caster = context.Owner;
        if (collision.GetComponent<CharacterManager>())
        {
            context.Target = new CharacterVarWrapper(collision.GetComponent<CharacterManager>());
        }

        Debug.LogWarning("TODO ! : ici y a un gros chantier � fairte � propos de comment get un type d'obj");
        // TODO ! Salut ! ICI il faut trouver un moyen de get un varWrapper en fonction du type d'obj touch�
        // pour l'instant on r�cup�re juste le character, mais il faudrait pouvoir r�cup�rer n'imp'

        foreach (OnEnterAreaActionTrigger actionTrigger in _OnEnterActionTrigger)
        {
            actionTrigger.TriggerAction(context);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        // TODO ici aussi 

        GameContext context = new GameContext();
        context.Owner = new CharacterVarWrapper(_Owner);
        context.Caster = context.Owner;
        if (collision.GetComponent<CharacterManager>())
        {
            context.Target = new CharacterVarWrapper(collision.GetComponent<CharacterManager>());
        }

        foreach (OnExitAreaActionTrigger actionTrigger in _OnExitActionTrigger)
        {
            actionTrigger.TriggerAction(context);
        }
    }

    public void OverrideOwner(CharacterManager newOwner)
    {
        _Owner = newOwner;
    }
}
