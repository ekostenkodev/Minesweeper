using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScript : MonoBehaviour
{
    [SerializeField]
    private GameSystem _gameSystem;
    [SerializeField]
    private TimerScript _timer;
    [Space]
    [SerializeField]
    private GameObject _winPanel;
    [SerializeField]
    private GameObject _losePanel;
    [SerializeField]
    void Start()
    {
        _gameSystem.EndGameEvent += ShowResult;
    }

    private void ShowResult(bool result)
    {
        if(result)
        {
            int size = _gameSystem.Size;

            int currentTime = _timer.TimeCount;
            int bestTime = PlayerPrefs.GetInt($"BestTime_{size}", currentTime);
            if(currentTime<=bestTime)
            {
                PlayerPrefs.SetInt($"BestTime_{size}", currentTime);
                bestTime = currentTime;
                
            }

            _winPanel.transform.Find("CurrentTime").GetComponent<TextMeshProUGUI>().text = currentTime.ToString("000");
            _winPanel.transform.Find("BestTime").GetComponent<TextMeshProUGUI>().text = bestTime.ToString("000");
            _winPanel.SetActive(true);

            FindObjectOfType<AudioManager>().Play("win");
        }
        else
        {
            StartCoroutine(detonateAllMines());
            _losePanel.SetActive(true);
            FindObjectOfType<AudioManager>().Play("lose");
        }
        
    }

    private IEnumerator detonateAllMines()
    {
        
        foreach (TileComponent item in _gameSystem.TileFabric.Tiles)
        {
            if(item != null && item.Tile.Mine)
            {
                yield return new WaitForSeconds(0.4f);
                // item?.OpenTile(); - Не работает, выдает MissingReferenceException при обновлении игры.Почему???
                if(item != null) 
                    item.OpenTile();
            }
        }
    }

}
