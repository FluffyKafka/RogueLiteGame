using ObjectGenerateData;
using StatsData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SkillSystem
{
    internal class SMClone : SMSkillModel
    {
        [SerializeField] protected int attackTypeCount;
        [SerializeField] protected float xPositionOffset;
        [SerializeField] protected float minDamageTransfer;

        [Header("Test")]
        protected bool canAttack = false;
        protected DDamageData damage;

        public void SetCanAttack()
        {
            canAttack = true;
        }

        public void GeneratePlayerClone()
        {
            int playerFacingDir = manager.CheckPlayerFacingDir();
            Vector3 clonePosition = manager.CheckPlayerTransform().position;
            clonePosition.x += playerFacingDir * xPositionOffset;

            damage = manager.CheckPlayerPrimaryDamage().Clone();
            damage.physical *= minDamageTransfer;
            damage.magical *= minDamageTransfer;

            manager.GeneratePlayerCloneAt(
                new DPlayerCloneData(new WReadOnlyDamageData(damage), canAttack, attackTypeCount), 
                clonePosition
                );
        }
    }
}

