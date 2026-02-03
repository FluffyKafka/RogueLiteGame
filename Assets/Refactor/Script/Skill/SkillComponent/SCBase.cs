using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal abstract class SCBase : ScriptableObject
    {
        public virtual void Init(SMSkillModel[] _modelManager)
        {
            
        }
        protected T TryGetModel<T>(SMSkillModel[] _modelManager) where T : SMSkillModel
        {
            foreach (var model in _modelManager)
            {
                if (model is T)
                {
                    return model as T;
                }
            }
            return null;
        }
    }
}