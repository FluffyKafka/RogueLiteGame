using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGenerate;

[ExcelAsset]
public class TileRules : ScriptableObject
{
	public List<DTileSetPrototypeRuleFromExcel> prototypeRule;
	public List<DTileSetTagRuleFromExcel> tagRule; 
}
