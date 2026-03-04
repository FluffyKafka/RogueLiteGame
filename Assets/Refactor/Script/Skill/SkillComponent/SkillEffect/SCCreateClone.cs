using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/Effects/CreateClone")]
    internal class SCCreateClone : SCEffectBase
    {
        protected SMClone clone;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            clone = TryGetModel<SMClone>(_modelManager);
        }

        public override void Effect(string _id)
        {
            base.Effect(_id);
            clone.GeneratePlayerClone();            
        }
    }
}

