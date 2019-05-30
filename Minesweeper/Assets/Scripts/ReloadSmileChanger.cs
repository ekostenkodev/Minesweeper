using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadSmileChanger : MonoBehaviour
{
    [SerializeField]
    private GameSystem _gameSystem;
    [Space]
    [SerializeField]
    private Sprite _smilePrefab;
    [SerializeField]
    private Sprite _winPrefab;
    [SerializeField]
    private Sprite _losePrefab;
    private Image _image;
    
    void Start()
    {
        _image = GetComponent<Image>();

        _gameSystem.GameStartEvent += () =>_image.sprite = _smilePrefab;
        _gameSystem.EndGameEvent += changeResultImage;
    }
    private void changeResultImage(bool result)
    {
        if(result)
        {
            _image.sprite = _winPrefab;
        }
        else
        {
            _image.sprite = _losePrefab;
        }
    }
}
