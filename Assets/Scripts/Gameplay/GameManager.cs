using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonScriptable<GameManager>
{

    private CharacterManager _Player = null;

    public CharacterManager GetPlayer()
    {
        if (_Player == null)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                _Player = playerController.GetCharaManager(); ;
            }
        }
        return (_Player);
    }
}
