using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGenerate
{
    internal class CTileDataGenerater : MonoBehaviour
    {
        [SerializeField]
        private string tileFolderPath = "Assets/Refactor/Tiles/Tiles/Medieval_Castle"; // Tile所在的文件位置

        [SerializeField]
        private string outputFolderPath = "Assets/Refactor/Tiles/TileData"; // DTile输出位置

        [SerializeField]
        private bool overwriteExisting = false; // 是否覆盖已存在的DTile

#if UNITY_EDITOR
        [ContextMenu("Generate DTiles")]
        public void GenerateDTiles()
        {
            if (string.IsNullOrEmpty(tileFolderPath))
            {
                Debug.LogError("Tile文件夹路径未设置！");
                return;
            }

            // 确保输出文件夹存在
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
                AssetDatabase.Refresh();
            }

            // 获取所有Tile资源
            string[] tileGuids = AssetDatabase.FindAssets("t:TileBase", new[] { tileFolderPath });
            List<TileBase> tiles = new List<TileBase>();

            foreach (string guid in tileGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
                if (tile != null)
                {
                    tiles.Add(tile);
                }
            }

            if (tiles.Count == 0)
            {
                Debug.LogWarning($"在路径 {tileFolderPath} 中没有找到任何Tile资源");
                return;
            }

            // 为每个Tile创建DTile
            List<DTile> createdDTiles = new List<DTile>();
            foreach (TileBase tile in tiles)
            {
                DTile dtile = CreateDTileFromTile(tile);
                if (dtile != null)
                {
                    createdDTiles.Add(dtile);
                }
            }

            Debug.Log($"成功创建了 {createdDTiles.Count} 个DTile，共处理了 {tiles.Count} 个Tile");

            // 刷新资源数据库
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 从单个Tile创建DTile
        /// </summary>
        private DTile CreateDTileFromTile(TileBase tile)
        {
            if (tile == null)
            {
                Debug.LogError("Tile为空，无法创建DTile");
                return null;
            }

            // 获取Tile的资产路径
            string tilePath = AssetDatabase.GetAssetPath(tile);
            string tileName = Path.GetFileNameWithoutExtension(tilePath);

            // 构建DTile的保存路径
            string dtilePath = Path.Combine(outputFolderPath, $"{tileName}_DTile.asset");

            // 检查是否已存在
            if (!overwriteExisting && File.Exists(dtilePath))
            {
                Debug.Log($"DTile已存在，跳过创建: {dtilePath}");
                return AssetDatabase.LoadAssetAtPath<DTile>(dtilePath);
            }

            // 创建DTile实例
            DTile dtile = ScriptableObject.CreateInstance<DTile>();

            // 设置属性
            dtile.tileBase = tile;

            // 保存为资源文件
            try
            {
                // 确保目录存在
                string directory = Path.GetDirectoryName(dtilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 创建资源文件
                AssetDatabase.CreateAsset(dtile, dtilePath);
                Debug.Log($"成功创建DTile: {dtilePath}");

                return dtile;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"创建DTile失败: {tileName}\n错误: {e.Message}");
                return null;
            }
        }
#endif
    }
}

