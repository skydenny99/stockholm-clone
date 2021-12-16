using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Debuff : Effect {

    new void Awake()
    {
        isBuff = false;
        base.Awake();
    }
    
}
