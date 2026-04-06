using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace MapGenerate
{
    [CreateAssetMenu(fileName = "New Tile Set Rule", menuName = "Map Generate System/Tile Set Rule")]
    internal abstract class DTileSetRuleBase : ScriptableObject
    {
        public abstract bool CanPlace(DTile[] neighborTiles, bool[] prototypeNeighbors);
        public abstract bool CanNeighborPlace(DTile _neighbor, int _index);
        public abstract bool CanPlace_Prototype(bool[] prototypeNeighbors);
    }
}


