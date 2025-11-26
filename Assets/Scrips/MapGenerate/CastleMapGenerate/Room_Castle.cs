using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room_Castle : MonoBehaviour
{
    protected MapGenerater_Castle generater;
    [SerializeField] protected Tilemap background_wall;

    public void GenerateRoom(MapGenerater_Castle _generater)
    {
        generater = _generater;
        FillUpBackgroundWall();
    }
    protected virtual void FillUpBackgroundWall()
    {
        List<Tile> backgroundWallTiles = generater.backgroundWallTile;
        int tileCount = backgroundWallTiles.Count;

        int maxX = background_wall.cellBounds.max.x;
        int maxY = background_wall.cellBounds.max.y;
        int minX = background_wall.cellBounds.min.x;
        int minY = background_wall.cellBounds.min.y;

        Debug.Log(maxX);
        for (int x = minX; x < maxX; ++x)
        {
            for(int y = minY; y < maxY; ++y)
            {
                if(background_wall.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    Tile randomBackWall = backgroundWallTiles[Random.Range(0, tileCount)];
                    background_wall.SetTile(new Vector3Int(x, y, 0), randomBackWall);
                }
            }
        }
    }
}
