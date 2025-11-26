using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;

public enum RoomType_Castle
{
    Entry,
    Exit,
    Passgae,
    Reward,
    Dead
}

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

//生成逻辑：将地图整体视为一个二维矩阵，每个格子表示一个房间，
//预设一组不同贯通情况的房间（左右、左右下、十字路口、奖励房（左右贯通）、出入口），
//先计算出一条主路经（向上或向左右）
//再计算支线路径
//通道房兼具战斗房的功能
public class MapGenerater_Castle : MonoBehaviour
{
    public static MapGenerater_Castle instance;

    [Header("Random Background Wall Tiles")]
    public List<Tile> backgroundWallTile;

    [Header("Room Prefabs")]
    public List<GameObject> entryRoomPrefabs;
    public List<GameObject> exitRoomPrefabs;
    public List<GameObject> rewardRoomPrefabs;
    public List<GameObject> passageRoomPrefabs_LR;
    public List<GameObject> passageRoomPrefabs_LRD;
    public List<GameObject> passageRoomPrefabs_Cross;
    public List<GameObject> deadRoomPrefabs;

    [Header("Map Info")]
    public int width;
    public int height;
    public int rewardCount;
    public int difficulty;
    public int flatRadius;
    [Range(0, 100)] public int upRate = 20;
    public float roomWidth;
    public float roomHeight;

    class RoomHelper
    {
        public RoomType_Castle type = RoomType_Castle.Dead;
        public Room_Castle room = null;
        public bool isLeft = false;
        public bool isRight = false;
        public bool isUp = false;
        public bool isDown = false;
    }

    private List<List<RoomHelper>> map;
    private RoomHelper entry = null;
    private RoomHelper exit = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitMap();
        InitMainPath();

        GenerateEntryRoom();
        GenerateMapRoom();
        GenerateExitRoom();

