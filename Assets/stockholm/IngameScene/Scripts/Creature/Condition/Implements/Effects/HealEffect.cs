using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealEffect : Buff {

    public int basicTickHealAmount = 30;
    public int healIncreaseRateByLevel = 0;
    private int finalTickHealAmount = 30;

    new void Awake()
    {
        base.Awake();
        finalTickHealAmount = basicTickHealAmount + conditionLevel * healIncreaseRateByLevel;
    }

    public override void applyEffect()
    {
        target.Heal(finalTickHealAmount);
    }

    public override void revokeEffect()
    {
        throw new NotImplementedException();
    }
    
}
