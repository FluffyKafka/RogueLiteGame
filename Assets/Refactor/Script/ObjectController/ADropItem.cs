using EntitySystem.EntityActor.PlayerActor;
using Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace ObjectController
{
    internal class ADropItem : AObjectController
    {
        protected FCDropItemFactory factory;

        protected IItem item;

        public void Setup(FCDropItemFactory _factory, IItem _item)
        {
            factory = _factory;

            Assert.IsNotNull(_item, "不允许使用null初始化ADropItem对象");

            item = _item;
            InvokeAction(InitAnimSprite, _item.CheckData().CheckIcon());
            InvokeAction(OriginProjectToward, 0);
            InvokeAction(ResetTrigger);
            HitPlayer += PlayerPickUp;
        }

        public void Clear()
        {
            item = null;
        }

        protected void PlayerPickUp(IObjectPlayer _player)
        {
            InvokeAction(SecondaryProjectToward, 0);
            if(_player.TryTakeItem(item))
            {
                SelfRecycle();
            }
        }

        protected override void SelfRecycle()
        {
            InvokeAction(SelfRecycleNotice);
            factory.RecycleDropItem(this);
        }
    }
}
