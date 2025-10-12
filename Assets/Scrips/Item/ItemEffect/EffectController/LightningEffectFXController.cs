using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEffectFXController : EffectFXControllerBase
{
    public override void PlayFX(EffectExcuteData _targetData, float _lifeTime, bool _isFaceToEnemy)
    {
        Vector2 dir = _targetData.target.transform.position - PlayerManager.instance.player.transform.position;
        transform.right = dir;
        transform.position = PlayerManager.instance.player.transform.position;
    }
}
