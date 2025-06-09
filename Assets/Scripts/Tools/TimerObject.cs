using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using System;

public class TimerObject : MonoBehaviour
{
    public enum ETextDisplayMode
    {
        PERCENT_INT,
        PERCENT_FLOAT,
        REMAIN_INT,
        REMAIN_FLOAT,
        PASSED_INT,
        PASSED_FLOAT,
    } 

    public SimpleDelegate OnTargetTimeReached = null;

    [SerializeField] private Image _ProgressionImage = null;
    [SerializeField] private Gradient _ProgressionGradient;
    [SerializeField] private bool _FillBarOverTime = false;
    [SerializeField] private TextMeshProUGUI _ProgressionText = null;
    [SerializeField] private ETextDisplayMode _TextDisplayMode = ETextDisplayMode.PERCENT_INT;

    private bool _IsRunning = false;
    private float _CurrentTimePassed = 0;
    [SerializeField] private float _TargetTime = 2;

    // delegate à certains %

    private void Start()
    {

    }

    private void Update()
    {
        if (_ProgressionImage)
        {
            float currentProgress = GetCurrentProgression();
            if (_FillBarOverTime)
            {
                _ProgressionImage.fillAmount = currentProgress;
            }
            else
            {
                _ProgressionImage.fillAmount = 1 - currentProgress;
            }
            _ProgressionImage.color = _ProgressionGradient.Evaluate(currentProgress);
        }
        if (_ProgressionText)
        {
            string text = "";
            switch ( _TextDisplayMode )
            {
                case ETextDisplayMode.PERCENT_INT:
                    text = Math.Round(GetCurrentProgression() * 100, 0) + "%";
                    break;
                case ETextDisplayMode.PERCENT_FLOAT:
                    text = Math.Round(GetCurrentProgression() * 100, 2).ToString("0.00") + "%";
                    break;
                case ETextDisplayMode.REMAIN_INT:
                    text = Math.Round(_TargetTime - _CurrentTimePassed, 0).ToString();
                    break;
                case ETextDisplayMode.REMAIN_FLOAT:
                    text = Math.Round(_TargetTime - _CurrentTimePassed, 2).ToString("0.00");
                    break;
                case ETextDisplayMode.PASSED_INT:
                    text = Math.Round(_CurrentTimePassed, 0).ToString();
                    break;
                case ETextDisplayMode.PASSED_FLOAT:
                    text = Math.Round(_CurrentTimePassed, 2).ToString("0.00");
                    break;
            }
            _ProgressionText.text = text;
        }

        if (!_IsRunning)
        {
            return;
        }

        _CurrentTimePassed += Time.deltaTime;

        if (_CurrentTimePassed >= _TargetTime)
        {
            _IsRunning = false;
            OnTargetTimeReached?.Invoke();
        }
    }

    [Button]
    public void StartTimer()
    {
        _IsRunning = true;
    }

    [Button]
    public void PauseTimer()
    {
        _IsRunning = false;
    }

    [Button]
    public void ResumeTimer()
    {
        _IsRunning = true;
    }

    [Button]
    public void RestTimeToZero()
    {
        _CurrentTimePassed = 0;
    }

    public float GetCurrentTimePassed() => _CurrentTimePassed;
    public float GetCurrentProgression() => _CurrentTimePassed / _TargetTime;

}