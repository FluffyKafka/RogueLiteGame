using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Linq;

namespace MapGenerate
{
    internal class TRoomShootGenerate : MonoBehaviour
    {
        [SerializeField] protected int shootTime = 50;
        [SerializeField] protected TileBase tileToPlace; // 要放置的Tile
        [SerializeField] protected Tilemap targetTilemap; // 目标Tilemap组件

        protected BoxCollider2D area;
        protected HashSet<Vector3Int> placedTilePositions = new HashSet<Vector3Int>();

        // 四个方向：上、下、左、右
        private readonly Vector3Int[] directions = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

        protected virtual void Awake()
        {
            area = GetComponent<BoxCollider2D>();
            if (area == null)
            {
                Debug.LogError("需要BoxCollider2D组件来定义区域");
            }

            if (targetTilemap == null)
            {
                targetTilemap = GetComponent<Tilemap>();
                if (targetTilemap == null)
                {
                    Debug.LogError("需要Tilemap组件或指定目标Tilemap");
                }
            }
        }

        [ContextMenu("生成房间")]
        protected void GenerateRoom()
        {
            area = GetComponent<BoxCollider2D>();
            if (area == null)
            {
                Debug.LogError("区域未定义");
                return;
            }

            if (tileToPlace == null)
            {
                Debug.LogError("未指定要放置的Tile");
                return;
            }

            if (targetTilemap == null)
            {
                Debug.LogError("未指定目标Tilemap");
                return;
            }

            ClearAreaTiles();

            if (shootTime <= 0)
            {
                Debug.LogWarning("射击次数为0，不生成任何tile");
                return;
            }

            // 获取区域内所有格子位置
            List<Vector3Int> availablePositions = GetAvailablePositionsInArea();

            if (availablePositions.Count == 0)
            {
                Debug.LogWarning("区域内没有可用格子");
                return;
            }

            // 随机选择第一个位置放置tile
            Vector3Int startPos = availablePositions[Random.Range(0, availablePositions.Count)];
            PlaceTile(startPos);

            // 执行shootTime轮射击
            for (int i = 0; i < shootTime; i++)
            {
                ShootAndPlaceTile();
            }

            Debug.Log($"生成完成，共放置了 {placedTilePositions.Count} 个tile");
        }

        protected void ShootAndPlaceTile()
        {
            if (placedTilePositions.Count == 0) return;

            // 从已放置的tile中随机选择一个作为射击起点
            List<Vector3Int> tilePositions = placedTilePositions.ToList();
            Vector3Int startPos = tilePositions[Random.Range(0, tilePositions.Count)];

            // 随机选择一个方向
            Vector3Int direction = directions[Random.Range(0, directions.Length)];

            // 从起点开始沿方向射击
            Vector3Int currentPos = startPos + direction;

            while (IsPositionInArea(currentPos))
            {
                // 如果这个位置没有tile，就在这里放置并结束射击
                if (!placedTilePositions.Contains(currentPos))
                {
                    PlaceTile(currentPos);
                    return;
                }
                // 如果已经有tile，继续向前射击
                currentPos += direction;
            }

            // 射击未命中任何可放置位置，本次射击无效
        }

        protected void PlaceTile(Vector3Int tilePos)
        {
            if (placedTilePositions.Contains(tilePos))
            {
                return;
            }

            targetTilemap.SetTile(tilePos, tileToPlace);
            placedTilePositions.Add(tilePos);
        }

        protected List<Vector3Int> GetAvailablePositionsInArea()
        {
            List<Vector3Int> positions = new List<Vector3Int>();

            if (area == null || targetTilemap == null) return positions;

            Bounds bounds = area.bounds;

            // 获取Tilemap的边界
            Vector3Int cellBoundsMin = targetTilemap.WorldToCell(bounds.min);
            Vector3Int cellBoundsMax = targetTilemap.WorldToCell(bounds.max);

            for (int x = cellBoundsMin.x; x <= cellBoundsMax.x; x++)
            {
                for (int y = cellBoundsMin.y; y <= cellBoundsMax.y; y++)
                {
                    Vector3Int cellPos = new Vector3Int(x, y, 0);
                    if (IsPositionInArea(cellPos))
                    {
                        positions.Add(cellPos);
                    }
                }
            }

            return positions;
        }

        protected bool IsPositionInArea(Vector3Int cellPos)
        {
            if (area == null || targetTilemap == null) return false;

            Vector3 worldPos = targetTilemap.GetCellCenterWorld(cellPos);
            return area.OverlapPoint(worldPos);
        }

        [ContextMenu("清除区域内tile")]
        protected void Clear()
        {
            ClearAreaTiles();
        }

        protected void ClearAllTiles()
        {
            foreach (var tilePos in placedTilePositions)
            {
                if (targetTilemap != null)
                {
                    targetTilemap.SetTile(tilePos, null);
                }
            }
            placedTilePositions.Clear();
        }

        protected void ClearAreaTiles()
        {
            List<Vector3Int> tilesToRemove = new List<Vector3Int>();

            foreach (var tilePos in placedTilePositions)
            {
                if (IsPositionInArea(tilePos))
                {
                    tilesToRemove.Add(tilePos);
                }
            }

            foreach (var tilePos in tilesToRemove)
            {
                if (targetTilemap != null)
                {
                    targetTilemap.SetTile(tilePos, null);
                }
                placedTilePositions.Remove(tilePos);
            }

            Debug.Log($"清除了 {tilesToRemove.Count} 个区域内的tile");
        }
    }
}