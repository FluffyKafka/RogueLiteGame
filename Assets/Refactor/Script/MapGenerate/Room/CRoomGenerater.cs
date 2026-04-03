using EnemySystem;
using Item;
using PlayerSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace MapGenerate
{
    internal enum EEventType
    {
        Witch,
        Trader,
        BlackSmith,
        RewardBox,
        AdvancedRewardBox
    }

    internal class CRoomGenerater : MonoBehaviour, IRoomGenerator
    {
        [Header("PassageRoomPrototypes")]
        [SerializeField] protected List<GameObject> horizonRooms;
        [SerializeField] protected List<GameObject> crossRooms;

        [Serializable]
        protected class DEventRate
        {
            public EEventType type;
            public float weight;
            public float weightDefault;
            public bool isSingle = false;
        }
        [Header("Events")]
        [SerializeField] protected List<DEventRate> events;
        [SerializeField] protected Vector2 rewardItemCount;
        [SerializeField] protected Vector2 rewardCoinCount;
        [SerializeField] protected List<ScriptableObject> rewardItems;
        [SerializeField] protected Vector2 advancedRewardCoinCount;
        [SerializeField] protected Vector2 advancedRewardsCount;
        [SerializeField] protected List<ScriptableObject> advancedRewardItems;

        [Header("Tile替换列表")]
        [SerializeField] protected List<DTile> groundTiles = new List<DTile>();
        [SerializeField] protected List<DTile> backgroundTiles = new List<DTile>();
        [SerializeField] protected List<DTile> platformTiles = new List<DTile>();

        [Header("TileMap名称")]
        [SerializeField] protected string groundTilemapName = "Ground";
        [SerializeField] protected string backgroundTilemapName = "Background";
        [SerializeField] protected string platformTilemapName = "Platform";

        [Header("Replace Tile")]
        [SerializeField] protected TileBase backgroundReplaceTile;

        [Header("Entity Generation Info")]
        [SerializeField] protected int entityHeight;
        [SerializeField] protected int entityRadius;
        [Header("Decoration")]
        [SerializeField] protected GameObject entryDoorPrefab;
        [SerializeField] protected GameObject decorationPrefab;
        [SerializeField] protected List<Sprite> decoSpriteList;
        [Header("Enemy")]
        [SerializeField] protected List<GameObject> enemys;

        [Header("Room Info")]
        [SerializeField] protected int boundWallThickness = 2;
        [SerializeField] protected int leftRightHight = 4;
        [SerializeField] protected Vector2 roomSize;

        [Header("Test")]
        [SerializeField] protected bool isTest = false;
        [SerializeField] protected bool haveLeftWall = true;
        [SerializeField] protected bool haveUpWall = true;
        [SerializeField] protected bool haveDownWall = true;
        [SerializeField] protected bool haveRightWall = true;
        [SerializeField] protected GameObject roomPrototype;
        [SerializeField] protected Transform generateTransform;
        [SerializeField] protected float enemyDifficulty;
        [SerializeField] protected ERoomType type;
        [SerializeField] protected bool isBranchEntry;
        [SerializeField] protected bool isBranchEnd;
        protected Vector3 beginPosition;
        

        private void Start()
        {
            if(isTest)
            {
                GenerateRoom(roomPrototype).transform.position = generateTransform.position;
            }           
        }

        public void GenerateRoomFromData(DRoomGenerateInfo _data)
        {
            haveLeftWall = _data.haveLeftWall;
            haveUpWall = _data.haveUpWall;
            haveDownWall = _data.haveDownWall;
            haveRightWall = _data.haveRightWall;

            isBranchEntry = _data.isBranchEntry;
            isBranchEnd = _data.isBranchEnd;

            enemyDifficulty = _data.enemyDifficulty;

            type = _data.roomType;

            roomPrototype = SelectRoomPrototype(_data);
      
            generateTransform.position = _data.roomIndex * roomSize;
            GameObject room = GenerateRoom(roomPrototype);
            room.transform.position = _data.roomIndex * roomSize;
        }

        #region Room Data Calculate
        internal enum ERoomDirection
        {
            Horizon,  // 水平方向房间（上下有墙）
            Cross     // 十字/拐角/三岔路口房间
        }
        protected GameObject SelectRoomPrototype(DRoomGenerateInfo roomInfo)
        {
            List<GameObject> selectedList = DetermineDirectionType(roomInfo);

            // 从列表中随机选择一个 GameObject
            if (selectedList != null && selectedList.Count > 0)
            {
                return selectedList[UnityEngine.Random.Range(0, selectedList.Count)];
            }

            Debug.LogWarning($"No room prototype found for type: {roomInfo.roomType}");
            return null;
        }
        private List<GameObject> DetermineDirectionType(DRoomGenerateInfo roomInfo)
        {
            bool hasUpDownWalls = roomInfo.haveUpWall && roomInfo.haveDownWall;
            bool hasLeftRightWalls = roomInfo.haveLeftWall && roomInfo.haveRightWall;

            // 同时有上下墙 → Horizon（水平方向房间）
            if (hasUpDownWalls)
            {
                return horizonRooms;
            }
            // 其他情况（拐角、三岔路口、十字路口）→ Cross
            else
            {
                return crossRooms;
            }
        }
        #endregion

        protected GameObject GenerateRoom(GameObject prototypeRoomPrefab)
        {
            if (prototypeRoomPrefab == null)
            {
                Debug.LogError("原型房间预制体为空！");
                return null;
            }

            // 实例化房间
            GameObject actualRoom = Instantiate(prototypeRoomPrefab);

            // 获取三个Tilemap组件
            Tilemap groundTilemap = FindTilemapInChildren(actualRoom, groundTilemapName);
            Tilemap backgroundTilemap = FindTilemapInChildren(actualRoom, backgroundTilemapName);
            Tilemap platformTilemap = FindTilemapInChildren(actualRoom, platformTilemapName);

            IRoomGeneratorComponents[] generateComponents =
                actualRoom.GetComponentsInChildren<IRoomGeneratorComponents>();
            foreach(var cop in generateComponents)
            {
                cop.Generate(this);
            }

            // 替换各层Tile
            if(groundTilemap != null)
            {
                ClearWallsInGroundBasedOnConfig(groundTilemap);
                ReplaceTilemapTiles(groundTilemap, groundTiles, true);
            }
            else
            {
                Debug.LogWarning("缺少Ground层");
            }
            if (backgroundTilemap != null)
            {
                ReplaceTilemapTiles(backgroundTilemap, backgroundTiles);
            }
            else
            {
                Debug.LogWarning("缺少Background层");
            }
            if(platformTilemap != null)
            {
                ReplaceTilemapTiles(platformTilemap, platformTiles);
            }
            else
            {
                Debug.LogWarning("缺少Platform层");
            }

            List<Vector3> positions = FindValidWorldPositions(groundTilemap, entityHeight, entityRadius, generateTransform);
            if(type == ERoomType.Passage)
            {
                GenerateEnemy(enemyDifficulty, enemys, positions);
            }
            if (type == ERoomType.Entry)
            {
                GenerateEntryDoor(positions);
            }
            if(type == ERoomType.Exit)
            {
                GenerateExit(positions);
            }
            if (type == ERoomType.Event)
            {
                GenerateEvent(positions);
            }
            if(isBranchEntry || isBranchEnd || type == ERoomType.Event || type == ERoomType.Entry || type == ERoomType.Exit)
            {
                GenerateDeliverPoints(positions);
            }

            GenerateDecorations(actualRoom, positions);
            return actualRoom;
        }

        #region Room Entity Generate
        protected void GenerateEnemy(float _difficulty, List<GameObject> _enemys, List<Vector3> _positions)
        {
            if (_positions.Count == 0)
            {
                Debug.LogWarning("房间无可用位置");
                return;
            }
            List<Vector3> positionsTemp = new(_positions);
            MMapGenerateManager manager = GetComponent<MMapGenerateManager>();

            float diff = 0;
            while(diff < _difficulty)
            {
                if (positionsTemp.Count == 0)
                {
                    Debug.LogWarning("位置不足无法继续生成敌人");
                    return;
                }

                Vector3 position = positionsTemp[UnityEngine.Random.Range(0, positionsTemp.Count)];
                IMapEnemy enemy = enemys[UnityEngine.Random.Range(0, enemys.Count)].GetComponent<IMapEnemy>();

                manager.GenerateEnemyAt(enemy.CheckType(), position);
                diff += enemy.CheckDifficulty();

                positionsTemp.Remove(position);
            }
        }

        protected void GenerateDecorations(GameObject _actualRoom, List<Vector3> _positions)
        {
            if (_positions.Count == 0)
            {
                Debug.LogWarning("房间无可用位置");
            }
            List<Vector3> positionsTemp = new(_positions);
            int decorationCount = _actualRoom.GetComponentInChildren<CRoomInfo>().CheckDecorationCount();
            for(int i = 0; i < decorationCount; ++i)
            {
                if(positionsTemp.Count == 0)
                {
                    Debug.LogWarning("位置不足无法继续生成装饰，已经生成：" + i + "个装饰");
                    return;
                }

                Vector3 position = positionsTemp[UnityEngine.Random.Range(0, positionsTemp.Count)];
                GameObject decoGameObject = Instantiate(decorationPrefab, position, Quaternion.identity);
                decoGameObject.GetComponent<SpriteRenderer>().sprite = 
                    decoSpriteList[UnityEngine.Random.Range(0, decoSpriteList.Count)];
                positionsTemp.Remove(position);
                positionsTemp.Remove(position + Vector3.left);
                positionsTemp.Remove(position + Vector3.right);
            }
        }

        protected void GenerateEvent(List<Vector3> _positions)
        {
            if (_positions.Count == 0)
            {
                Debug.LogWarning("房间无可用位置");
                return;
            }
            Vector3 position = _positions[UnityEngine.Random.Range(0, _positions.Count)];
            _positions.Remove(position);
            _positions.Remove(position + Vector3.left);
            _positions.Remove(position + Vector3.right);
            MMapGenerateManager manager = GetComponent<MMapGenerateManager>();

            EEventType eventType = SelectEventByWeight();
            switch(eventType)
            {
                case EEventType.Witch:
                    manager.GenerateNPCAt(ENPCType.Witch, position);
                    break;
                case EEventType.Trader:
                    manager.GenerateNPCAt(ENPCType.Trader, position);
                    break;
                case EEventType.BlackSmith:
                    manager.GenerateNPCAt(ENPCType.BlackSmith, position);
                    break;
                case EEventType.RewardBox:
                    manager.GenerateRewardBoxAt(GetRandomRewardItems(false), GetRandomCoin(false), position, false);
                    break;
                case EEventType.AdvancedRewardBox:
                    manager.GenerateRewardBoxAt(GetRandomRewardItems(true), GetRandomCoin(true), position, true);
                    break;
            }

            foreach (var eventRate in events)
            {
                if (eventRate.type == eventType && eventRate.isSingle)
                {
                    eventRate.weightDefault = eventRate.weight;
                    eventRate.weight = 0;
                }
            }
        }
        protected List<IItemData> GetRandomRewardItems(bool _isAdvanced)
        {
            int count;
            List<ScriptableObject> sourceList;
            if (!_isAdvanced)
            {
                count = UnityEngine.Random.Range((int)rewardItemCount.x, (int)rewardItemCount.y + 1);
                count = Mathf.Min(count, rewardItems.Count);
                sourceList = rewardItems;
            }
            else
            {
                count = UnityEngine.Random.Range((int)advancedRewardsCount.x, (int)advancedRewardsCount.y + 1);
                count = Mathf.Min(count, advancedRewardItems.Count);
                sourceList = advancedRewardItems;
            }

            // Fisher-Yates 洗牌算法打乱列表（只在副本上操作）
            for (int i = 0; i < sourceList.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, sourceList.Count);
                (sourceList[i], sourceList[randomIndex]) = (sourceList[randomIndex], sourceList[i]);
            }

            // 取前 count 个并转换为 IItemData
            List<IItemData> result = new List<IItemData>();
            for (int i = 0; i < count; i++)
            {
                if (sourceList[i] is IItemData itemData)
                {
                    result.Add(itemData);
                }
            }

            return result;
        }
        protected float GetRandomCoin(bool _isAdvanced)
        {
            if(!_isAdvanced)
            {
                return UnityEngine.Random.Range(rewardCoinCount.x, rewardCoinCount.y);
            }
            else
            {
                return UnityEngine.Random.Range(advancedRewardCoinCount.x, advancedRewardCoinCount.y);
            }
        }
        protected EEventType SelectEventByWeight()
        {
            if (events == null || events.Count == 0)
            {
                Debug.LogWarning("Events list is empty or null");
                return default(EEventType);
            }

            // 计算总权重
            float totalWeight = 0f;
            foreach (var eventRate in events)
            {
                totalWeight += eventRate.weight;
            }

            if (totalWeight <= 0f)
            {
                Debug.LogWarning("Total weight is zero or negative");
                return default(EEventType);
            }

            // 随机选择
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var eventRate in events)
            {
                currentWeight += eventRate.weight;
                if (randomValue <= currentWeight)
                {
                    return eventRate.type;
                }
            }

            // 理论上不会到这里，但为了防止万一，返回第一个
            return events[0].type;
        }

        protected void GenerateDeliverPoints(List<Vector3> _positions)
        {           
            if(_positions.Count == 0)
            {
                Debug.LogWarning("房间无可用位置");
                return;
            }
            Vector3 position = _positions[UnityEngine.Random.Range(0, _positions.Count)];
            _positions.Remove(position);
            GetComponent<MMapGenerateManager>().GenerateDeliverPointAt(position);
        }

        protected void GenerateEntryDoor(List<Vector3> _positions)
        {
            if (_positions.Count == 0)
            {
                Debug.LogWarning("房间无可用位置");
                return;
            }
            Vector3 position = _positions[UnityEngine.Random.Range(0, _positions.Count)];
            _positions.Remove(position);
            _positions.Remove(position + Vector3.left);
            _positions.Remove(position + 2 * Vector3.left);
            _positions.Remove(position + Vector3.right);
            _positions.Remove(position + 2 * Vector3.right);
            Instantiate(entryDoorPrefab, position, Quaternion.identity);
            beginPosition = position;
        }

        protected void GenerateExit(List<Vector3> _positions)
        {
            if (_positions.Count == 0)
            {
                Debug.LogWarning("房间无可用位置");
                return;
            }
            Vector3 position = _positions[UnityEngine.Random.Range(0, _positions.Count)];
            _positions.Remove(position);
            _positions.Remove(position + Vector3.left);
            _positions.Remove(position + 2 * Vector3.left);
            _positions.Remove(position + Vector3.right);
            _positions.Remove(position + 2 * Vector3.right);
            GetComponent<MMapGenerateManager>().GenerateSceneSwitchEntry(position);
        }

        public List<Vector3> FindValidWorldPositions(Tilemap tilemap, int _upVoidCount, int _checkRadius, Transform _generateTransform)
        {
            List<Vector3> validWorldPositions = new List<Vector3>();

            tilemap.CompressBounds();
            // 获取Tilemap的边界
            BoundsInt bounds = tilemap.cellBounds;

            // 遍历所有格子
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin + boundWallThickness; y < bounds.yMax; y++)
                {
                    Vector3Int currentCellPos = new Vector3Int(x, y, 0);
                    Vector3Int belowPos = new Vector3Int(x, y - 1, 0);

                    // 条件1：下方格子有tile，且向上n格没有tile
                    if (IsBelowHasTile(tilemap, belowPos) && IsUpwardEmpty(tilemap, currentCellPos, _upVoidCount))
                    {
                        // 条件2：左右m个格子都符合条件1
                        if (CheckHorizontalRange(tilemap, currentCellPos, _upVoidCount, _checkRadius))
                        {
                            // 转换为世界坐标：x为格子中心，y为格子底部
                            Vector3 worldPos = GetWorldPositionAtBottom(tilemap, currentCellPos);
                            worldPos += _generateTransform.position;
                            validWorldPositions.Add(worldPos);
                        }
                    }
                }
            }

            return validWorldPositions;
        }
        private Vector3 GetWorldPositionAtBottom(Tilemap tilemap, Vector3Int cellPos)
        {
            // 获取格子中心的世界坐标
            Vector3 cellCenterWorld = tilemap.GetCellCenterWorld(cellPos);

            // 获取格子大小
            Vector3 cellSize = tilemap.cellSize;

            // 计算格子底部中心坐标（中心Y坐标减去一半的高度）
            Vector3 bottomCenter = new Vector3(
                cellCenterWorld.x,
                cellCenterWorld.y - cellSize.y * 0.5f,
                cellCenterWorld.z
            );

            return bottomCenter;
        }
        private bool IsBelowHasTile(Tilemap tilemap, Vector3Int belowPos)
        {
            return tilemap.HasTile(belowPos);
        }
        private bool IsUpwardEmpty(Tilemap tilemap, Vector3Int startPos, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Vector3Int checkPos = new Vector3Int(startPos.x, startPos.y + i, 0);
                if (tilemap.HasTile(checkPos))
                {
                    return false; // 发现tile，不符合条件
                }
            }
            return true; // 所有格子都为空
        }
        private bool CheckHorizontalRange(Tilemap tilemap, Vector3Int centerPos, int n, int m)
        {
            for (int offset = -m; offset <= m; offset++)
            {
                if (offset == 0) continue; // 跳过中心位置本身

                Vector3Int checkPos = new Vector3Int(centerPos.x + offset, centerPos.y, 0);
                Vector3Int belowPos = new Vector3Int(checkPos.x, checkPos.y - 1, 0);

                // 检查该位置是否满足条件1
                if (!IsBelowHasTile(tilemap, belowPos) || !IsUpwardEmpty(tilemap, checkPos, n))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Tile Replacement
        private void ClearWallsInGroundBasedOnConfig(Tilemap groundTilemap)
        {
            // 获取边界
            groundTilemap.CompressBounds();
            BoundsInt bounds = groundTilemap.cellBounds;

            int clearedCount = 0;

            // 定义要清除的区域（墙壁区域，不包括最外层边界）
            List<Vector3Int> positionsToClear = new List<Vector3Int>();

            // 墙壁起始位置（边界内侧）
            int wallStartX = bounds.xMin + 1;
            int wallEndX = bounds.xMax - 1;
            int wallStartY = bounds.yMin + 1;
            int wallEndY = bounds.yMax - 1;

            // 1. 清除左边墙壁（如果左边墙壁不存在）
            if (!haveLeftWall)
            {
                for (int y = wallStartY + leftRightHight; y < wallEndY - boundWallThickness; y++)
                {
                    for (int thickness = 0; thickness < boundWallThickness; thickness++)
                    {
                        int x = wallStartX + thickness;
                        // 确保不超出边界内侧范围
                        if (x < wallEndX)
                        {
                            Vector3Int pos = new Vector3Int(x, y, 0);
                            if (groundTilemap.HasTile(pos))
                            {
                                positionsToClear.Add(pos);
                            }
                        }
                    }
                }
            }

            // 2. 清除右边墙壁（如果右边墙壁不存在）
            if (!haveRightWall)
            {
                for (int y = wallStartY + leftRightHight; y < wallEndY - boundWallThickness; y++)
                {
                    for (int thickness = 0; thickness < boundWallThickness; thickness++)
                    {
                        int x = wallEndX - 1 - thickness;
                        // 确保不超出边界内侧范围
                        if (x >= wallStartX)
                        {
                            Vector3Int pos = new Vector3Int(x, y, 0);
                            if (groundTilemap.HasTile(pos))
                            {
                                positionsToClear.Add(pos);
                            }
                        }
                    }
                }
            }

            // 3. 清除上边墙壁（如果上边墙壁不存在）
            if (!haveUpWall)
            {
                for (int x = wallStartX + boundWallThickness; x < wallEndX - boundWallThickness; x++)
                {
                    for (int thickness = 0; thickness < boundWallThickness; thickness++)
                    {
                        int y = wallEndY - 1 - thickness;
                        // 确保不超出边界内侧范围
                        if (y >= wallStartY)
                        {
                            Vector3Int pos = new Vector3Int(x, y, 0);
                            if (groundTilemap.HasTile(pos))
                            {
                                positionsToClear.Add(pos);
                            }
                        }
                    }
                }
            }

            // 4. 清除下边墙壁（如果下边墙壁不存在）
            if (!haveDownWall)
            {
                for (int x = wallStartX + boundWallThickness; x < wallEndX - boundWallThickness; x++)
                {
                    for (int thickness = 0; thickness < boundWallThickness; thickness++)
                    {
                        int y = wallStartY + thickness;
                        // 确保不超出边界内侧范围
                        if (y < wallEndY)
                        {
                            Vector3Int pos = new Vector3Int(x, y, 0);
                            if (groundTilemap.HasTile(pos))
                            {
                                positionsToClear.Add(pos);
                            }
                        }
                    }
                }
            }

            // 执行清除
            foreach (Vector3Int pos in positionsToClear)
            {
                groundTilemap.SetTile(pos, null);
                clearedCount++;
            }

            // 去重（虽然List可能有重复位置，但SetTile多次设置为null也没问题）
            if (clearedCount > 0)
            {
                Debug.Log($"根据墙壁配置清除了 {clearedCount} 个墙壁Tile");
            }

            groundTilemap.RefreshAllTiles();
        }
        private void ReplaceTilemapTiles(Tilemap tilemap, List<DTile> availableTiles, bool hasBorder = false)
        {
            if (availableTiles == null || availableTiles.Count == 0)
            {
                Debug.LogWarning($"Tilemap {tilemap.name} 没有可用的替换Tile列表");
                return;
            }

            // 获取Tilemap边界
            tilemap.CompressBounds();
            BoundsInt bounds = tilemap.cellBounds;

            // 收集所有需要替换的Tile位置（边框除外）
            List<Vector3Int> positionsToReplace = new List<Vector3Int>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (tilemap.HasTile(pos))
                    {
                        // 如果是Ground层且有边框，跳过边框位置（不加入替换列表）
                        if (hasBorder && IsBorderPosition(x, y, bounds))
                        {
                            continue;
                        }
                        // 背景只替换指定tile
                        if(tilemap.name == backgroundTilemapName && tilemap.GetTile(pos) != backgroundReplaceTile)
                        {
                            continue;
                        }
                        positionsToReplace.Add(pos);
                    }
                }
            }

            // 按x+y排序，从左上到右下依次处理，确保邻居信息尽可能可用
            positionsToReplace.Sort((a, b) =>
            {
                int sumA = a.x + a.y;
                int sumB = b.x + b.y;
                if (sumA != sumB) return sumA.CompareTo(sumB);
                return a.x.CompareTo(b.x);
            });

            // 存储已放置的DTile，供后续位置查询
            Dictionary<Vector3Int, DTile> placedTiles = new Dictionary<Vector3Int, DTile>();

            // 处理每个需要替换的位置
            foreach (Vector3Int pos in positionsToReplace)
            {
                // 获取原型邻居信息（边框视为存在）
                bool[] prototypeNeighbors = GetPrototypeNeighbors(tilemap, pos);

                // 获取实际已放置的邻居DTile
                DTile[] neighborTiles = GetNeighborDTiles(tilemap, pos, placedTiles);

                // 为当前位置选择合适的DTile
                DTile selectedTile = SelectTileForPosition(availableTiles, neighborTiles, prototypeNeighbors, pos, placedTiles);

                if (selectedTile != null)
                {
                    // 立即放置Tile
                    tilemap.SetTile(pos, selectedTile.tileBase);
                    // 记录已放置的DTile信息
                    placedTiles[pos] = selectedTile;
                }
                else
                {
                    Debug.LogWarning($"位置 {pos} 没有找到符合规则的Tile，保持原有Tile");
                    // 保持原有Tile，并记录为null
                    placedTiles[pos] = null;
                }
            }

            // 清除边框位置（仅Ground层）
            if (hasBorder)
            {
                ClearBorderTiles(tilemap, bounds);
            }

            tilemap.RefreshAllTiles();
        }
        private bool IsBorderPosition(int x, int y, BoundsInt bounds)
        {
            return x == bounds.xMin || x == bounds.xMax - 1 ||
                   y == bounds.yMin || y == bounds.yMax - 1;
        }
        private void ClearBorderTiles(Tilemap tilemap, BoundsInt bounds)
        {
            int clearedCount = 0;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (IsBorderPosition(x, y, bounds) && tilemap.HasTile(pos))
                    {
                        tilemap.SetTile(pos, null);
                        clearedCount++;
                    }
                }
            }

            if (clearedCount > 0)
            {
                Debug.Log($"清除了 {clearedCount} 个Ground边框tile");
            }
        }
        private DTile SelectTileForPosition(List<DTile> availableTiles, DTile[] neighborTiles, bool[] prototypeNeighbors, Vector3Int position, Dictionary<Vector3Int, DTile> placedTiles)
        {
            // 过滤出所有符合规则的Tile
            List<DTile> validTiles = new List<DTile>();

            foreach (DTile dtile in availableTiles)
            {
                // 第一步：检查自身的规则是否满足
                if (!dtile.CanPlace(neighborTiles, prototypeNeighbors))
                {
                    continue;
                }

                //第二步：检查已放置的邻居的规则是否会因为放置这个Tile而被违反
                if (!CheckPlacedNeighborRulesForPosition(dtile, position, placedTiles))
                {
                    continue;
                }

                validTiles.Add(dtile);
            }

            // 从符合规则的Tile中随机选择一个
            if (validTiles.Count > 0)
            {
                return validTiles[UnityEngine.Random.Range(0, validTiles.Count)];
            }

            return null;
        }
        private bool[] GetPrototypeNeighbors(Tilemap tilemap, Vector3Int center)
        {
            bool[] neighbors = new bool[8];

            Vector3Int[] offsets = new Vector3Int[]
            {
            new Vector3Int(-1, 1, 0),  // 左上
            new Vector3Int(0, 1, 0),   // 上
            new Vector3Int(1, 1, 0),   // 右上
            new Vector3Int(-1, 0, 0),  // 左
            new Vector3Int(1, 0, 0),   // 右
            new Vector3Int(-1, -1, 0), // 左下
            new Vector3Int(0, -1, 0),  // 下
            new Vector3Int(1, -1, 0)   // 右下
            };

            for (int i = 0; i < 8; i++)
            {
                neighbors[i] = tilemap.HasTile(center + offsets[i]);
            }

            return neighbors;
        }
        private DTile[] GetNeighborDTiles(Tilemap tilemap, Vector3Int center, Dictionary<Vector3Int, DTile> placedTiles)
        {
            DTile[] neighborTiles = new DTile[8];

            Vector3Int[] offsets = new Vector3Int[]
            {
            new Vector3Int(-1, 1, 0),  // 左上
            new Vector3Int(0, 1, 0),   // 上
            new Vector3Int(1, 1, 0),   // 右上
            new Vector3Int(-1, 0, 0),  // 左
            new Vector3Int(1, 0, 0),   // 右
            new Vector3Int(-1, -1, 0), // 左下
            new Vector3Int(0, -1, 0),  // 下
            new Vector3Int(1, -1, 0)   // 右下
            };

            for (int i = 0; i < 8; i++)
            {
                Vector3Int neighborPos = center + offsets[i];

                // 优先从已放置的字典中获取
                if (placedTiles.TryGetValue(neighborPos, out DTile placedTile))
                {
                    neighborTiles[i] = placedTile;
                }
                // 如果还没放置，检查Tilemap中是否已有Tile（可能是原始Tile或已放置的）
                else if (tilemap.HasTile(neighborPos))
                {
                    // 如果位置有Tile但还没记录在字典中，说明还没处理到
                    // 返回null，表示邻居尚未生成
                    neighborTiles[i] = null;
                }
                else
                {
                    neighborTiles[i] = null;
                }
            }

            return neighborTiles;
        }
        private Tilemap FindTilemapInChildren(GameObject parent, string name)
        {
            Tilemap[] tilemaps = parent.GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tilemaps)
            {                
                if (tilemap != null && tilemap.name == name)
                {
                    return tilemap;
                }
            }
            return null;
        }
        private bool CheckPlacedNeighborRulesForPosition(DTile candidateTile, Vector3Int center, Dictionary<Vector3Int, DTile> placedTiles)
        {
            Vector3Int[] offsets = new Vector3Int[]
            {
        new Vector3Int(-1, 1, 0),  // 左上
        new Vector3Int(0, 1, 0),   // 上
        new Vector3Int(1, 1, 0),   // 右上
        new Vector3Int(-1, 0, 0),  // 左
        new Vector3Int(1, 0, 0),   // 右
        new Vector3Int(-1, -1, 0), // 左下
        new Vector3Int(0, -1, 0),  // 下
        new Vector3Int(1, -1, 0)   // 右下
            };

            // 遍历8个邻居方向
            for (int i = 0; i < 8; i++)
            {
                Vector3Int neighborPos = center + offsets[i];

                // 只检查已经放置的邻居
                if (!placedTiles.TryGetValue(neighborPos, out DTile neighborTile))
                {
                    continue; // 未放置的邻居跳过
                }

                // 如果邻居Tile为空（表示保持了原有Tile但没有DTile信息），也跳过
                if (neighborTile == null)
                {
                    continue;
                }

                if (!neighborTile.CanNeighborPlace(candidateTile, 7 - i))
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        public bool IsAnyKeyInput()
        {
            return GetComponent<MMapGenerateManager>().IsAnyKeyInput();
        }

        public Vector3 CheckEntryRoomBeginPosition()
        {
            return beginPosition;
        }
    }

    interface IRoomGeneratorComponents
    {
        public void Generate(CRoomGenerater _generator);
    }
}