using EnemySystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace MapGenerate
{
    internal class CRoomGenerater : MonoBehaviour
    {
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
        [SerializeField] protected GameObject decorationPrefab;
        [SerializeField] protected List<Sprite> decoSpriteList;
        [Header("Enemy")]
        [SerializeField] protected float difficulty;
        [SerializeField] protected List<GameObject> enemys;

        [Header("Test")]
        [SerializeField] protected bool isTest = false;
        [SerializeField] protected bool haveLeftWall = true;
        [SerializeField] protected bool haveUpWall = true;
        [SerializeField] protected bool haveDownWall = true;
        [SerializeField] protected bool haveRightWall = true;
        [SerializeField] protected int boundWallThickness = 2;
        [SerializeField] protected int leftRightHight = 4;
        [SerializeField] protected GameObject testPrototype;
        [SerializeField] protected Transform generateTransform;

        

        private void Start()
        {
            if(isTest)
            {
                GenerateRoom(testPrototype).transform.position = generateTransform.position;
            }           
        }

        public GameObject GenerateRoom(GameObject prototypeRoomPrefab)
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
                cop.Generate();
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
            GenerateDecorations(actualRoom, positions);
            GenerateEnemy(difficulty, enemys, positions);

            return actualRoom;
        }

        protected void GenerateEnemy(float _difficulty, List<GameObject> _enemys, List<Vector3> _positions)
        {
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

                Vector3 position = positionsTemp[Random.Range(0, positionsTemp.Count)];
                IMapEnemy enemy = enemys[Random.Range(0, enemys.Count)].GetComponent<IMapEnemy>();

                manager.GenerateEnemyAt(enemy.CheckType(), position);
                diff += enemy.CheckDifficulty();

                positionsTemp.Remove(position);
            }
        }
        protected void GenerateDecorations(GameObject _actualRoom, List<Vector3> _positions)
        {
            List<Vector3> positionsTemp = new(_positions);
            int decorationCount = _actualRoom.GetComponentInChildren<CRoomInfo>().CheckDecorationCount();
            for(int i = 0; i < decorationCount; ++i)
            {
                if(positionsTemp.Count == 0)
                {
                    Debug.LogWarning("位置不足无法继续生成装饰，已经生成：" + i + "个装饰");
                    return;
                }

                Vector3 position = positionsTemp[Random.Range(0, positionsTemp.Count)];
                GameObject decoGameObject = Instantiate(decorationPrefab, position, Quaternion.identity);
                decoGameObject.GetComponent<SpriteRenderer>().sprite = 
                    decoSpriteList[Random.Range(0, decoSpriteList.Count)];
                positionsTemp.Remove(position);
                positionsTemp.Remove(position + Vector3.left);
                positionsTemp.Remove(position + Vector3.right);
            }
        }
        public List<Vector3> FindValidWorldPositions(Tilemap tilemap, int _upVoidCount, int _checkRadius, Transform _generateTransform)
        {
            List<Vector3> validWorldPositions = new List<Vector3>();

            // 获取Tilemap的边界
            BoundsInt bounds = tilemap.cellBounds;

            // 遍历所有格子
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
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

            // 5. 处理拐角区域（墙壁相交处）
            // 注意：拐角区域只清除墙壁厚度范围内的区域，不触碰边界

            // 左上角区域（左墙和上墙的交汇处）
            if (!haveLeftWall && !haveUpWall)
            {
                for (int xOffset = 0; xOffset < boundWallThickness; xOffset++)
                {
                    for (int yOffset = 0; yOffset < boundWallThickness; yOffset++)
                    {
                        int x = wallStartX + xOffset;
                        int y = wallEndY - 1 - yOffset;
                        // 确保在墙壁范围内
                        if (x < wallEndX && y >= wallStartY)
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

            // 右上角区域（右墙和上墙的交汇处）
            if (!haveRightWall && !haveUpWall)
            {
                for (int xOffset = 0; xOffset < boundWallThickness; xOffset++)
                {
                    for (int yOffset = 0; yOffset < boundWallThickness; yOffset++)
                    {
                        int x = wallEndX - 1 - xOffset;
                        int y = wallEndY - 1 - yOffset;
                        // 确保在墙壁范围内
                        if (x >= wallStartX && y >= wallStartY)
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

            // 左下角区域（左墙和下墙的交汇处）
            if (!haveLeftWall && !haveDownWall)
            {
                for (int xOffset = 0; xOffset < boundWallThickness; xOffset++)
                {
                    for (int yOffset = 0; yOffset < boundWallThickness; yOffset++)
                    {
                        int x = wallStartX + xOffset;
                        int y = wallStartY + yOffset;
                        // 确保在墙壁范围内
                        if (x < wallEndX && y < wallEndY)
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

            // 右下角区域（右墙和下墙的交汇处）
            if (!haveRightWall && !haveDownWall)
            {
                for (int xOffset = 0; xOffset < boundWallThickness; xOffset++)
                {
                    for (int yOffset = 0; yOffset < boundWallThickness; yOffset++)
                    {
                        int x = wallEndX - 1 - xOffset;
                        int y = wallStartY + yOffset;
                        // 确保在墙壁范围内
                        if (x >= wallStartX && y < wallEndY)
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
                return validTiles[Random.Range(0, validTiles.Count)];
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
    }

    interface IRoomGeneratorComponents
    {
        public void Generate();
    }
}