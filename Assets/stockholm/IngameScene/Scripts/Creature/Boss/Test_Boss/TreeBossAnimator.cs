using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBossAnimator : BossAnimator {

    new void Start()
    {
        base.Start();
    }
    

    public void setAttackPos(int i)
    {
        _anim.SetInteger("AttackPos", i);
    }
}
