using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Tile[,] tiles;
    public int rows;
    public int columns;

    void Start()
    {
        tiles = new Tile[columns, rows];

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                tiles[x, y] = new Tile();
            }
        }
    }

    public bool IsTileTaken(float row, float column)
    {
        return tiles[(int)row, (int)column].isTaken;
    }

    public void SetTileTaken(float row, float column, bool isTileTaken, GameObject obj)
    {
        tiles[(int)row, (int)column].isTaken = isTileTaken;
        tiles[(int)row, (int)column].obj = obj;
    }
}
