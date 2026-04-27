using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelParent : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        _CurrentlyDisplayingText = false;
        gameObject.SetActive(false);
    }

    public bool IsOpen()
    {
        return (gameObject.activeSelf);
    }

}
