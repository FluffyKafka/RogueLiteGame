using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(RoomFog))]
public class Room_OutCastle : MonoBehaviour
{
    protected RoomType_OutCastle type;

    [Header("Room Info")]
    [SerializeField] public Access_OutCastle upperAccess;
    [SerializeField] public Access_OutCastle lowerAccess;
    [SerializeField] public Access_OutCastle leftAccess;
    [SerializeField] public Access_OutCastle rightAccess;
    [SerializeField] public int minDecorationAmount;
    [SerializeField] public int maxDecorationAmount;

    [Header("Room Element Generate Info")]
    [SerializeField] protected Tilemap groundTilemap;

    protected List<Vector2> flatPositions = null;
    protected int usedFlatPositionIndexEnd = 0;

    public List<RoomGenerateStruct_OutCastle> GenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        List<RoomGenerateStruct_OutCastle> nextRooms = new List<RoomGenerateStruct_OutCastle>();
        PreGenerateRoom(_manager, _currentLine, _index);
        RoomGenerateStruct_OutCastle branchRoom = GenerateCurrentRoom(_manager, _currentLine, _index);
        RoomGenerateStruct_OutCastle nextRoom = GenerateNextRoom(_manager, _currentLine, _index);
        if(branchRoom.index != -1)
        {
            nextRooms.Add(branchRoom);
        }
        if(nextRoom.index != -1)
        {
            nextRooms.Add(nextRoom);
        }
        return nextRooms;
    }
    protected virtual void PreGenerateRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        flatPositions = GetFlatPositionsInRoomByRadius(2);
        usedFlatPositionIndexEnd = 0;
    }

    protected virtual RoomGenerateStruct_OutCastle GenerateCurrentRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {
        GenerateDecorations(_manager);
        return new RoomGenerateStruct_OutCastle(-1, null, null);
    }
    protected void GenerateDecorations(MapGenerateManager_OutCastle _manager)
    {
        int randomDecorationAmount = Random.Range(minDecorationAmount, maxDecorationAmount);
        int currentDecorationAmount = 0;

        while (currentDecorationAmount < randomDecorationAmount)
        {
            Sprite randomDecorationSprite = _manager.decorations[Random.Range(0, _manager.decorations.Count)];

            if (!HaveFreePosition())
            {
                break;//无空闲位置
            }
            Vector3 randomPosition = GetRandomNonOverlapPosition(Mathf.CeilToInt(randomDecorationSprite.bounds.size.x));
            randomPosition += new Vector3(0, 1f);//瓦片大小
            randomPosition += new Vector3(0, randomDecorationSprite.bounds.size.y / 2);//sprite高度

            GameObject newDecoration = Instantiate(_manager.decorationPrefab, randomPosition, Quaternion.identity);
            newDecoration.GetComponent<SpriteRenderer>().sprite = randomDecorationSprite;
            ++currentDecorationAmount;
        }
    }
    private bool HaveFreePosition()
    {
        if (usedFlatPositionIndexEnd >= flatPositions.Count)
        {
            return false;//无空闲位置
        }
        else
        {
            return true;
        }
    }
    private Vector3 GetRandomNonOverlapPosition(int _width)
    {
        int randomPositionIndex = Random.Range(usedFlatPositionIndexEnd, flatPositions.Count);
        Vector3 randomPosition = flatPositions[randomPositionIndex];

        int usedPositionIndex = randomPositionIndex - _width / 2;
        if(usedPositionIndex < usedFlatPositionIndexEnd)
        {
            usedPositionIndex = usedFlatPositionIndexEnd;
        }
        int end = randomPositionIndex + _width / 2;
        if(end >= flatPositions.Count)
        {
            end = flatPositions.Count - 1;
        }
        for (;usedPositionIndex <= end; usedPositionIndex++)
        {
            Vector3 usedPosition = flatPositions[usedPositionIndex];
            flatPositions[usedPositionIndex] = flatPositions[usedFlatPositionIndexEnd];
            flatPositions[usedFlatPositionIndexEnd] = usedPosition;
            ++usedFlatPositionIndexEnd;
        }

        return randomPosition;
    }

    protected virtual RoomGenerateStruct_OutCastle GenerateNextRoom(MapGenerateManager_OutCastle _manager, Line_OutCastle _currentLine, int _index)
    {

        Room_OutCastle newRoom = null;
        //判断下一个房间类型
        RoomType_OutCastle roomType = _currentLine.GetNextRoomType(_index, type);

        switch (roomType)
        {
            case RoomType_OutCastle.Battle:
                newRoom = GetNewRoomByPrefabList(_manager.battleRoomPrefabs);
                break;
            case RoomType_OutCastle.Passage:
                newRoom = GetNewRoomByPrefabList(_manager.passageRoomPrefabs);
                break;
            case RoomType_OutCastle.Exit:
                newRoom = GetNewRoomByPrefabList(_manager.exitRoomPrefabs);
                break;
            case RoomType_OutCastle.Reward:
                newRoom = GetNewRoomByPrefabList(_manager.rewardRoomPrefabs);
                break;
            case RoomType_OutCastle.Branch:
                newRoom = GetNewRoomByPrefabList(_manager.branchRoomPrefabs);
                break;
            case RoomType_OutCastle.BranchExit:
                newRoom = GetNewRoomByPrefabList(_manager.branchExitRoomPrefabs);
                break;
            default:
                Assert.IsTrue(false, "未定义的房间类型： " + roomType); break;
        }
        return new RoomGenerateStruct_OutCastle(_index + 1, newRoom, _currentLine);
    }

    protected Room_OutCastle GetNewRoomByPrefabList(List<GameObject> _list)
    {
        //选一个房间
        int battleRoomIndex = Random.Range(0, _list.Count);

        //计算房间位置
        Room_OutCastle nextBattleRoom = _list[battleRoomIndex].GetComponent<Room_OutCastle>();
        Vector3 nextBattleRoomPosition = GetNextRoomPosition(rightAccess, nextBattleRoom);

        //生成房间
        Room_OutCastle newBattleRoom =
            Instantiate(_list[battleRoomIndex], nextBattleRoomPosition, Quaternion.identity).GetComponent<Room_OutCastle>();
        return newBattleRoom;
    }

    protected Vector3 GetNextRoomPosition(Access_OutCastle _exitAccess, Room_OutCastle _nextRoom)
    {
        Transform nextRoomEnterTransform;
        if (_exitAccess == upperAccess)
        {
            nextRoomEnterTransform = _nextRoom.lowerAccess.transform;
        }
        else if (upperAccess == lowerAccess)
        {
            nextRoomEnterTransform = _nextRoom.upperAccess.transform;
        }
        else if (upperAccess == leftAccess)
        {
            nextRoomEnterTransform = _nextRoom.rightAccess.transform;
        }
        else
        {
            nextRoomEnterTransform = _nextRoom.leftAccess.transform;
        }

        return _exitAccess.transform.position - nextRoomEnterTransform.position;
    }

    protected List<Vector2> GetFlatPositionsInRoomByRadius(int _flatRadius)
    {
        List<Vector2> flatPositions = new List<Vector2>();

        Vector3Int lowerLeftCoo = groundTilemap.cellBounds.min;

        //遍历房间内的每个瓦片位置
        for (int x = lowerLeftCoo.x + _flatRadius; x < (groundTilemap.cellBounds.size.x + lowerLeftCoo.x - _flatRadius); x++)
        {
            for (int y = lowerLeftCoo.y; y < (groundTilemap.cellBounds.size.y + lowerLeftCoo.y - 1); y++)//最高层上面必然没有方块，不需要判断
            {
                //若一个瓦片位置及其两侧flatRadius宽内的所有瓦片位置都符合条件：此处有瓦片且此处上方没有瓦片
                //则，此处是一个平坦位置
                bool isSuit = true;
                for(int flatCheckX = x - _flatRadius; flatCheckX <= x + _flatRadius; flatCheckX++)
                {
                    if (groundTilemap.GetTile(new Vector3Int(flatCheckX, y, 0)) == null 
                        || groundTilemap.GetTile(new Vector3Int(flatCheckX, y + 1, 0)) != null
                        )//如果此处不为空方块
                    {
                        isSuit = false;
                    }
                }

                if(isSuit)
                {
                    flatPositions.Add((Vector2)groundTilemap.CellToWorld(new Vector3Int(x, y)));
                }
            }
        }

        return flatPositions;
    }
}
