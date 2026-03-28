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
        public ETileMapType layer;
        [SerializeField] protected List<string> tags;
        [SerializeField] protected List<DTileSetRuleBase> rules;

        public bool CanPlace(DTile[] neighborTiles, bool[] prototypeNeighbors)
        {
            foreach(var rule in rules)
            {
                if (!rule.CanPlace(neighborTiles, prototypeNeighbors))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsTag(string _tag)
        {
            if(tags.Count == 0)
            {
                return true;
            }

            foreach(var tag in tags)
            {
                if(tag == _tag)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CanNeighborPlace(DTile _neighbor, int _index)
        {
            foreach(var rule in rules)
            {
                if(!rule.CanNeighborPlace(_neighbor, _index))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

