using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class TileFabric : MonoBehaviour
{

    [Space]
    public  TileComponent TilePrefab = null;
    public  float Offset;
    [HideInInspector]
    public float TotalSize{get;private set;}
    public TileComponent[,] Tiles;
    public void ClearFabric()
    {
        if(transform.childCount != 0)
            Destroy(transform.GetChild(0).gameObject);
        Tiles = null;
    }



    public void GenerateTiles(int size,int numberOfMines,UnityAction<TileComponent,bool> clickEvent)
    {
        ClearFabric();
        Tiles = new TileComponent[size,size];
        int?[,] matrix = GetRandomMatrix(size,numberOfMines);
        var empty = new GameObject("Cells");
        float correctOffset = TilePrefab.GetComponent<SpriteRenderer>().bounds.size.x + Offset; 
        //Offset +=1;
        for (int row = 0; row < size; row++)
        {
            for (int column = 0; column < size; column++)
            {
                TileComponent current = Instantiate(TilePrefab,empty.transform);
                
                current.SetType(matrix[row,column]);
                current.TileClickEvent += clickEvent;
                current.transform.localPosition = new Vector3(-row+size/2,-column+size/2,0) * correctOffset;//new Vector3(row*_offset, column*_offset, 0); // todo offset

                Tiles[row,column] = current;
            }
        }
        empty.transform.parent = transform;
        
        if(size%2==0)
            empty.transform.position -= new Vector3(0.5f, 0, 0); 

        TotalSize = TilePrefab.GetComponent<SpriteRenderer>().bounds.size.x * size + Offset*(size-1);


    }

    private int?[,] GetRandomMatrix(int size,int numberOfBombs)
    {
        int?[,] matrix = new int?[size,size];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i,j] = 0;
            }
        }

        while(numberOfBombs>0)
        {
            int x = UnityEngine.Random.Range(0,size);
            int y = UnityEngine.Random.Range(0,size);
            if(matrix[x,y].HasValue)
            {
                matrix[x,y] = null;

                StaticFunc.CircleFunc<int?>(matrix,x,y,(array,i,j)=>array[i,j]++);

                numberOfBombs--;
            }
        }
        
        
        return matrix;
    }
}



