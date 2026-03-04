using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/UnlockEffect/SetCloneCanAttack")]
    internal class SCSetCloneCanAttack : SCUnlockEffectBase
    {
        protected SMClone clone;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            clone = TryGetModel<SMClone>(_modelManager);
        }

        public override void Effect()
        {
            base.Effect();
            clone.SetCanAttack();
        }
    }
}

