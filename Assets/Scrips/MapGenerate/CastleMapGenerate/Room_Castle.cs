using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room_Castle : MonoBehaviour
{
    protected MapGenerater_Castle generater;
    [SerializeField] protected Tilemap background_wall;

    public void FillUpBackgroundWall()
    {
        List<Tile> backgroundWallTiles = generater.backgroundWallTile;
        int tileCount = backgroundWallTiles.Count;

        int width = background_wall.cellBounds.x;
        int height = background_wall.cellBounds.y;
        int minX = background_wall.cellBounds.min.x;
        int minY = background_wall.cellBounds.min.y;

        for (int x = minX; x < minX + width; ++x)
        {
            for(int y = minY; y < minY + height; ++y)
            {
                Tile randomBackWall = backgroundWallTiles[Random.Range(0, tileCount)];
                background_wall.SetTile(new Vector3Int(x, y, 0), randomBackWall);
            }
        }
    }
}
