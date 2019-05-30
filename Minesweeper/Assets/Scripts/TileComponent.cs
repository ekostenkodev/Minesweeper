using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TileComponent : MonoBehaviour
{
    [HideInInspector]
    public TileEntity Tile;
    public event UnityAction<TileComponent,bool> TileClickEvent;

    [SerializeField]
    private Sprite _openedTilePrefab;
    [SerializeField]
    private  SpriteRenderer _minePrefab = null;
    [SerializeField]
    private  GameObject _countPrefab = null;
    
    private GameObject _type;
    private GameObject _flag;


    private void Awake() 
    {
        Tile = new TileEntity();
        _flag = transform.Find("Flag").gameObject;
    }

    public void OnClick(bool flag)
    {
        if(Tile.OpenStatus)
            return;
        TileClickEvent?.Invoke(this,flag);
    }

    public void OpenTile()
    {
        if(Tile.OpenStatus)
            return;
        Tile.OpenStatus = true;
        ChangeFlagStatus(false);
        GetComponent<SpriteRenderer>().sprite = _openedTilePrefab;

        _type?.SetActive(true);
        
    }



    public void SetType(int? countOfBombs)
    {  
        Tile.CountOfMines = countOfBombs;
        Tile.Mine = !countOfBombs.HasValue; // если null - значит, там бомба 

        if(!countOfBombs.HasValue)
        {
            _type = Instantiate(_minePrefab,transform).gameObject;
        }
        else if(countOfBombs.Value != 0)
        {
            _type = Instantiate(_countPrefab,transform);
            _type.GetComponent<TextMeshPro>().text = countOfBombs.ToString();
            _type?.GetComponent<TextMeshPro>().fontMaterial.SetColor(ShaderUtilities.ID_GlowColor, Colors.TextColorShader[Tile.CountOfMines.Value-1,0]);
        }

        _type?.gameObject.SetActive(false);

    }




    public void ChangeFlagStatus(bool status)
    {
        Tile.Flag = status;
        _flag.SetActive(status); 
    }


    
}
