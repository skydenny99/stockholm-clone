using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConditionReactableObject : ReactableObject
{

    public Condition conditionWillBetossed;

    public override void React(Collider2D col)
    {

        //TODO : 액션 던지는 식으로 변경 예정
        Condition c = Instantiate(conditionWillBetossed);
        target_.transform.GetComponentInChildren<ConditionManager>().AddCondition(c);
    }


}
