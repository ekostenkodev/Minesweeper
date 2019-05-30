using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public GameSystem GameSystem;
    public int TimeCount{get;set;}
    private TextMeshProUGUI _textComponent;
    IEnumerator timerCoroutine;
    bool isCoroutineActive = false;
    void Awake()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();

        GameSystem.ClickEvent += ActivateTimer;
        GameSystem.GameStartEvent += ClearTimer;
        GameSystem.EndGameEvent += StopTimer;
    }

    private void ActivateTimer(TileComponent tile,bool flag)
    {
        if(isCoroutineActive || flag)
            return;
        timerCoroutine = StartTimer();
        StartCoroutine(timerCoroutine);
        isCoroutineActive = true;

    }
    private void StopTimer(bool result)
    {
        if(timerCoroutine!=null)
            StopCoroutine(timerCoroutine);
        isCoroutineActive = false;
    }

    public void ClearTimer()
    {
        StopTimer(false);
        TimeCount = 0;
        _textComponent.text = TimeCount.ToString("000");
    }
    private IEnumerator StartTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);
            TimeCount++;
            _textComponent.text = TimeCount.ToString("000");
        }
    }
}