        PlayerManager.instance.player.transform.position
            = (entry.room as EntryRoom_Castle).playerEnterTransform.position;
    }

    private void GenerateExitRoom()
    {
        Vector3 generatePosition = transform.position;
        generatePosition.x = transform.position.x + width * roomWidth;
        generatePosition.y = transform.position.y + (height - 1) * roomHeight;
        Room_Castle room =
            Instantiate(GetRandomPrefab(exitRoomPrefabs), generatePosition, Quaternion.identity)
            .GetComponent<Room_Castle>();
        room.GenerateRoom(this);
        exit = new RoomHelper();
        exit.isLeft = true;
        exit.type = RoomType_Castle.Exit;
        exit.room = room;
    }
    private void GenerateMapRoom()
    {
        Vector3 generatePosition = transform.position;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                generatePosition.x = transform.position.x + roomWidth * x;
                generatePosition.y = transform.position.y + roomHeight * y;
                GameObject roomGameObject = GenerateRoomByType(map[x][y], ref generatePosition);
                if(roomGameObject != null)
                {
                    map[x][y].room = roomGameObject.GetComponent<Room_Castle>();
                    map[x][y].room.GenerateRoom(this);
                }
            }
        }
    }
    private void GenerateEntryRoom()
    {
        Vector3 generatePosition = transform.position;
        generatePosition.x = transform.position.x - roomWidth;
        generatePosition.y = transform.position.y;
        Room_Castle room =
            Instantiate(GetRandomPrefab(entryRoomPrefabs), generatePosition, Quaternion.identity)
            .GetComponent<Room_Castle>();
        room.GenerateRoom(this);
        entry = new RoomHelper();
        entry.type = RoomType_Castle.Entry;
        entry.room = room;
        entry.isRight = true;
    }
    private GameObject GenerateRoomByType(RoomHelper _room, ref Vector3 _position)
    {
        switch(_room.type)
        {
            case RoomType_Castle.Entry:
                return Instantiate(GetRandomPrefab(entryRoomPrefabs), _position, Quaternion.identity);
            case RoomType_Castle.Exit:
                return Instantiate(GetRandomPrefab(exitRoomPrefabs), _position, Quaternion.identity);
            case RoomType_Castle.Passgae:
                {
                    if(_room.isUp)
                    {
                        return Instantiate(GetRandomPrefab(passageRoomPrefabs_Cross), _position, Quaternion.identity);
                    }
                    else if(_room.isDown)
                    {
                        return Instantiate(GetRandomPrefab(passageRoomPrefabs_LRD), _position, Quaternion.identity);
                    }
                    else
                    {
                        return Instantiate(GetRandomPrefab(passageRoomPrefabs_LR), _position, Quaternion.identity);
                    }
                }
            case RoomType_Castle.Dead:
                return Instantiate(GetRandomPrefab(deadRoomPrefabs), _position, Quaternion.identity);
        }
        return null;
    }
    private GameObject GetRandomPrefab(List<GameObject> _list)
    {
        return _list[Random.Range(0, _list.Count)];
    }

    private void InitMainPath()//生成一条由Passage型房构成的主路经
    {
        Vector2Int curLoc = new Vector2Int(0, 0);
        Vector2Int endLoc = new Vector2Int(width - 1, height - 1);
        while (curLoc != endLoc)
        {
            if(Random.Range(0, 100) < upRate)
            {
                MoveTo(ref curLoc, Direction.Up);
            }
            else if(Random.Range(0, 2) == 0)
            {
                MoveTo(ref curLoc, Direction.Left);
            }
            else
            {
                MoveTo(ref curLoc, Direction.Right);
            }
        }
        map[0][0].isLeft = true;
        map[width - 1][height - 1].isRight = true;
    }
    private void MoveTo(ref Vector2Int _curLoc, Direction _dir)
    {
        if (IsEdge(ref _curLoc, Direction.Up))
        {
            _dir = Direction.Right;
        }

        if (IsEdge(ref _curLoc, _dir))
        {
            MoveTo(ref _curLoc, GetReverseDir(_dir));
        }
        else if(IsPassage(ref _curLoc, _dir))
        {
            MoveTo(ref _curLoc, Direction.Up);
        }
        else
        {
            ConnectTo(ref _curLoc, _dir);
        }
    }
    private Direction GetReverseDir(Direction _dir)
    {
        switch(_dir)
        {
            case Direction.Up:return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
        }
        return Direction.Up;
    }
    private bool IsEdge(ref Vector2Int _loc, Direction _dir)
    {
        switch(_dir)
        {
            case Direction.Up: return _loc.y >= height - 1;
            case Direction.Down: return _loc.y <= 0;
            case Direction.Left: return _loc.x <= 0;
            case Direction.Right: return _loc.x >= width - 1;
        }
        return false;
    }
    private bool IsPassage(ref Vector2Int _loc, Direction _dir)
    {
        switch (_dir)
        {
            case Direction.Up: return map[_loc.x][_loc.y + 1].type == RoomType_Castle.Passgae;
            case Direction.Down: return map[_loc.x][_loc.y - 1].type == RoomType_Castle.Passgae;
            case Direction.Left: return map[_loc.x - 1][_loc.y].type == RoomType_Castle.Passgae;
            case Direction.Right: return map[_loc.x + 1][_loc.y].type == RoomType_Castle.Passgae;
        }
        return false;
    }
    private void ConnectTo(ref Vector2Int _loc, Direction _dir)
    {
        int tarX = _loc.x;
        int tarY = _loc.y;
        RoomHelper left = map[_loc.x][_loc.y];
        switch (_dir)
        {
            case Direction.Up:
                {
                    _loc.y += 1;
                    RoomHelper right = map[_loc.x][_loc.y];
                    Connect_SetTypeHelper(left, right);
                    left.isUp = true;
                    right.isDown = true;
                    break;
                } 
            case Direction.Down:
                {
                    _loc.y -= 1;
                    RoomHelper right = map[_loc.x][_loc.y];
                    Connect_SetTypeHelper(left, right);
                    left.isDown = true;
                    right.isUp = true;
                    break;
                }
            case Direction.Left:
                {
                    _loc.x -= 1;
                    RoomHelper right = map[_loc.x][_loc.y];
                    Connect_SetTypeHelper(left, right);
                    left.isLeft = true;
                    right.isRight = true;
                    break;
                }
            case Direction.Right:
                {
                    _loc.x += 1;
                    RoomHelper right = map[_loc.x][_loc.y];
                    Connect_SetTypeHelper(left, right);
                    left.isRight = true;
                    right.isLeft = true;
                    break;
                }
        }
    }
    private void Connect_SetTypeHelper(RoomHelper _left, RoomHelper _right)
    {
        _left.type = RoomType_Castle.Passgae;
        _right.type = RoomType_Castle.Passgae;
    }

    private void InitMap()
    {
        map = new List<List<RoomHelper>>();
        for (int x = 0; x < width; ++x)
        {
            map.Add(new List<RoomHelper>());
            for (int y = 0; y < height; ++y)
            {
                map[x].Add(new RoomHelper());
            }
        }
    }
}