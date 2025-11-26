using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

[Serializable]
public class RewardSlot_Castle
{
    [SerializeField] public int minRewardTime = 1;
    [SerializeField] public int maxRewardTime = 3;//每个房间提供三个奖励生成位置
    [SerializeField] public int rewardAmount;
    [SerializeField] public int advancedAmount;
    [Range(0, 100)][SerializeField] public float witcherRate;
    [Range(0, 100)][SerializeField] public float traderRate;
    [Range(0, 100)][SerializeField] public float blackSmithRate;
    [Range(0, 100)][SerializeField] public float advancedRewardRate;
    [Range(0, 100)][SerializeField] public float mimicRate;
    [Range(0, 100)][SerializeField] public float mimicAdvancedRewardRate;
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

    [Header("Reward Prefabs")]
    public GameObject witcherPrefab;
    public GameObject traderPrefab;
    public GameObject blackSmithPrefab;
    public GameObject advancedRewardChestPrefab;
    public GameObject primaryRewardChestPrefab;
    public GameObject mimicChestPrefab;
    public List<Drop> primaryRewards;
    public List<Drop> advancedRewards;
    public float advancedRewardPrice = 150f;

    [Header("Room Info")]
    public List<Sprite> decorations;
    public GameObject decorationPrefab;
    public float decoYOffset = -0.5f;
    public List<GameObject> enemyPrefabList;
    public float enemyYOffeset = 1f;

    [Header("Map Info")]
    public int width;
    public int height;
    public int difficulty;
    public int difficultyRandomDivider = 3;
    public int flatRadius;
    [Range(0, 100)] public int upRate = 20;
    public float roomWidth;
    public float roomHeight;
    public int randomGenerateTime = 5;
    public int maxTryRandomGenerateTime = 10;
    public List<RewardSlot_Castle> rewards;

    class RoomHelper
    {
        public RoomType_Castle type = RoomType_Castle.Dead;
        public Room_Castle room = null;
        public bool isLeft = false;
        public bool isRight = false;
        public bool isUp = false;
        public bool isDown = false;
        public int difficulty = 0;

        public bool IsCross()
        {
            return isUp && isDown && isLeft && isRight;
        }
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
        InitSubPath();
        InitRewardRoom();
        InitPassageRoomDifficulty();

        GenerateEntryRoom();
        GenerateMapRoom();
        GenerateExitRoom();

        GenerateEntryEdge();
        GenerateExitEdge();
        GenerateMapEdge();


        PlayerManager.instance.player.transform.position
            = (entry.room as EntryRoom_Castle).playerEnterTransform.position;
    }

