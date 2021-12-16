using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : Debuff {

    public float basicDecreaseMovementSpeed = 0.9f;
    public float movementSpeedDecreaseRateByLevel = -0.1f;
    private float finalDecreaseMovementSpeed = 0;

    public float basicIncreaseRollTime = 1.1f;
    public float RollTimeIecreaseRateByLevel = 0.1f;
    private float finalIncreaseRollTime = 0;

    public float basicDecreaseAttackSpeed = 0.9f;
    public float AttackSpeedDecreaseRateByLevel = -0.1f;
    private float finalDecreaseAttackSpeed = 0;

    new void Start()
    {
        finalDecreaseMovementSpeed = basicDecreaseMovementSpeed + movementSpeedDecreaseRateByLevel * conditionLevel;
        finalIncreaseRollTime = basicIncreaseRollTime + RollTimeIecreaseRateByLevel * conditionLevel;
        finalDecreaseAttackSpeed = basicDecreaseAttackSpeed + AttackSpeedDecreaseRateByLevel * conditionLevel;
    }

    public override void applyEffect()
    {
        //TODO : 스테뭐시기 만들면 개발시작
    }

    public override void revokeEffect()
    {
        throw new NotImplementedException();
    }
    
}
