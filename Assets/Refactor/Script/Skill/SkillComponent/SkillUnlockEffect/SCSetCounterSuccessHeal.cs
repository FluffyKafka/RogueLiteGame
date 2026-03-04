using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/UnlockEffect/SetCounterSuccessHeal")]
    internal class SCSetCounterSuccessHeal : SCUnlockEffectBase
    {
        protected SMCounterAttack counter;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            counter = TryGetModel<SMCounterAttack>(_modelManager);
        }

        public override void Effect()
        {
            base.Effect();
            counter.SetCounterSuccessHeal();
        }
    }
}

