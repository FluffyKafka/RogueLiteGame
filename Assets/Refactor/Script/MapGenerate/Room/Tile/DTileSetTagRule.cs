using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerate
{
    internal class DTileSetTagRule : DTileSetRuleBase
    {
        [SerializeField] protected List<string> tags;

        public override bool CanNeighborPlace(DTile _neighbor, int _index)
        {
            if (tags[_index] == string.Empty || _neighbor.IsTag(tags[_index]))
            {
                return true;
            }
            return false;
        }

        public override bool CanPlace(DTile[] neighborTiles, bool[] prototypeNeighbors)
        {
            for(int i = 0; i < 8; ++i)
            {
                if (tags[i] != string.Empty && neighborTiles[i] != null && !neighborTiles[i].IsTag(tags[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public void SetUp(DTileSetTagRuleFromExcel excelData)
        {
            if (excelData == null)
            {
                Debug.LogError("excelData ЮЊПе");
                return;
            }

            tags = new List<string>
        {
            excelData.tag_0,
            excelData.tag_1,
            excelData.tag_2,
            excelData.tag_3,
            excelData.tag_4,
            excelData.tag_5,
            excelData.tag_6,
            excelData.tag_7
        };

            this.name = excelData.name;
        }

        public override bool CanPlace_Prototype(bool[] prototypeNeighbors)
        {
            return true;
        }
    }
}

