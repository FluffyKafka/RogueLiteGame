using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerate
{
    internal class DTileSetPrototypeRule : DTileSetRuleBase
    {
        [SerializeField] protected List<int> prototypeTiles;

        public override bool CanNeighborPlace(DTile _neighbor, int _index)
        {
            return true;
        }

        public override bool CanPlace(DTile[] neighborTiles, bool[] prototypeNeighbors)
        {
            for(int i = 0; i < 8; ++i)
            {
                if(prototypeTiles[i] != 0)
                {
                    if (prototypeTiles[i] < 0 && prototypeNeighbors[i])
                    {
                        return false;
                    }
                    if (prototypeTiles[i] > 0 && !prototypeNeighbors[i])
                    {
                        return false;
                    }
                }             
            }
            return true;
        }

        public void SetUp(DTileSetPrototypeRuleFromExcel excelData)
        {
            if (excelData == null)
            {
                Debug.LogError("excelData ЮЊПе");
                return;
            }

            prototypeTiles = new List<int>
        {
            excelData.tile_0,
            excelData.tile_1,
            excelData.tile_2,
            excelData.tile_3,
            excelData.tile_4,
            excelData.tile_5,
            excelData.tile_6,
            excelData.tile_7
        };

            this.name = excelData.name;
        }
    }
}

