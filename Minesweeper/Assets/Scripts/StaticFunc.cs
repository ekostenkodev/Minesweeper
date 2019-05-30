using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class StaticFunc
{

    public static void CircleFunc<T>(T[,] array,int x,int y,UnityAction<T[,],int,int> func)
    {
        int size = array.GetLength(0);
        int matrixLength = 3;
        for (int xOffset = -1; xOffset < matrixLength-1; xOffset++)
        {
            if(!inBounds(x+xOffset,size))
                continue;
            for (int yOffset = -1; yOffset < matrixLength-1; yOffset++)
            {
                if(!inBounds(y+yOffset,size))
                    continue;
                func(array,x+xOffset,y+yOffset);
            }
        }
    }


    private static bool inBounds(int targetCoordinates, int arrayLength)
    {
        if(targetCoordinates < 0 || targetCoordinates >= arrayLength)
            return false;
        else
            return true;
    }

    public static (int,int) GetIndexes(TileComponent[,] array, TileComponent cell)
    {
        for (int row = 0; row < array.GetLength(0); row++)
        {
            for (int column = 0; column < array.GetLength(1); column++)
            {
                if(array[row,column].Equals(cell))
                    return (row,column);
            }
        }
        return (-1,-1);
    }
}
