using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotDamageEffect : Debuff
{
    public int basicTickDamage = 1;
    public int damageIncreaseRateByLevel = 1;
    private int finalTickDamage = 0;

    new void Awake()
    {
        base.Awake();
        finalTickDamage = basicTickDamage + conditionLevel * damageIncreaseRateByLevel;
    }

    public override void applyEffect()
    {
        target.Damage(finalTickDamage);
    }

    public override void revokeEffect()
    {
        //throw new NotImplementedException();
    }
}
