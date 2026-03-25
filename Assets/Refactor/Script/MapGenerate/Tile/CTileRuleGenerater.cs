using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGenerate
{
    internal class CTileRuleGenerater : MonoBehaviour
    {
        [SerializeField] protected TileRules rules;
        [SerializeField] protected string tagRulePath;
        [SerializeField] protected string prototypeRulePath;

        [ContextMenu("Generate All Rules")]
        public void GenerateAndSaveRules()
        {
#if UNITY_EDITOR
            GeneratePrototypeRules();
            GenerateTagRules();
#endif
        }

        [ContextMenu("Generate Prototype Rules")]
        public void GeneratePrototypeRules()
        {
#if UNITY_EDITOR
            if (rules.prototypeRule == null) return;

            foreach (var prototypeRuleData in rules.prototypeRule)
            {
                string path = System.IO.Path.Combine(prototypeRulePath, $"{prototypeRuleData.name}.asset");
                DTileSetPrototypeRule existingRule = UnityEditor.AssetDatabase.LoadAssetAtPath<DTileSetPrototypeRule>(path);

                if (existingRule != null)
                {
                    existingRule.SetUp(prototypeRuleData);
                    UnityEditor.EditorUtility.SetDirty(existingRule);
                }
                else
                {
                    DTileSetPrototypeRule newRule = ScriptableObject.CreateInstance<DTileSetPrototypeRule>();
                    newRule.SetUp(prototypeRuleData);

                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                    }

                    UnityEditor.AssetDatabase.CreateAsset(newRule, path);
                }
            }

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        [ContextMenu("Generate Tag Rules")]
        public void GenerateTagRules()
        {
#if UNITY_EDITOR
            if (rules.tagRule == null) return;

            foreach (var tagRuleData in rules.tagRule)
            {
                string path = System.IO.Path.Combine(tagRulePath, $"{tagRuleData.name}.asset");
                DTileSetTagRule existingRule = UnityEditor.AssetDatabase.LoadAssetAtPath<DTileSetTagRule>(path);

                if (existingRule != null)
                {
                    existingRule.SetUp(tagRuleData);
                    UnityEditor.EditorUtility.SetDirty(existingRule);
                }
                else
                {
                    DTileSetTagRule newRule = ScriptableObject.CreateInstance<DTileSetTagRule>();
                    newRule.SetUp(tagRuleData);

                    if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
                    {
                        System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
                    }

                    UnityEditor.AssetDatabase.CreateAsset(newRule, path);
                }
            }

            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}

