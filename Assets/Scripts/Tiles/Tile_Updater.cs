using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Updater
{
    private static Tile_Updater instance;
    public static Tile_Updater Instance
    {
        get
        {
            if (instance == null) instance = new Tile_Updater();
            return instance;
        }
    }

    public void ChangeTileState(Vector2 position, string newState)
    {
        EventSystem.Instance.Fire($"{position.x}, {position.y}", newState); //Set the tile's new state
    }

    public void UpdateAllTiles(int X, int Y) //X and Y dictate size of array
    {
        for(int y = 0; y < Y; y++) //Iterate through array left to right, top to bottom
        {
            for(int x = 0; x < X; x++)
            {
                EventSystem.Instance.Fire($"{x}, {y}", "Update"); //Update the tile at position x, y
            }
        }
    }
}
