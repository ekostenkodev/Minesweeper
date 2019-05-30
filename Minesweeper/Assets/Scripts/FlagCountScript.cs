using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagCountScript : MonoBehaviour
{
    [SerializeField]
    private GameSystem _gameSystem;
    
    private int _flagCount;
    
    void Awake()
    {
        _gameSystem.ClickEvent += changeFlagCount;
        _gameSystem.GameStartEvent += resetCount;

        resetCount();

    }

    private void resetCount()
    {
        _flagCount = _gameSystem.FlagCount;
        GetComponent<TextMeshProUGUI>().text = _flagCount.ToString("000"); 
    }

    private void changeFlagCount(TileComponent tile,bool flag)
    {
        if(!flag)
            return;
        if(tile.Tile.Flag)
            _flagCount++;
        else
            _flagCount--;

        GetComponent<TextMeshProUGUI>().text = _flagCount.ToString("000"); 
    }
}
