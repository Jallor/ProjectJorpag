using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextBoxManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _CharaNameText;
    [SerializeField] private TextMeshProUGUI _TextBlock;

    private bool _CurrentlyDisplayingText = false;
    private Queue<string> _RemainingTextToDisplay = new Queue<string>();

    public void Awake()
    {
        Close();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetMouseButtonDown(0))
        {
            if (_RemainingTextToDisplay.Count > 0)
            {
                DisplayNextText();
            }
            else
            {
                Close();
            }
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        _CurrentlyDisplayingText = false;
        gameObject.SetActive(false);
    }

    public void QueueTextToDisplay(string textToDisplay)
    {
        _RemainingTextToDisplay.Enqueue(textToDisplay);
        if (!_CurrentlyDisplayingText)
        {
            DisplayNextText();
        }
    }

    private void DisplayNextText()
    {
        _CharaNameText.gameObject.SetActive(false);
        _TextBlock.text = _RemainingTextToDisplay.Dequeue();
        _CurrentlyDisplayingText = true;
    }

    public bool IsOpen()
    {
        return (gameObject.activeSelf);
    }
}
