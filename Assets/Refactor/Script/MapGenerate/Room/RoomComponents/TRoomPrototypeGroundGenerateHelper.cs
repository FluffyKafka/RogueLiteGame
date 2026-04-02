using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGenerate
{
    internal class TRoomPrototypeGroundGenerateHelper : MonoBehaviour
    {
        [SerializeField] protected Tilemap map;
        [SerializeField] protected TileBase tile;

        [Header("生成参数")]
        [SerializeField] private Vector2Int mapSize = new Vector2Int(50, 50);
        [SerializeField] private float initialFillProbability = 0.45f;
        [SerializeField] private int randomSeed = 42;

        // 存储当前地图状态
        private bool[,] groundMap;

        private void Awake()
        {
            if (map == null)
                map = GetComponent<Tilemap>();
        }

        [ContextMenu("初始化")]
        public void Initialize()
        {
            if (map == null) return;

            // 初始化地图数组
            groundMap = new bool[mapSize.x, mapSize.y];

            // 设置随机种子
            Random.InitState(randomSeed);

            // 随机填充
            for (int x = 1; x < mapSize.x - 1; x++)
            {
                for (int y = 1; y < mapSize.y - 1; y++)
                {
                    groundMap[x, y] = Random.value < initialFillProbability;
                }
            }

            // 渲染地图
            RenderMap();

            Debug.Log($"初始化完成，填充率: {CalculateFillRate():P2}");
        }

        [ContextMenu("执行填补迭代")]
        public void ExecuteFillIteration()
        {
            if (groundMap == null)
            {
                Debug.LogError("请先初始化地图！");
                return;
            }

            int filledCount = 0;
            bool[,] newMap = (bool[,])groundMap.Clone();

            // 遍历每个位置
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    // 跳过已经是tile的位置
                    if (groundMap[x, y]) continue;

                    // 检查是否符合填补规则：两个tile中间有一个空位置
                    if (ShouldFill(x, y))
                    {
                        newMap[x, y] = true;
                        filledCount++;
                    }
                }
            }

            groundMap = newMap;
            RenderMap();

            Debug.Log($"填补迭代完成，新增 {filledCount} 个tile，当前填充率: {CalculateFillRate():P2}");
        }

        [ContextMenu("执行消除迭代")]
        public void ExecuteEliminateIteration()
        {
            if (groundMap == null)
            {
                Debug.LogError("请先初始化地图！");
                return;
            }

            int eliminatedCount = 0;
            bool[,] newMap = (bool[,])groundMap.Clone();

            // 遍历每个位置
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    // 只检查有tile的位置
                    if (!groundMap[x, y]) continue;

                    // 检查是否符合消除规则：两个空位置中间的tile
                    if (ShouldEliminate(x, y))
                    {
                        newMap[x, y] = false;
                        eliminatedCount++;
                    }
                }
            }

            groundMap = newMap;
            RenderMap();

            Debug.Log($"消除迭代完成，消除 {eliminatedCount} 个tile，当前填充率: {CalculateFillRate():P2}");
        }

        [ContextMenu("清空全部tile")]
        public void ClearAllTiles()
        {
            if (map == null) return;

            map.ClearAllTiles();
            if (groundMap != null)
            {
                System.Array.Clear(groundMap, 0, groundMap.Length);
            }

            Debug.Log("已清空全部tile");
        }

        // 检查是否应该填补位置 (x, y)
        private bool ShouldFill(int x, int y)
        {
            // 检查水平方向：左边和右边都有tile
            if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
            {
                if (groundMap[x - 1, y] && groundMap[x + 1, y])
                    return true;
                if (groundMap[x, y - 1] && groundMap[x, y + 1])
                    return true;
            }

            return false;
        }

        // 检查是否应该消除位置 (x, y)
        private bool ShouldEliminate(int x, int y)
        {
            // 检查水平方向：左右都是空
            if (x > 0 && x < mapSize.x - 1 && y > 0 && y < mapSize.y - 1)
            {
                if ((!groundMap[x - 1, y] && !groundMap[x + 1, y]) || (!groundMap[x, y - 1] && !groundMap[x, y + 1]))
                    return true;
            }

            return false;
        }

        // 渲染地图到Tilemap
        private void RenderMap()
        {
            if (map == null || tile == null) return;

            map.ClearAllTiles();

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (groundMap[x, y])
                    {
                        Vector3Int tilePosition = new Vector3Int(x, y, 0);
                        map.SetTile(tilePosition, tile);
                    }
                }
            }
        }

        // 计算当前填充率（用于调试）
        private float CalculateFillRate()
        {
            if (groundMap == null) return 0;

            int filledCount = 0;
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    if (groundMap[x, y]) filledCount++;
                }
            }

            return (float)filledCount / (mapSize.x * mapSize.y);
        }
    }
}