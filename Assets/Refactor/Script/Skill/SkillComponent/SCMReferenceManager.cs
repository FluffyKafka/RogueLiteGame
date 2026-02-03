using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SkillSystem
{
    internal class SCMReferenceManager : MonoBehaviour
    {
        [SerializeField] protected string assetDirectorPath;
        [SerializeField] protected List<SCBase> skillComponents;

        public SMSkillModel[] models;
        protected void Awake()
        {
            models = GetComponents<SMSkillModel>();
            foreach(var sc in skillComponents)
            {
                sc.Init(models);
            }
        }
#if UNITY_EDITOR
        [ContextMenu("Update Skill Components")]
        protected void UpdateSCDatabase()
        {
            string[] assetNames = AssetDatabase.FindAssets("t:SCBase", new[] { assetDirectorPath });
            skillComponents.Clear();

            foreach (string SOName in assetNames)
            {
                var SOPath = AssetDatabase.GUIDToAssetPath(SOName);
                var sc = AssetDatabase.LoadAssetAtPath<SCBase>(SOPath);         
                skillComponents.Add(sc);              
            }            
        }
#endif
    }
}
