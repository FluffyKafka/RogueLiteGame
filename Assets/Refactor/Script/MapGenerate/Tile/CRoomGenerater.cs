using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [Header("Test")]
        [SerializeField] protected bool isTest = false;
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

            // 替换各层Tile
            if(groundTilemap != null)
            {
                ReplaceTilemapTiles(groundTilemap, groundTiles);
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
            return actualRoom;
        }

        private void ReplaceTilemapTiles(Tilemap tilemap, List<DTile> availableTiles)
        {
            if (availableTiles == null || availableTiles.Count == 0)
            {
                Debug.LogWarning($"Tilemap {tilemap.name} 没有可用的替换Tile列表");
                return;
            }

            // 获取所有需要替换的Tile位置并排序，确保生成顺序的一致性
            BoundsInt bounds = tilemap.cellBounds;
            List<Vector3Int> allPositions = new List<Vector3Int>();

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (tilemap.HasTile(pos))
                    {
                        allPositions.Add(pos);
                    }
                }
            }

            // 按x+y排序，从左上到右下依次处理，确保邻居信息尽可能可用
            allPositions.Sort((a, b) =>
            {
                int sumA = a.x + a.y;
                int sumB = b.x + b.y;
                if (sumA != sumB) return sumA.CompareTo(sumB);
                return a.x.CompareTo(b.x);
            });

            // 存储已放置的DTile，供后续位置查询
            Dictionary<Vector3Int, DTile> placedTiles = new Dictionary<Vector3Int, DTile>();

            // 一次遍历：依次处理每个位置
            foreach (Vector3Int pos in allPositions)
            {
                // 获取原型邻居信息
                bool[] prototypeNeighbors = GetPrototypeNeighbors(tilemap, pos);
                string bs = "";
                foreach(bool b in prototypeNeighbors)
                {
                    bs += b;
                    bs += ",";
                }
                Debug.Log(bs);

                // 获取实际已放置的邻居DTile
                DTile[] neighborTiles = GetNeighborDTiles(tilemap, pos, placedTiles);

                // 为当前位置选择合适的DTile
                DTile selectedTile = SelectTileForPosition(availableTiles, neighborTiles, prototypeNeighbors);

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
            tilemap.RefreshAllTiles();
        }

        private DTile SelectTileForPosition(List<DTile> availableTiles, DTile[] neighborTiles, bool[] prototypeNeighbors)
        {
            // 过滤出所有符合规则的Tile
            List<DTile> validTiles = new List<DTile>();

            foreach (DTile dtile in availableTiles)
            {
                if (dtile.rule == null)
                {
                    // 如果没有规则，默认允许放置
                    validTiles.Add(dtile);
                    continue;
                }

                if (dtile.rule.CanPlace(neighborTiles, prototypeNeighbors))
                {
                    validTiles.Add(dtile);
                }
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
    }
}

