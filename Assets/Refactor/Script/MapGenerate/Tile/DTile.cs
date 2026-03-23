using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MapGenerate
{
    [CreateAssetMenu(fileName = "New Tile Data", menuName = "Map Generate System/Tile Data")]
    internal class DTile : ScriptableObject
    {
        public TileBase tileBase;
        public DTileSetRuleBase rule;
        public ETileMapType layer;
        public string tag = string.Empty;
    }
}

