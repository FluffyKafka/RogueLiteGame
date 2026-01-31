using UnityEngine;

namespace EntityBehaviour
{
    internal class CEntityBattle : CEntityComponentBase
    {
        [SerializeField] protected Transform attackValidCheck;
        [SerializeField] protected float attackValidCheckRadius;

        protected bool canBeDamage_current;
        protected bool canBeDamage;

        override protected void Awake()
        {
            base.Awake();

            entity.CanBeDamage += CanBeDamage;
            entity.SetCanBeDamage += SetCanBeDamage;
        }

        protected void SetCanBeDamage(MEntityBehaviour.CanBeDamageSetData _data)
        {
            if (_data.isSetToDefault)
            {
                canBeDamage_current = canBeDamage;
            }
            else
            {
                canBeDamage_current = _data.canBeDamage;
                if (!_data.isTempSetting)
                {
                    canBeDamage = _data.canBeDamage;
                }
            }
        }

        protected bool CanBeDamage()
        {
            return canBeDamage_current;
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackValidCheck.position, attackValidCheckRadius);
        }
    }
}

