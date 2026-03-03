using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Component", menuName = "Skill System/Skill Component/UnlockEffect/SetSwordType")]
    internal class SCSetSwordType : SCUnlockEffectBase
    {
        [SerializeField] protected ESwordType type;

        protected SMSword swordModel;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            swordModel = TryGetModel<SMSword>(_modelManager);
        }

        public override void Effect()
        {
            base.Effect();
            swordModel.SetSwordType(type);
        }
    }
}

