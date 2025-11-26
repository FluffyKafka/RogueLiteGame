using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(RoomFog))]
public class Room_Castle : MonoBehaviour
{
    protected MapGenerater_Castle generater;
    [SerializeField] protected Tilemap background_wall;
    [SerializeField] protected Tilemap groundTilemap;
    [SerializeField] protected int minDecoAmount;
    [SerializeField] protected int maxDecoAmount;
    protected List<Vector2> flatPositions = new List<Vector2>();
    protected int usedFlatPositionIndexEnd = 0;

    public virtual void GenerateRoom(MapGenerater_Castle _generater)
    {
        generater = _generater;
        FillUpBackgroundWall();
        SearchFlatPosition();
        GenerateDecoration();
    }

    private void GenerateDecoration()
    {
        int randomDecoAmount = Random.Range(minDecoAmount, maxDecoAmount);
        int curDecoAmount = 0;

        while (curDecoAmount < randomDecoAmount)
        {
            Sprite randomDecoSprite = generater.decorations[Random.Range(0, generater.decorations.Count)];

            if (usedFlatPositionIndexEnd >= flatPositions.Count)
            {
                break;//无空闲位置
            }
            Vector3 randomPosition = GetRandomNonOverlapPosition(Mathf.CeilToInt(randomDecoSprite.bounds.size.x));

            randomPosition += new Vector3(0, 1f);//瓦片大小
            randomPosition += new Vector3(0, randomDecoSprite.bounds.size.y / 2 + generater.decoYOffset);//sprite高度

            GameObject newDecoration = Instantiate(generater.decorationPrefab, randomPosition, Quaternion.identity);
            newDecoration.GetComponent<SpriteRenderer>().sprite = randomDecoSprite;
            ++curDecoAmount;
        }
    }

    private Vector3 GetRandomNonOverlapPosition(int _width)
    {
        int randomPositionIndex = Random.Range(usedFlatPositionIndexEnd, flatPositions.Count);
        Vector3 randomPosition = flatPositions[randomPositionIndex];

        int usedPositionIndex = randomPositionIndex - _width / 2;
        if (usedPositionIndex < usedFlatPositionIndexEnd)
        {
            usedPositionIndex = usedFlatPositionIndexEnd;
        }
        int end = randomPositionIndex + _width / 2;
        if (end >= flatPositions.Count)
        {
            end = flatPositions.Count - 1;
        }
        for (; usedPositionIndex <= end; usedPositionIndex++)
        {
            Vector3 usedPosition = flatPositions[usedPositionIndex];
            flatPositions[usedPositionIndex] = flatPositions[usedFlatPositionIndexEnd];
            flatPositions[usedFlatPositionIndexEnd] = usedPosition;
            ++usedFlatPositionIndexEnd;
        }

        return randomPosition;
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
    protected virtual void SearchFlatPosition()
    {
        int radius = MapGenerater_Castle.instance.flatRadius;

        Vector3Int lowerLeftCoo = groundTilemap.cellBounds.min;

        //遍历房间内的每个瓦片位置
        for (int x = lowerLeftCoo.x + radius; x < (groundTilemap.cellBounds.size.x + lowerLeftCoo.x - radius); x++)
        {
            for (int y = lowerLeftCoo.y; y < (groundTilemap.cellBounds.size.y + lowerLeftCoo.y - 1); y++)//最高层上面必然没有方块，不需要判断
            {
                //若一个瓦片位置及其两侧flatRadius宽内的所有瓦片位置都符合条件：此处有瓦片且此处上方没有瓦片
                //则，此处是一个平坦位置
                bool isSuit = true;
                for (int flatCheckX = x - radius; flatCheckX <= x + radius; flatCheckX++)
                {
                    if (groundTilemap.GetTile(new Vector3Int(flatCheckX, y, 0)) == null
                        || groundTilemap.GetTile(new Vector3Int(flatCheckX, y + 1, 0)) != null
                        )//如果此处不为空方块
                    {
                        isSuit = false;
                    }
                }

                if (isSuit)
                {
                    flatPositions.Add((Vector2)groundTilemap.CellToWorld(new Vector3Int(x, y)));
                }
            }
        }
    }
}
