using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindEffect : Debuff {

    public float basicLengthOfView = 1f;
    public float lengthDecreaseRateByLevel= 0f;
    public float finalLengthOfView = 0;

    new void Start()
    {
        finalLengthOfView = basicLengthOfView + lengthDecreaseRateByLevel * conditionLevel;
    }

    public override void applyEffect()
    {
        //TODO : 실명 코드 적용
    }

    public override void revokeEffect()
    {
        throw new NotImplementedException();
    }
    
}
