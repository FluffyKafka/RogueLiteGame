using Item;
using PlayerSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace ObjectController
{
    internal class ADropItem : AObjectController
    {
        [SerializeField] protected float pickUpDelay = 0.4f;

        protected IItem item;

        public void Setup(FCDropItemFactory _factory, IItem _item)
        {
            factory = _factory;

            Assert.IsNotNull(_item, "不允许使用null初始化ADropItem对象");

            item = _item;
            anim.InitAnimImage(_item.CheckData().CheckIcon());
            InvokeAction(OriginProjectToward, 0);
            InvokeAction(ResetTrigger);
            HitPlayer += PlayerPickUp;
        }

        public override void Clear()
        {
            base.Clear();
            item = null;
            HitPlayer -= PlayerPickUp;
        }

        protected void PlayerPickUp(IObjectPlayer _player)
        {
            InvokeAction(SecondaryProjectToward, 0);
            StartCoroutine(PickUpAfter(_player));
        }
        protected IEnumerator PickUpAfter(IObjectPlayer _player)
        {
            yield return new WaitForSeconds(pickUpDelay);
            if (_player.TryTakeItem(item))
            {
                SelfRecycle();
            }
        }
    }
}
