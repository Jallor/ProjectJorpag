using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonobehaviourSingleton<UiManager>
{
    [SerializeField] TextBoxManager _TextBox;

    public bool IsUiBlockingCharaActions()
    {
        if (_TextBox.IsOpen())
        {
            return true;
        }

        return false;
    }

    internal void OpenTextBox(string textToDisplay)
    {
        _TextBox.Open();
        _TextBox.QueueTextToDisplay(textToDisplay);
    }
}
