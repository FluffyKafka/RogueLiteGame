using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/Aimming Effect/SwordAimming ")]
    internal class SCSwordAimming : SCAimmingEffectBase
    {
        protected SMSword sword;
        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);

            sword = TryGetModel<SMSword>(_modelManager);
        }

        public override void AimmingStart(string _id)
        {
            base.AimmingStart(_id);
            sword.AimmingBegin();
        }

        public override void AimmingFinish(string _id)
        {
            base.AimmingFinish(_id);
            sword.AimmingEnd();
        }
    }
}