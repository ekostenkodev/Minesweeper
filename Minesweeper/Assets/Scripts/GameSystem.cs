using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class GameSystem : MonoBehaviour
{

    public event UnityAction GameStartEvent;
    public event UnityAction<TileComponent,bool> ClickEvent;
    public event UnityAction<bool> EndGameEvent;
    
    public int Size = 9;
    public int NumberOfMines = 9;

    public TileFabric TileFabric = null;

    private int _openedTilesCount;
    private int openedTilesCount 
    {
        get=>_openedTilesCount;
        set
        {
            _openedTilesCount = value;
            if(_openedTilesCount <=0 )
                EndGameEvent?.Invoke(true);
        }
    }
    public bool FlagMode {get;set;}

    public int FlagCount;

    #region MonoBehaviour
    private void OnValidate() 
    {
        int tileCount = Size*Size;
        if(NumberOfMines > tileCount)
            NumberOfMines = tileCount;
    }
    #endregion



    void Awake()
    {
        Size = PlayerPrefs.GetInt("Size");
        NumberOfMines = PlayerPrefs.GetInt("Mines");
        FlagCount = NumberOfMines;
        StartNewGame();
    }

    public void StartNewGame()
    {
        StopAllCoroutines();
        TileFabric.GenerateTiles(Size,NumberOfMines,SimpleClickEvent);

        _openedTilesCount = Size*Size - NumberOfMines;

        GameStartEvent?.Invoke();
    }

    private void MineClick(TileComponent tile)
    { 

        tile.OpenTile();

        EndGameEvent?.Invoke(false);
    }



    private void SimpleClickEvent(TileComponent clickedCell,bool flag)
    {

        ClickEvent?.Invoke(clickedCell,FlagMode || flag);

        if(FlagMode || flag)
        {
            clickedCell.ChangeFlagStatus(!clickedCell.Tile.Flag);
            FindObjectOfType<AudioManager>().Play("flag");
        }
        else if(clickedCell.Tile.Flag)
        {
            return;
        }
        else if(clickedCell.Tile.Mine)
        {
            MineClick(clickedCell);
            FindObjectOfType<AudioManager>().Play("mine");

        }
        else
        {
            var (row,column) = StaticFunc.GetIndexes(TileFabric.Tiles,clickedCell);
            openSimpleTile(row,column);
            FindObjectOfType<AudioManager>().Play("open");

        }

    }


    
    private void openSimpleTile(int row,int column)
    {   
        TileComponent currentTile = TileFabric.Tiles[row,column];
        if(currentTile.Tile.OpenStatus || currentTile.Tile.Flag)
            return;
        
        currentTile.OpenTile();

        openedTilesCount--;

        if(currentTile.Tile.CountOfMines!=0)
        {
            return;
        }
        else
        {
            StaticFunc.CircleFunc<TileComponent>(TileFabric.Tiles,row,column,(array,i,j)=>openSimpleTile(i,j));
        }
        
    }
}