    private void InitPassageRoomDifficulty()
    {
        int passageRoomCount = 0;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (map[x][y].type == RoomType_Castle.Passgae)
                {
                    ++passageRoomCount;
                }
            }
        }
        int roomDifficulty = difficulty / passageRoomCount;
        int randomDifficulty = roomDifficulty / difficultyRandomDivider;
        int minDiff = roomDifficulty - randomDifficulty;
        int maxDiff = roomDifficulty + randomDifficulty;
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (map[x][y].type == RoomType_Castle.Passgae)
                {
                    map[x][y].difficulty = UnityEngine.Random.Range(minDiff, maxDiff);
                }
            }
        }
    }
    private void InitRewardRoom()
    {
        int rewardTime = rewards.Count;
        List<RoomHelper> randomRooms = new List<RoomHelper>();
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                RoomHelper room = map[x][y];
                if (room.type == RoomType_Castle.Passgae && !room.isUp)
                {
                    randomRooms.Add(room);
                }
            }
        }
        while (randomRooms.Count > 0 && rewardTime > 0)
        {
            --rewardTime;
            int index = UnityEngine.Random.Range(0, randomRooms.Count);
            randomRooms[index].type = RoomType_Castle.Reward;
            randomRooms.RemoveAt(index);
        }
    }
    private void InitSubPath()
    {
        List<Vector2Int> baseRooms = new List<Vector2Int>();
        for (int x = 0; x < width; ++x)
        {
            for (int y = 0; y < height; ++y)
            {
                if (map[x][y].type == RoomType_Castle.Passgae)
                {
                    if (map[x][y].isUp)
                    {
                        baseRooms.Add(new Vector2Int(x, y));
                    }
                    if (map[x][y].isDown)
                    {
                        baseRooms.Add(new Vector2Int(x, y));
                    }
                    if (map[x][y].isLeft)
                    {
                        baseRooms.Add(new Vector2Int(x, y));
                    }
                    if (map[x][y].isRight)
                    {
                        baseRooms.Add(new Vector2Int(x, y));
                    }
                }
            }
        }
        List<int> randomDirs = new List<int>();
        Vector2Int curCell = new Vector2Int(-1, -1);
        while (maxTryRandomGenerateTime > 0 && randomGenerateTime > 0)
        {
            --maxTryRandomGenerateTime;
            Vector2Int roomCell;
            if (curCell.x >= 0)
            {
                roomCell = curCell;
            }
            else
            {
                roomCell = baseRooms[UnityEngine.Random.Range(0, baseRooms.Count)];
            }
            curCell.x = -1;

            RoomHelper room = map[roomCell.x][roomCell.y];
            randomDirs.Clear();
            if (roomCell.y + 1 <= height - 1 && map[roomCell.x][roomCell.y + 1].type == RoomType_Castle.Dead)
            {
                randomDirs.Add(0);
            }
            if (roomCell.y - 1 >= 0 && map[roomCell.x][roomCell.y - 1].type == RoomType_Castle.Dead)
            {
                randomDirs.Add(1);
            }
            if (roomCell.x + 1 <= width - 1 && map[roomCell.x + 1][roomCell.y].type == RoomType_Castle.Dead)
            {
                randomDirs.Add(2);
            }
            if (roomCell.x - 1 >= 0 && map[roomCell.x - 1][roomCell.y].type == RoomType_Castle.Dead)
            {
                randomDirs.Add(3);
            }
            if (randomDirs.Count == 0)
            {
                baseRooms.Remove(roomCell);
                continue;
            }

            Vector2Int targetCell = roomCell;
            int dirRandom = randomDirs[UnityEngine.Random.Range(0, randomDirs.Count)];
            switch (dirRandom)
            {
                case 0:
                    {
                        targetCell.y += 1;
                        map[roomCell.x][roomCell.y].isUp = true;
                        map[targetCell.x][targetCell.y].isDown = true;
                        map[targetCell.x][targetCell.y].type = RoomType_Castle.Passgae;
                        --randomGenerateTime;
                        break;
                    }
                case 1:
                    {
                        targetCell.y -= 1;
                        map[roomCell.x][roomCell.y].isDown = true;
                        map[targetCell.x][targetCell.y].isUp = true;
                        map[targetCell.x][targetCell.y].type = RoomType_Castle.Passgae;
                        --randomGenerateTime;
                        break;
                    }
                case 2:
                    {
                        targetCell.x += 1;
                        map[roomCell.x][roomCell.y].isRight = true;
                        map[targetCell.x][targetCell.y].isLeft = true;
                        map[targetCell.x][targetCell.y].type = RoomType_Castle.Passgae;
                        --randomGenerateTime;
                        break;
                    }
                case 3:
                    {
                        targetCell.x -= 1;
                        map[roomCell.x][roomCell.y].isLeft = true;
                        map[targetCell.x][targetCell.y].isRight = true;
                        map[targetCell.x][targetCell.y].type = RoomType_Castle.Passgae;
                        --randomGenerateTime;
                        break;
                    }
            }

            curCell = targetCell;
        }
    }
    private void GenerateMapEdge()
    {
        float botY = transform.position.y - roomHeight;
        float topY = transform.position.y + height * roomHeight;
        Vector3 generatePosition = transform.position;
        for (int x = -1; x <= width; ++x)
        {
            generatePosition.x = transform.position.x + x * roomWidth;

            generatePosition.y = topY;
            Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
            generatePosition.y = botY;
            Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        }

        float leftX = transform.position.x - roomWidth;
        float rightX = transform.position.x + width * roomWidth;
        for (int y = 0; y < height; ++y)
        {
            generatePosition.y = transform.position.y + y * roomHeight;

            if (y != 0)
            {
                generatePosition.x = leftX;
                Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
            }

            if (y != height - 1)
            {
                generatePosition.x = rightX;
                Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
            }
        }
    }
    private void GenerateExitEdge()
    {
        Vector3 generatePosition = transform.position;
        Vector3 endRoomPosition =
            new Vector3(transform.position.x + width * roomWidth, transform.position.y + (height - 1) * roomHeight);
        generatePosition.x = endRoomPosition.x + roomWidth;
        generatePosition.y = endRoomPosition.y;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        generatePosition.x = endRoomPosition.x;
        generatePosition.y = endRoomPosition.y + roomHeight;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        generatePosition.x = endRoomPosition.x + roomWidth;
        generatePosition.y = endRoomPosition.y + roomHeight;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        generatePosition.x = endRoomPosition.x + roomWidth;
        generatePosition.y = endRoomPosition.y - roomHeight;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
    }
    private void GenerateEntryEdge()
    {
        Vector3 generatePosition = transform.position;
        generatePosition.x = transform.position.x - 2 * roomWidth;
        generatePosition.y = transform.position.y;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        generatePosition.x = transform.position.x - roomWidth;
        generatePosition.y = transform.position.y - roomHeight;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        generatePosition.x = transform.position.x - 2 * roomWidth;
        generatePosition.y = transform.position.y - roomHeight;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
        generatePosition.x = transform.position.x - 2 * roomWidth;
        generatePosition.y = transform.position.y + roomHeight;
        Instantiate(GetRandomPrefab(deadRoomPrefabs), generatePosition, Quaternion.identity);
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
                    if (map[x][y].type == RoomType_Castle.Passgae)
                    {
                        (map[x][y].room as PassageRoom_Castle).difficulty = map[x][y].difficulty;
                    }
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
            case RoomType_Castle.Reward:
                return Instantiate(GetRandomPrefab(rewardRoomPrefabs), _position, Quaternion.identity);
        }
        return null;
    }
    private GameObject GetRandomPrefab(List<GameObject> _list)
    {
        return _list[UnityEngine.Random.Range(0, _list.Count)];
    }

    private void InitMainPath()//生成一条由Passage型房构成的主路经
    {
        Vector2Int curLoc = new Vector2Int(0, 0);
        Vector2Int endLoc = new Vector2Int(width - 1, height - 1);
        while (curLoc != endLoc)
        {
            if(UnityEngine.Random.Range(0, 100) < upRate)
            {
                MoveTo(ref curLoc, Direction.Up);
            }
            else if(UnityEngine.Random.Range(0, 2) == 0)
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

#if UNITY_EDITOR
    [ContextMenu("Fill Up Rewards")]
    private void GetItemDatabase()
    {
        string[] assetNames = AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/Scrips/Item/ItemData" });

        foreach (string SOName in assetNames)
        {
            var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOPath);

            if(itemData.price < advancedRewardPrice)
            {
                primaryRewards.Add(new Drop(itemData, 100 * (itemData.price / ((advancedRewardPrice/2) + itemData.price))));
            }
            else
            {
                advancedRewards.Add(new Drop(itemData, 100 * (itemData.price / (advancedRewardPrice + itemData.price))));
            }
        }
    }
#endif
}