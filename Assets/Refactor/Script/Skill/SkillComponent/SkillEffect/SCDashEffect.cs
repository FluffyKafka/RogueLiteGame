using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    [CreateAssetMenu(fileName = "New Skill Entity", menuName = "Skill System/Skill Component/Effects/DashEffect")]
    internal class SCDashEffect : SCEffectBase
    {
        protected SMDash dashModel;

        public override void Init(SMSkillModel[] _modelManager)
        {
            base.Init(_modelManager);
            dashModel = TryGetModel<SMDash>(_modelManager);
        }

        public override void Effect(string _id)
        {
            base.Effect(_id);
            dashModel.Dash();
        }
    }
}